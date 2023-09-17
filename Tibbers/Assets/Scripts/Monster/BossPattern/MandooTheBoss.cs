using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class MandooTheBoss : Unit
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

    private Vector3 targetDirection;
    #endregion MonsterMove 관련 변수 선언부

    #region 장판 변수 선언부
    private EffectArea effectArea;
    #endregion
    #region 라이프 사이클

    private void Start() {
        monsterMove = new MonsterMove();


        thisRigidbody2D = GetComponentInChildren<Rigidbody2D>();
        thisTransform = GetComponentInChildren<Transform>();

        effectArea = GetComponent<EffectArea>();

        FrozenInit();
    }

    private void FixedUpdate() {
        currentTime += Time.fixedDeltaTime;
        switch (currentMandooState) {
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

    #region 각 스테이트 실행 함수 (Init, Update에서 호출 함수)
    /* Frozen */
    private void FrozenInit() {
        //TODO: 후라이팬 장판 소환
        effectArea.SpawnFireArea(targetTransform);
    }
    private void FrozenPattern() {
        // 슬라이드 & 냉기 장판
        if (currentTime > dashInterval)
        {
            Vector3 targetDirection = monsterMove.SetDashPosition(thisTransform, targetTransform);
            if (currentMoveCoroutine != null) {
                StopCoroutine(currentMoveCoroutine);
            }
            currentMoveCoroutine = StartCoroutine(monsterMove.DashToTarget(thisTransform, targetDirection, dashSpeed, dashDuration));
            if (currentAttactCoroutine != null) {
                StopCoroutine(currentAttactCoroutine);
            }
            currentAttactCoroutine = StartCoroutine(effectArea.SpawnIceArea(thisTransform, dashSpeed, dashDuration));
            currentTime = 0;
        }
        //TODO: 다 녹았을 때
    }

    private void FrozenEnd()
    {
        //TODO: 장판을 지운다
    }
    /* END Frozen */

    /* Mad */
    private void MadInit() {
        //TODO: 후라이팬 장판
    }
    private void MadPattern() {


        //TODO: 점프 (점프 타이머가 다 돌았을 때)
        if (currentTime > dashInterval) {
            Debug.Log(currentTime);
            currentMoveCoroutine = StartCoroutine(monsterMove.JumpToTarget(targetTransform, thisTransform));
            currentTime = 0;
        }
        //TODO: 랜덤 방향 돌진 (점프를 하지 않을 때)
        else
        {
            monsterMove.RandomMove(thisTransform, speed, randomMoveDuration);
        }

        //TODO: 종료 상태 결정
    }
    /* END Mad */

    /* Normal */
    private void NormalInit()
    {
        
    }
    private void NormalPattern() {
        // 이동
        monsterMove.FollowTarget(speed, thisTransform, targetTransform);

        //TODO: 탄환 던지기

        //TODO: 체력이 지정된 상태가 되면, ex) 50퍼 미만 => 발악 패턴
        //currentMandooState = MANDOO_STATE.MAD;
        //MadInit();
    }
    /* End Normal */

    private void Dead() {
        //TODO: 보스 관련 오브젝트 죄다 삭제
        //TODO: 보스 죽는 모션
        //TODO: 보상으로 이동
    }
    #endregion 각 스테이트 실행 함수 (Init, Update에서 호출 함수)
}
