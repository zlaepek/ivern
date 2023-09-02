using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove
{
    /* �ٸ� ��ũ��Ʈ�� �ܾ �����Ϸ���...
    
    // �⺻ �̼�
    public float speed = 1.0f;

    // ���� ����
    public float dashSpeed = 10f;   // ���� �ӵ�
    public float dashInterval = 2f; // ���� ����
    public float dashDuration = 1f; // ���� ���� �ð�

    // �̵� Ÿ��
    public Transform targetTransfrom;

    // this ������Ʈ
    private Rigidbody2D thisRigidbody2D;
    private Transform thisTransform;

    // Ÿ�̸�
    private float lastDashTime;

    private Vector3 targetDirection;

    public enum MosterMoveState
    {
        Follow,
        Slide
    }

    private MosterMoveState currentMoveState = MosterMoveState.Follow;
    */

    float TimeScale = 500f;

    #region Move Methods
    public void FollowTarget(float speed, Transform thisTransform, Transform targetTransfrom, Rigidbody2D thisRigidbody2D)
    {
        Vector3 targetDirection = targetTransfrom.position - thisTransform.position;
        if (targetDirection.magnitude > 1) {
            targetDirection.Normalize();
        }

        thisRigidbody2D.velocity = (targetDirection * speed * Time.deltaTime * TimeScale);
    }

    public void SetDashPosition(Vector3 targetDirection, Transform thisTransform, Transform targetTransfrom) {
        targetDirection = targetTransfrom.position - thisTransform.position;
        targetDirection.Normalize();
    }
    public void DashToTarget(Vector3 targetDirection, Rigidbody2D thisRigidbody2D, float dashSpeed) {

        thisRigidbody2D.velocity = (targetDirection * dashSpeed * Time.deltaTime * TimeScale);
    }

    public void SetRandomDirection(Vector3 targetDirection) {
        targetDirection = Random.insideUnitCircle.normalized;
        targetDirection.Normalize();
    }
    public void RandomMove(Vector3 targetDirection, Rigidbody2D thisRigidbody2D, float speed) {
        thisRigidbody2D.velocity = (targetDirection * speed * Time.deltaTime * TimeScale);
    }

    public void SetJumpPosition() {
        // ��ǥ ǥ�� �̵� (�÷��̾ ���󰣴�)
        // �����Ǹ� �����
    }
    public void JumpToTarget() {
        // �׸��ڸ� �׸��� 

        // �׸��ڸ� ���ٴڿ� �ΰ� ���θ� �ϴ÷� ������

        // �׸��ڸ� ������ �̵��ߴٰ� Ÿ�� ��ǥ�� �����ϸ�

        // ���θ� ������

    }
    #endregion
}
