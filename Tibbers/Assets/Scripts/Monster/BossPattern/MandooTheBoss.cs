using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

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

    // �ڷ�ƾ
    private Coroutine currentMoveCoroutine = null;
    private Coroutine currentAttactCoroutine = null;


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
    public float speed = 1.0f;
    public float randomMoveDuration = 1.0f;

    // ���� ����
    public float dashSpeed = 10.0f;     // ���� �ӵ�
    public float dashInterval = 5.0f;   // ���� ����
    public float dashDuration = 0.2f;     // ���� ���� �ð�

    // �̵� Ÿ��
    public Transform targetTransform;

    // this ������Ʈ
    private Rigidbody2D thisRigidbody2D;
    private Transform thisTransform;

    // Ÿ�̸�
    private float currentTime = 0;

    public GameObject targetPositionMarker = null;
    #endregion MonsterMove ���� ���� �����

    #region ���� ���� �����
    [SerializeField] private EffectArea effectArea;
    #endregion
    #region ������ ����Ŭ

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
    #endregion ������ ����Ŭ

    #region Frozen
    private void FrozenInit()
    {
        // �Ķ����� ���� ��ȯ
        effectArea.SpawnFireArea(targetTransform, transform.parent);
    }
    private void FrozenPattern()
    {
        // �����̵� & �ñ� ����
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

        // �� ����� ��
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
        // ������ �����
        effectArea.DestroyFireArea();
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
            currentMoveCoroutine = StartCoroutine(monsterMove.JumpToTarget(targetTransform, thisTransform));
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
            madMandooHeadCoroutine = StartCoroutine(monsterMove.RandomMove(madMandooHead.transform, speed, randomMoveDuration));
            currentTime = 0;
            mandooBodyJumpCount++;
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
        monsterMove.FollowTarget(speed, thisTransform, targetTransform);

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
