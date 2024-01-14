using UnityEngine;
using UnityEngine.UI;

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

    // Coroutine
    private Coroutine currentMoveCoroutine = null;
    private Coroutine currentAttactCoroutine = null;

    // Unit
    private Unit unit;

    // Frozen Value
    public float maxFrozenValue = 30f;
    public float currentFrozenValue;

    public Slider frozenSlider;

    // UI
    public BossUI bossUI;

    // Animation
    public Animator animator = null;
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
    public float randomMoveDuration = 1.0f;

    // 돌진 관련
    public float dashSpeed = 10.0f;     // 돌진 속도
    public float dashInterval = 5.0f;   // 돌진 간격
    public float dashDuration = 0.2f;     // 돌진 지속 시간

    // 이동 타겟
    public Transform targetTransform;

    // 타이머
    private float currentTime = 0;

    public GameObject targetPositionMarker = null;
    #endregion MonsterMove 관련 변수 선언부

    #region 장판 변수 선언부
    [SerializeField] private MandooEffectAreaController effectAreaController;
    #endregion

    #region 라이프 사이클


    private void Start()
    {
        // get from BossManager
        targetTransform = BossManager.Instance.PlayerTransform;
        bossUI = BossManager.Instance.BossUI;

        // Unit Init
        unit = GetComponent<Unit>();

        unit.m_stStat.fDamage_Base = 2.0f;

        unit.m_stStat.fHp_Base = 100.0f;
        bossUI.SetMaxHP(unit.m_stStat.fHp_Base);

        unit.m_stStat.fMoveSpeed_Base = 1.0f;

        unit.ResetHp();

        // Frozen Init
        frozenSlider = bossUI.bossFrozenSlider;
        bossUI.InitFrozenSlider(0f, maxFrozenValue);
        currentFrozenValue = maxFrozenValue;
        frozenSlider.value = currentFrozenValue;

        // Move Init
        monsterMove = new MonsterMove();
        monsterMove.Initialize(targetPositionMarker);

        effectAreaController = transform.parent.GetComponentInChildren<MandooEffectAreaController>();

        FrozenInit();
    }

    private void FixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;
        bossUI.UpdateHPSlider(unit.m_stStat.fHp_Cur);
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
        currentMandooState = MANDOO_STATE.FROZEN;
        // 후라이팬 장판 소환
        effectAreaController.SpawnFireArea(targetTransform, transform.parent);

        animator.SetBool("isFrozen", true);
    }
    private void FrozenPattern()
    {
        // 슬라이드 & 냉기 장판
        if (currentTime > dashInterval)
        {
            Vector3 targetDirection = monsterMove.SetDashPosition(transform, targetTransform);
            if (currentMoveCoroutine != null)
            {
                StopCoroutine(currentMoveCoroutine);
            }
            currentMoveCoroutine = StartCoroutine(monsterMove.DashToTarget(transform, targetDirection, dashSpeed, dashDuration));
            if (currentAttactCoroutine != null)
            {
                StopCoroutine(currentAttactCoroutine);
            }
            currentAttactCoroutine = StartCoroutine(effectAreaController.SpawnIceArea(transform, dashSpeed, dashDuration));
            currentTime = 0;
        }

        // 다 녹았을 때
        if (currentFrozenValue <= 0f)
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
        effectAreaController.DestroyFireArea();
    }

    public void GetMelt(float value)
    {
        currentFrozenValue -= value;
        frozenSlider.value = currentFrozenValue;
    }
    #endregion

    #region Mad
    private void MadInit()
    {
        currentMandooState = MANDOO_STATE.MAD;
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
            currentMoveCoroutine = StartCoroutine(monsterMove.JumpToTarget(targetTransform, transform));
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
            if (madMandooHead != null)
            {
                madMandooHeadCoroutine = StartCoroutine(monsterMove.RandomMove(madMandooHead.transform, unit.fCurMoveSpeed, randomMoveDuration));
                currentTime = 0;
                mandooBodyJumpCount++;
            }
            else
            {
                MadInit();
            }
        }


        //TODO: 종료 상태 결정
        if (unit.m_stStat.fHp_Cur == 30f) // hp가 일정 깎였을 때 
        {
            MadEnd();
            NormalInit();
        }
        if (unit.m_stStat.fHp_Cur == 0) // hp가 0이면 사망
        {
            MadEnd();
            Dead();
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
        monsterMove.FollowTarget(unit.fCurMoveSpeed, transform, targetTransform);

        //TODO: 탄환 던지기

        if (unit.m_stStat.fHp_Cur == 50) // hp가 일정 깎였을 때 => 광란패턴
        {
            NormalEnd();
            MadInit();
        }
        if (unit.m_stStat.fHp_Cur == 0) // hp가 0이면 사망
        {
            NormalEnd();
            Dead();
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
        currentMandooState = MANDOO_STATE.DEAD;
        BossManager.Instance.BossClear();
        //TODO: 보스 관련 오브젝트 죄다 삭제
        //TODO: 보스 죽는 모션
        //TODO: 보상으로 이동
    }
    #endregion
}
