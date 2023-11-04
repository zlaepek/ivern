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
    #endregion ���� �����

    #region ���� �Ӹ�
    [SerializeField] private GameObject madMandooHeadPrefab = null;
    private GameObject madMandooHead = null;

    private Coroutine madMandooHeadCoroutine = null;
    private int mandooBodyJumpCount = 0;
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

    #region ���� ���� �����
    [SerializeField] private MandooEffectAreaController effectAreaController;
    #endregion

    #region ������ ����Ŭ

    private void Start()
    {
        // player Init
        targetTransform = GameObject.FindGameObjectWithTag("tag_Player").transform;


        bossUI = GameObject.FindGameObjectWithTag("Canvas").GetComponentInChildren<BossUI>();
        // Unit Init
        unit = GetComponent<Unit>();

        unit.m_stStat.fDamage_Base = 2.0f;
        unit.m_stStat.fHp_Base = 100.0f;
        bossUI.bossHpSlider.maxValue = 100;

        unit.m_stStat.fMoveSpeed_Base = 1.0f;
        unit.m_stStat.fMoveSpeed_Base = 1.0f;

        unit.ResetHp();



        // Frozen Init
        frozenSlider = bossUI.bossFrozenSlider;
        frozenSlider.maxValue = maxFrozenValue;
        frozenSlider.minValue = 0f;
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
        bossUI.updateHPSlider(unit.m_stStat.fHp_Cur);
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

    #region Frozen
    private void FrozenInit()
    {
        // �Ķ����� ���� ��ȯ
        effectAreaController.SpawnFireArea(targetTransform, transform.parent);
    }
    private void FrozenPattern()
    {
        // �����̵� & �ñ� ����
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

        // �� ����� ��
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
        // ������ �����
        effectAreaController.DestroyFireArea();
    }

    public void GetMelt(float value) {
        currentFrozenValue -= value;
        frozenSlider.value = currentFrozenValue;
    }
    #endregion

    #region Mad
    private void MadInit()
    {
        // �Ӹ� ���� �и�
        madMandooHead = Instantiate(madMandooHeadPrefab, transform.parent);
    }
    private void MadPattern()
    {

        // (3���� ���� ��) ����
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
        // �Ӹ� ���� ���� ����
        else if (currentTime > dashInterval)
        {
            if (madMandooHeadCoroutine != null)
            {
                StopCoroutine(madMandooHeadCoroutine);
            }
            if (madMandooHead != null) {
                madMandooHeadCoroutine = StartCoroutine(monsterMove.RandomMove(madMandooHead.transform, unit.fCurMoveSpeed, randomMoveDuration));
                currentTime = 0;
                mandooBodyJumpCount++;
            }
            else {
                MadInit();
            }
        }


        //TODO: ���� ���� ����
        if (false) // hp�� 0�̸� ���
        {
            MadEnd();
            Dead();
        }
        if (false) // hp�� ���� ���� �� 
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
        // �̵�
        monsterMove.FollowTarget(unit.fCurMoveSpeed, transform, targetTransform);

        //TODO: źȯ ������

        //TODO: ü���� ������ ���°� �Ǹ�, ex) 50�� �̸� => �߾� ����
        //currentMandooState = MANDOO_STATE.MAD;
        //MadInit();

        if (false) // hp�� 0�̸� ���
        {
            NormalEnd();
            Dead();
        }
        if (false) // hp�� ���� ���� �� => ��������
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
        //TODO: ���� ���� ������Ʈ �˴� ����
        //TODO: ���� �״� ���
        //TODO: �������� �̵�
    }
    #endregion
}
