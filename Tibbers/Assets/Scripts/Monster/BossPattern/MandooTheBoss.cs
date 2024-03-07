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
    private CircleCollider2D _mandooCollider;

    // Frozen Value
    public float maxFrozenValue = 30f;
    public float currentFrozenValue;

    // UI
    private BossUI _bossUI;
    #endregion 변수 선언부

    #region 만두 머리
    [SerializeField] private GameObject _madMandooHeadPrefab = null;
    private GameObject _madMandooHead = null;

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

        _unit.m_stStat.fMoveSpeed_Base = 1.0f;

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
    private void MadInit()
    {
        _currentMandooState = MANDOO_STATE.MAD;

        ThrowHead();

        StartCoroutine(mandooAnimation.StartMad(0.75f, _madMandooHead));

        // 만두 머리 운동 시작
        _madMandooHeadCoroutine = StartCoroutine(HeadMove(2.0f));
    }

    private void ThrowHead()
    {
        mandooAnimation.StartBodyThrow();
        _madMandooHead = Instantiate(_madMandooHeadPrefab, transform.parent);
        _madMandooHeadCoroutine = StartCoroutine(HeadInitialMove());
    }

    private IEnumerator HeadInitialMove() {
        Vector3 direction = (targetTransform.position - _madMandooHead.transform.position).normalized;
        var speed = 0.5f;
        _madMandooHead.GetComponent<Rigidbody2D>().velocity = direction * speed;
        yield break;
    }

    private IEnumerator HeadMove(float delay)
    {
        // _madMandooHead.GetComponent<MonsterMove>().JumpToTarget();

        yield return null;
    }

    private void RandomBodyDash()
    {
        if (_currentMoveCoroutine != null)
        {
            StopCoroutine(_currentMoveCoroutine);
        }
        _currentMoveCoroutine = StartCoroutine(_monsterMove.JumpToTarget(_mandooCollider, targetTransform, transform, dashSpeed, dashDuration));
        _currentTime = 0;
        _mandooBodyDashCount = 0;
    }

    private void MadPattern()
    {
        // (3번의 돌진 후) 점프
        if (_mandooBodyDashCount > 3 && _currentTime > dashInterval)
        {
            RandomBodyDash();
        }

        // 머리 랜덤 방향 점프
        else if (_currentTime > dashInterval)
        {
            if (_madMandooHeadCoroutine != null)
            {
                StopCoroutine(_madMandooHeadCoroutine);
            }
            if (_madMandooHead != null)
            {
                _madMandooHeadCoroutine = StartCoroutine(_monsterMove.JumpToTarget(_madMandooHead.GetComponent<CircleCollider2D>(), targetTransform, _madMandooHead.transform, _unit.fCurMoveSpeed, randomMoveDuration));
                _currentTime = 0;
                _mandooBodyDashCount++;
            }
            else
            {
                MadInit();
            }
        }

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
        if (_currentTime >= 3.0f) {
            _currentAttactCoroutine = StartCoroutine(MandooShotBullet(1f));
            _currentTime = 0f;
        }

        // 광란 패턴
        if (_unit.m_stStat.fHp_Cur <= 30)
        {
            NormalEnd();
            MadInit();
        }
    }

    private IEnumerator MandooShotBullet(float delay) {
        mandooAnimation.StartThrow();

        Vector3 direction = targetTransform.position - transform.position;
        BossBullet bullet = BossManager.Instance.ShotBullet(BossBulletType.mandooBullet, direction, transform).GetComponent<BossBullet>();
        bullet.InitialMoveInfo(direction, 2.0f);

        yield return new WaitForSeconds(delay);

        mandooAnimation.EndThrow();
    }

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
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("tag_Player")) {
            collision.GetComponent<Unit>().GetDamage(_unit.m_stStat.fDamage_Base);
        }
    }
    #endregion
}
