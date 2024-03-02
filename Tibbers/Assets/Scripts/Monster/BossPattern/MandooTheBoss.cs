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

    [SerializeField] private MANDOO_STATE currentMandooState = MANDOO_STATE.FROZEN;

    // Coroutine
    private Coroutine _currentMoveCoroutine = null;
    private Coroutine _currentAttactCoroutine = null;

    // Unit
    private Unit _unit;
    private CircleCollider2D _mandooCollider;

    // Frozen Value
    public float maxFrozenValue = 30f;
    public float currentFrozenValue;

    public Slider FrozenSlider
    {
        get
        {
            return bossUI.bossFrozenSlider;
        }
        set
        {
            bossUI.bossFrozenSlider = value;
        }
    }

    // UI
    public BossUI bossUI;
    #endregion ���� �����

    #region ���� �Ӹ�
    [SerializeField] private GameObject madMandooHeadPrefab = null;
    private GameObject madMandooHead = null;

    private Coroutine madMandooHeadCoroutine = null;
    private int mandooBodyDashCount = 0;
    #endregion

    #region MonsterMove ���� ���� �����
    // Monster Move ����
    private MonsterMove monsterMove;

    // �⺻ �̼�
    public float randomMoveDuration = 1.0f;

    // ���� ����
    public float dashSpeed = 10.0f;     // ���� �ӵ�
    public float dashInterval = 5.0f;   // ���� ����
    public float dashDuration = 0.2f;     // ���� ���� �ð�

    // �̵� Ÿ��
    public Transform targetTransform;

    // Ÿ�̸�
    private float currentTime = 0;

    public GameObject targetPositionMarker = null;
    #endregion MonsterMove ���� ���� �����

    public MandooAnimation mandooAnimation = null;

    #region ���� ���� �����
    [SerializeField] private MandooEffectAreaController effectAreaController;
    #endregion

    #region ������ ����Ŭ
    private void InitializeUnitValues()
    {
        _unit = GetComponent<Unit>();

        _unit.m_stStat.fDamage_Base = 2.0f;

        _unit.m_stStat.fHp_Base = 100.0f;
        bossUI.SetMaxHP(_unit.m_stStat.fHp_Base);

        _unit.m_stStat.fMoveSpeed_Base = 1.0f;

        _unit.ResetHp();
    }

    private void Start()
    {
        // Get from BossManager
        targetTransform = BossManager.Instance.PlayerTransform;
        bossUI = BossManager.Instance.BossUI;

        InitializeUnitValues();

        // Move Init
        monsterMove = new MonsterMove();
        monsterMove.Initialize(targetPositionMarker);

        effectAreaController = transform.parent.GetComponentInChildren<MandooEffectAreaController>();

        FrozenInit();
    }

    private void FixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;
        bossUI.UpdateHPSlider(_unit.m_stStat.fHp_Cur);
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
        if (madMandooHeadCoroutine != null)
        {
            StopCoroutine(madMandooHeadCoroutine);
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
        bossUI.InitFrozenSlider(0f, maxFrozenValue);

        currentMandooState = MANDOO_STATE.FROZEN;
        
        effectAreaController.SpawnFireArea(targetTransform, transform.parent);

        mandooAnimation.FrozenAnimation(true);
    }

    private void FrozenPattern()
    {
        if (currentTime > dashInterval)
        {
            Vector3 targetDirection = monsterMove.SetDashPosition(transform, targetTransform);

            if (_currentMoveCoroutine != null)
            {
                StopCoroutine(_currentMoveCoroutine);
            }
            _currentMoveCoroutine = StartCoroutine(monsterMove.DashToTarget(transform, targetDirection, dashSpeed, dashDuration));

            if (_currentAttactCoroutine != null)
            {
                StopCoroutine(_currentAttactCoroutine);
            }
            _currentAttactCoroutine = StartCoroutine(effectAreaController.SpawnIceArea(transform, dashSpeed, dashDuration));

            currentTime = 0;
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
        effectAreaController.DestroyFireArea();
    }

    public void GetMelt(float value)
    {
        currentFrozenValue -= value;
        FrozenSlider.value = currentFrozenValue;
    }
    #endregion

    #region Mad
    private void MadInit()
    {
        currentMandooState = MANDOO_STATE.MAD;

        ThrowHead();

        StartCoroutine(mandooAnimation.StartMad(0.75f, madMandooHead));
    }

    private void ThrowHead()
    {
        mandooAnimation.StartBodyThrow();
        madMandooHead = Instantiate(madMandooHeadPrefab, transform.parent);
        madMandooHeadCoroutine = StartCoroutine(HeadInitialMove());
    }

    private IEnumerator HeadInitialMove() {
        Vector3 direction = (targetTransform.position - madMandooHead.transform.position).normalized;
        var speed = 0.5f;
        madMandooHead.GetComponent<Rigidbody2D>().velocity = direction * speed;
        yield break;
    }

    private void RandomBodyDash()
    {
        if (_currentMoveCoroutine != null)
        {
            StopCoroutine(_currentMoveCoroutine);
        }
        _currentMoveCoroutine = StartCoroutine(monsterMove.JumpToTarget(_mandooCollider, targetTransform, transform, dashSpeed, dashDuration));
        currentTime = 0;
        mandooBodyDashCount = 0;
    }

    private void MandooHeadJump()
    {

    }

    private void MadPattern()
    {
        // (3���� ���� ��) ����
        if (mandooBodyDashCount > 3 && currentTime > dashInterval)
        {
            RandomBodyDash();
        }

        // �Ӹ� ���� ���� ����
        else if (currentTime > dashInterval)
        {
            if (madMandooHeadCoroutine != null)
            {
                StopCoroutine(madMandooHeadCoroutine);
            }
            if (madMandooHead != null)
            {
                madMandooHeadCoroutine = StartCoroutine(monsterMove.RandomMove(madMandooHead.transform, _unit.fCurMoveSpeed, randomMoveDuration));
                currentTime = 0;
                mandooBodyDashCount++;
            }
            else
            {
                MadInit();
            }
        }

        //TODO: ���� ���� ����
        if (_unit.m_stStat.fHp_Cur == 30f) // hp�� ���� ���� �� 
        {
            MadEnd();
            NormalInit();
        }
        if (_unit.m_stStat.fHp_Cur == 0) // hp�� 0�̸� ���
        {
            MadEnd();
            Dead();
        }

    }

    private void MadEnd()
    {
        ResetCoroutine();

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
        monsterMove.FollowTarget(_unit.fCurMoveSpeed, transform, targetTransform);

        currentTime += Time.deltaTime;
        //TODO: źȯ ������
        if (currentTime >= 3.0f) {
            Vector3 direction = targetTransform.position - transform.position;
            BossManager.Instance.ShotBullet(BossBulletType.mandooBullet, direction);
            Debug.Log("shot");
            currentTime = 0f;
        }

        if (_unit.m_stStat.fHp_Cur <= 80 && _unit.m_stStat.fHp_Cur <= 70) // hp�� ���� ���� �� => ��������
        {
            NormalEnd();
            MadInit();
        }
        if (_unit.m_stStat.fHp_Cur == 0) // hp�� 0�̸� ���
        {
            NormalEnd();
            Dead();
        }
    }

    private void NormalEnd()
    {
        ResetCoroutine();
    }
    #endregion

    #region Dead
    private void Dead()
    {
        currentMandooState = MANDOO_STATE.DEAD;
        BossManager.Instance.BossClear();
    }
    #endregion

    #region
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "tag_Player") {
            collision.GetComponent<Unit>().GetDamage(_unit.m_stStat.fDamage_Base);
        }
    }
    #endregion
}
