using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class MandooTheBoss : Unit
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

    private Vector3 targetDirection;
    #endregion MonsterMove ���� ���� �����

    #region ���� ���� �����
    private EffectArea effectArea;
    #endregion
    #region ������ ����Ŭ

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
    #endregion ������ ����Ŭ

    #region �� ������Ʈ ���� �Լ� (Init, Update���� ȣ�� �Լ�)
    /* Frozen */
    private void FrozenInit() {
        //TODO: �Ķ����� ���� ��ȯ
        effectArea.SpawnFireArea(targetTransform);
    }
    private void FrozenPattern() {
        // �����̵� & �ñ� ����
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
        //TODO: �� ����� ��
    }

    private void FrozenEnd()
    {
        //TODO: ������ �����
    }
    /* END Frozen */

    /* Mad */
    private void MadInit() {
        //TODO: �Ķ����� ����
    }
    private void MadPattern() {


        //TODO: ���� (���� Ÿ�̸Ӱ� �� ������ ��)
        if (currentTime > dashInterval) {
            Debug.Log(currentTime);
            currentMoveCoroutine = StartCoroutine(monsterMove.JumpToTarget(targetTransform, thisTransform));
            currentTime = 0;
        }
        //TODO: ���� ���� ���� (������ ���� ���� ��)
        else
        {
            monsterMove.RandomMove(thisTransform, speed, randomMoveDuration);
        }

        //TODO: ���� ���� ����
    }
    /* END Mad */

    /* Normal */
    private void NormalInit()
    {
        
    }
    private void NormalPattern() {
        // �̵�
        monsterMove.FollowTarget(speed, thisTransform, targetTransform);

        //TODO: źȯ ������

        //TODO: ü���� ������ ���°� �Ǹ�, ex) 50�� �̸� => �߾� ����
        //currentMandooState = MANDOO_STATE.MAD;
        //MadInit();
    }
    /* End Normal */

    private void Dead() {
        //TODO: ���� ���� ������Ʈ �˴� ����
        //TODO: ���� �״� ���
        //TODO: �������� �̵�
    }
    #endregion �� ������Ʈ ���� �Լ� (Init, Update���� ȣ�� �Լ�)
}
