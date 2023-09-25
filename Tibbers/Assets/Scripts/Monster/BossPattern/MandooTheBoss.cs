using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class MandooTheBoss : MonoBehaviour
{

    #region 변수 선언부
    private enum MANDOO_STATE
    {
        FROZEN,
        NORMAL,
        MAD,
        DEAD
    }

    [SerializeField] private MANDOO_STATE currentMandooState = MANDOO_STATE.FROZEN;

    // 코루틴
    private Coroutine currentMoveCoroutine = null;
    private Coroutine currentAttactCoroutine = null;


    #endregion 변수 선언부

    #region 만두 머리
    [SerializeField] private GameObject madMandooHeadPrefab = null;
    private GameObject madMandooHead = null;

    private Coroutine madMandooHeadCoroutine = null;
    private int mandooBodyJumpCount = 0;
    #endregion

    #region MonsterMove 관련 변수 선언부
    // Monster Move 선언
    private MonsterMove monsterMove;

    // 기본 이속
    public float speed = 1.0f;
    public float randomMoveDuration = 1.0f;

    // 돌진 관련
    public float dashSpeed = 10.0f;     // 돌진 속도
    public float dashInterval = 5.0f;   // 돌진 간격
    public float dashDuration = 0.2f;     // 돌진 지속 시간

    // 이동 타겟
    public Transform targetTransform;

    // this 오브젝트
    private Rigidbody2D thisRigidbody2D;
    private Transform thisTransform;

    // 타이머
    private float currentTime = 0;

    public GameObject targetPositionMarker = null;
    #endregion MonsterMove 관련 변수 선언부

    #region 장판 변수 선언부
    [SerializeField] private EffectArea effectArea;
    #endregion
    #region 라이프 사이클

    private void Start()
    {
        monsterMove = new MonsterMove();

        monsterMove.Initialize(targetPositionMarker); 

        thisRigidbody2D = GetComponentInChildren<Rigidbody2D>();
        thisTransform = GetComponentInChildren<Transform>();

        effectArea = thisTransform.parent.GetComponentInChildren<EffectArea>();

        FrozenInit();
        MadInit();
    }

    private void FixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;
        switch (currentMandooState)
        {
            case MANDOO_STATE.FROZEN:
                FrozenPattern();
                break;
            case MANDOO_STATE.NORMAL:
                NormalPattern();
                break;
            case MANDOO_STATE.MAD:
                MadPattern();
                break;
            case MANDOO_STATE.DEAD:
                Dead();
                break;
        }
    }
    #endregion 라이프 사이클

    #region Frozen
    private void FrozenInit()
    {
        // 후라이팬 장판 소환
        effectArea.SpawnFireArea(targetTransform, transform.parent);
    }
    private void FrozenPattern()
    {
        // 슬라이드 & 냉기 장판
        if (currentTime > dashInterval)
        {
            Vector3 targetDirection = monsterMove.SetDashPosition(thisTransform, targetTransform);
            if (currentMoveCoroutine != null)
            {
                StopCoroutine(currentMoveCoroutine);
            }
            currentMoveCoroutine = StartCoroutine(monsterMove.DashToTarget(thisTransform, targetDirection, dashSpeed, dashDuration));
            if (currentAttactCoroutine != null)
            {
                StopCoroutine(currentAttactCoroutine);
            }
            currentAttactCoroutine = StartCoroutine(effectArea.SpawnIceArea(thisTransform, dashSpeed, dashDuration));
            currentTime = 0;
        }

        // 다 녹았을 때
        if (false)
        {
            FrozenEnd();
            NormalInit();
        }
    }

    private void FrozenEnd()
    {
        if (currentMoveCoroutine != null)
        {
            StopCoroutine(currentMoveCoroutine);
        }
        if (currentAttactCoroutine != null)
        {
            StopCoroutine(currentAttactCoroutine);
        }
        // 장판을 지운다
        effectArea.DestroyFireArea();
    }
    #endregion

    #region Mad
    private void MadInit()
    {
        // 머리 몸통 분리
        madMandooHead = Instantiate(madMandooHeadPrefab, transform.parent);
    }
    private void MadPattern()
    {

        // (3번의 돌진 후) 점프
        if (mandooBodyJumpCount > 3 && currentTime > dashInterval)
        {
            if (currentMoveCoroutine != null)
            {
                StopCoroutine(currentMoveCoroutine);
            }
            currentMoveCoroutine = StartCoroutine(monsterMove.JumpToTarget(targetTransform, thisTransform));
            currentTime = 0;
            mandooBodyJumpCount = 0;
        }
        // 머리 랜덤 방향 돌진
        else if (currentTime > dashInterval)
        {
            if (madMandooHeadCoroutine != null)
            {
                StopCoroutine(madMandooHeadCoroutine);
            }
            madMandooHeadCoroutine = StartCoroutine(monsterMove.RandomMove(madMandooHead.transform, speed, randomMoveDuration));
            currentTime = 0;
            mandooBodyJumpCount++;
        }


        //TODO: 종료 상태 결정
        if (false) // hp가 0이면 사망
        {
            MadEnd();
            Dead();
        }
        if (false) // hp가 일정 깎였을 때 
        {
            MadEnd();
            NormalInit();
        }
    }

    private void MadEnd()
    {
        if (currentMoveCoroutine != null)
        {
            StopCoroutine(currentMoveCoroutine);
        }
        if (madMandooHeadCoroutine != null)
        {
            StopCoroutine(madMandooHeadCoroutine);
        }
        if (currentAttactCoroutine != null)
        {
            StopCoroutine(currentAttactCoroutine);
        }
        Destroy(madMandooHead);
    }
    #endregion

    #region Normal
    private void NormalInit()
    {
        currentMandooState = MANDOO_STATE.NORMAL;

    }
    private void NormalPattern()
    {
        // 이동
        monsterMove.FollowTarget(speed, thisTransform, targetTransform);

        //TODO: 탄환 던지기

        //TODO: 체력이 지정된 상태가 되면, ex) 50퍼 미만 => 발악 패턴
        //currentMandooState = MANDOO_STATE.MAD;
        //MadInit();

        if (false) // hp가 0이면 사망
        {
            NormalEnd();
            Dead();
        }
        if (false) // hp가 일정 깎였을 때 => 광란패턴
        {
            NormalEnd();
            MadInit();
        }
    }
    private void NormalEnd()
    {
        if (currentMoveCoroutine != null)
        {
            StopCoroutine(currentMoveCoroutine);
        }
        if (currentAttactCoroutine != null)
        {
            StopCoroutine(currentAttactCoroutine);
        }
    }
    #endregion

    #region Dead
    private void Dead()
    {
        //TODO: 보스 관련 오브젝트 죄다 삭제
        //TODO: 보스 죽는 모션
        //TODO: 보상으로 이동
    }
    #endregion
}
