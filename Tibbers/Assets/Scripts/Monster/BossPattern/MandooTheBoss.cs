using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    #endregion ���� �����

    #region MonsterMove ���� ���� �����
    // Monster Move ����
    private MonsterMove monsterMove;
    // �⺻ �̼�
    public float speed = 1.0f;

    // ���� ����
    public float dashSpeed = 10.0f;   // ���� �ӵ�
    public float dashInterval = 5.0f; // ���� ����
    public float dashDuration = 1f; // ���� ���� �ð�

    // �̵� Ÿ��
    public Transform targetTransfrom;

    // this ������Ʈ
    private Rigidbody2D thisRigidbody2D;
    private Transform thisTransform;

    // Ÿ�̸�
    private float dashTimer;
    private float currentTime = 0;

    private Vector3 targetDirection;
    #endregion MonsterMove ���� ���� �����

    #region ������ ����Ŭ

    private void Start() {
        monsterMove = new MonsterMove();

        thisRigidbody2D = GetComponentInChildren<Rigidbody2D>();
        thisTransform = GetComponentInChildren<Transform>();

        FrozenInit();
    }
    private void Update() {
        currentTime += Time.deltaTime;
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
    private void FrozenInit() {
        //TODO: �Ķ����� ���� ��ȯ
    }
    private void FrozenPattern() {
        //TODO: �����̵� & �ñ� ����
    }

    private void MadInit() {
        //TODO: �Ķ����� ����
    }
    private void MadPattern() {
        //TODO: ���� ���� ���� (������ ���� ���� ��)

        //TODO: ���� (���� Ÿ�̸Ӱ� �� ������ ��)
        if (currentTime > dashInterval) {
            Debug.Log(currentTime);
            monsterMove.SetDashPosition(targetDirection, thisTransform, targetTransfrom);

            while (dashDuration > dashTimer) {
                monsterMove.DashToTarget(targetDirection, thisRigidbody2D, dashSpeed);
                dashTimer += Time.deltaTime;
            }

            currentTime = 0;
            dashTimer = 0;
        }




        //TODO: ���� ���� ����
    }

    private void NormalPattern() {
        // �̵�
        monsterMove.FollowTarget(speed, thisTransform, targetTransfrom, thisRigidbody2D);

        //TODO: źȯ ������

        //TODO: ü���� ������ ���°� �Ǹ�, ex) 50�� �̸� => �߾� ����
        //currentMandooState = MANDOO_STATE.MAD;
        //MadInit();
    }

    private void Dead() {
        //TODO: ���� ���� ������Ʈ �˴� ����
        //TODO: �������� �̵�
    }
    #endregion �� ������Ʈ ���� �Լ� (Init, Update���� ȣ�� �Լ�)
}
