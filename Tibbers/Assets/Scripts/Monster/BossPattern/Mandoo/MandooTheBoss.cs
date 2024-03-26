using System.Collections;
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

    [SerializeField] private MANDOO_STATE _currentMandooState = MANDOO_STATE.FROZEN;

    // Coroutine
    private Coroutine _currentMoveCoroutine = null;
    private Coroutine _currentAttactCoroutine = null;

    // Unit
    private Unit _unit;
    public Unit Unit 
    { 
        get { return _unit; }
        set { _unit = value; }
    }
    private CircleCollider2D _mandooCollider;

    // Frozen Value
    public float maxFrozenValue = 30f;
    public float currentFrozenValue;

    // UI
    private BossUI _bossUI;
    #endregion 변수 선언부

    #region 만두 머리
    [SerializeField] private GameObject _madMandooHead = null;

    private Coroutine _madMandooHeadCoroutine = null;
    private int _mandooBodyDashCount = 0;
    #endregion

    #region MonsterMove 관련 변수 선언부
    // Monster Move 선언
    private MonsterMove _monsterMove;

    // 기본 이속
    public float randomMoveDuration = 1.0f;

    // 돌진 관련
    public float dashSpeed = 10.0f;     // 돌진 속도
    public float dashInterval = 5.0f;   // 돌진 간격
    public float dashDuration = 0.2f;     // 돌진 지속 시간

    // 이동 타겟
    public Transform targetTransform;

    // 타이머
    private float _currentTime = 0;

    public GameObject targetPositionMarker = null;
    #endregion MonsterMove 관련 변수 선언부

    public MandooAnimation mandooAnimation = null;

    [SerializeField] private MandooEffectAreaController _effectAreaController;

    #region 라이프 사이클
    private void InitializeUnitValues()
    {
        _unit = GetComponent<Unit>();

        _unit.m_stStat.fDamage_Base = 10.0f;

        _unit.m_stStat.fHp_Base = 100.0f;
        _bossUI.SetMaxHP(_unit.m_stStat.fHp_Base);

        _unit.m_stStat.fMoveSpeed_Base = 0.2f;

        _unit.ResetHp();
    }

    private void Start()
    {
        // Get from BossManager
        targetTransform = BossManager.Instance.PlayerTransform;
        _bossUI = BossManager.Instance.BossUI;

        InitializeUnitValues();

        // Move Init
        _monsterMove = new MonsterMove();
        _monsterMove.Initialize(targetPositionMarker);

        _effectAreaController = transform.parent.GetComponentInChildren<MandooEffectAreaController>();

        FrozenInit();
    }

    private void FixedUpdate()
    {
        _currentTime += Time.fixedDeltaTime;
        _bossUI.UpdateHPSlider(_unit.m_stStat.fHp_Cur);
        switch (_currentMandooState)
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

    #region Coroutine
    private void ResetCoroutine()
    {
        if (_currentMoveCoroutine != null)
        {
            StopCoroutine(_currentMoveCoroutine);
        }
        if (_currentAttactCoroutine != null)
        {
            StopCoroutine(_currentAttactCoroutine);
        }
        if (_madMandooHeadCoroutine != null)
        {
            StopCoroutine(_madMandooHeadCoroutine);
        }
    }
    #endregion

    #region Frozen
    private void FrozenInit()
    {
        // 무적
        gameObject.tag = "tag_Enemy_Invincible";
        // Value
        currentFrozenValue = maxFrozenValue;
        _bossUI.InitFrozenSlider(0f, maxFrozenValue);

        _currentMandooState = MANDOO_STATE.FROZEN;

        _effectAreaController.SpawnFireArea(targetTransform, transform.parent);

        mandooAnimation.FrozenAnimation(true);
    }

    private void FrozenPattern()
    {
        if (_currentTime > dashInterval)
        {
            Vector3 targetDirection = _monsterMove.SetDashPosition(transform, targetTransform);

            if (_currentMoveCoroutine != null)
            {
                StopCoroutine(_currentMoveCoroutine);
            }
            _currentMoveCoroutine = StartCoroutine(_monsterMove.DashToTarget(transform, targetDirection, dashSpeed, dashDuration));

            if (_currentAttactCoroutine != null)
            {
                StopCoroutine(_currentAttactCoroutine);
            }
            _currentAttactCoroutine = StartCoroutine(_effectAreaController.SpawnIceArea(transform, dashSpeed, dashDuration));

            _currentTime = 0;
        }

        if (currentFrozenValue <= 0f)
        {
            FrozenEnd();
            NormalInit();
        }
    }

    private void FrozenEnd()
    {
        // 무적 종료
        gameObject.tag = "tag_Enemy";

        ResetCoroutine();

        // 애니메이션 종료
        mandooAnimation.FrozenAnimation(false);

        // 장판을 지운다
        _effectAreaController.DestroyFireArea();
    }

    public void GetMelt(float value)
    {
        currentFrozenValue -= value;
        _bossUI.bossFrozenSlider.value = currentFrozenValue;
    }
    #endregion

    #region Mad
    // init
    private void MadInit()
    {
        _currentMandooState = MANDOO_STATE.MAD;

        ThrowHead();

        StartCoroutine(mandooAnimation.StartMad(0.75f));
    }

    private void ThrowHead()
    {
        mandooAnimation.StartBodyThrow();
        _madMandooHeadCoroutine = StartCoroutine(HeadInitialMove());
    }

    private IEnumerator HeadInitialMove()
    {
        Vector3 direction = (targetTransform.position - _madMandooHead.transform.position).normalized;
        var speed = 0.5f;
        _madMandooHead.GetComponent<Rigidbody2D>().velocity = direction * speed;
        yield break;
    }

    // random body
    private void RandomBodyDash()
    {
        Vector3 targetDirection = _monsterMove.SetRandomDirection();

        if (_currentMoveCoroutine != null)
        {
            StopCoroutine(_currentMoveCoroutine);
        }
        _currentMoveCoroutine = StartCoroutine(_monsterMove.DashToTarget(transform, targetDirection, dashSpeed, dashDuration));

        _currentTime = 0;
        _mandooBodyDashCount = 0;
    }

    private void ShortBodyJumpToHead()
    {
        if (_currentMoveCoroutine != null)
        {
            StopCoroutine(_currentMoveCoroutine);
        }

        _currentMoveCoroutine = StartCoroutine(_monsterMove.JumpToTarget(_mandooCollider, targetTransform, transform, dashSpeed, dashDuration));
    }

    // head idle
    private void HeadMove()
    {
        _monsterMove.FollowTarget(_unit.fCurMoveSpeed, _madMandooHead.transform, targetTransform);
    }

    private void MadPattern()
    {
        // body
        if (_currentTime > dashInterval)
        {
            if (_mandooBodyDashCount > 3)
            {
                RandomBodyDash();
            }
            else
            {
                ShortBodyJumpToHead();
            }
        }

        // 만두 머리는 살살 쩜프하면서 따라다님
        HeadMove();

        // 사망
        if (_unit.m_stStat.fHp_Cur == 0)
        {
            MadEnd();
            Dead();
        }

    }

    private void MadEnd()
    {
        ResetCoroutine();

        Destroy(_madMandooHead);
    }
    #endregion

    #region Normal
    private void NormalInit()
    {
        _currentMandooState = MANDOO_STATE.NORMAL;
    }

    private void NormalPattern()
    {
        _monsterMove.FollowTarget(_unit.fCurMoveSpeed, transform, targetTransform);

        _currentTime += Time.deltaTime;


        // 탄환
        if (_currentTime >= 3.0f)
        {
            _currentAttactCoroutine = StartCoroutine(MandooShotBullet(1f, 3));
            _currentTime = 0f;
        }

        // 광란 패턴
        // if (_unit.m_stStat.fHp_Cur <= 30)
        if (_unit.m_stStat.fHp_Cur <= 80)
        {
            NormalEnd();
            MadInit();
        }
    }

    private IEnumerator MandooShotBullet(float delay, int number = 1)
    {
        mandooAnimation.StartThrow();

        Vector3 originDirection = targetTransform.position - transform.position;
        float startAngle = Mathf.Atan2(originDirection.y, originDirection.x) * Mathf.Rad2Deg - (number - 1) * 10.0f; // 시작 각도 조정

        for (int i = 0; i < number; i++)
        {
            // 각 총알에 대한 방향 계산
            float angle = startAngle + (20.0f * i); // 20.0f는 각 총알 간의 각도 차이입니다. 이 값을 조정하여 총알 간의 간격을 변경할 수 있습니다.
            Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);

            // 총알 발사
            BossBullet bullet = BossManager.Instance.ShotBullet(BossBulletType.mandooBullet, direction, transform).GetComponent<BossBullet>();
            bullet.InitialMoveInfo(direction, 8.0f);

        }
        yield return new WaitForSeconds(delay);
        mandooAnimation.EndThrow();
    }
    //private IEnumerator MandooShotBullet(float delay, int number = 1) {
    //    mandooAnimation.StartThrow();

    //    Vector3 direction = targetTransform.position - transform.position;

    //    BossBullet bullet = BossManager.Instance.ShotBullet(BossBulletType.mandooBullet, direction, transform).GetComponent<BossBullet>();
    //    bullet.InitialMoveInfo(direction, 8.0f);

    //    yield return new WaitForSeconds(delay);

    //    mandooAnimation.EndThrow();
    //}

    private void NormalEnd()
    {
        ResetCoroutine();
    }
    #endregion

    #region Dead
    private void Dead()
    {
        _currentMandooState = MANDOO_STATE.DEAD;
        BossManager.Instance.BossClear();
    }
    #endregion

    #region
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("tag_Player"))
        {
            collision.GetComponent<Unit>().GetDamage(_unit.m_stStat.fDamage_Base);
        }
    }
    #endregion
}
