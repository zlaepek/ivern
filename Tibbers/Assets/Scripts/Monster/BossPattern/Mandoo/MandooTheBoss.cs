using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MandooTheBoss : MonoBehaviour
{
    #region ���� �����
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
    #endregion ���� �����

    #region ���� �Ӹ�
    [SerializeField] private GameObject _madMandooHead = null;

    private Coroutine _madMandooHeadCoroutine = null;
    private int _mandooBodyDashCount = 0;
    #endregion

    #region MonsterMove ���� ���� �����
    // Monster Move ����
    private MonsterMove _monsterMove;

    // �⺻ �̼�
    public float randomMoveDuration = 1.0f;

    // ���� ����
    public float dashSpeed = 10.0f;     // ���� �ӵ�
    public float dashInterval = 5.0f;   // ���� ����
    public float dashDuration = 0.2f;     // ���� ���� �ð�

    // �̵� Ÿ��
    public Transform targetTransform;

    // Ÿ�̸�
    private float _currentTime = 0;

    public GameObject targetPositionMarker = null;
    #endregion MonsterMove ���� ���� �����

    public MandooAnimation mandooAnimation = null;

    [SerializeField] private MandooEffectAreaController _effectAreaController;

    #region ������ ����Ŭ
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
    #endregion ������ ����Ŭ

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
        // ����
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
        // ���� ����
        gameObject.tag = "tag_Enemy";

        ResetCoroutine();

        // �ִϸ��̼� ����
        mandooAnimation.FrozenAnimation(false);

        // ������ �����
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

        // ���� �Ӹ��� ��� �����ϸ鼭 ����ٴ�
        HeadMove();

        // ���
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


        // źȯ
        if (_currentTime >= 3.0f)
        {
            _currentAttactCoroutine = StartCoroutine(MandooShotBullet(1f, 3));
            _currentTime = 0f;
        }

        // ���� ����
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
        float startAngle = Mathf.Atan2(originDirection.y, originDirection.x) * Mathf.Rad2Deg - (number - 1) * 10.0f; // ���� ���� ����

        for (int i = 0; i < number; i++)
        {
            // �� �Ѿ˿� ���� ���� ���
            float angle = startAngle + (20.0f * i); // 20.0f�� �� �Ѿ� ���� ���� �����Դϴ�. �� ���� �����Ͽ� �Ѿ� ���� ������ ������ �� �ֽ��ϴ�.
            Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);

            // �Ѿ� �߻�
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
