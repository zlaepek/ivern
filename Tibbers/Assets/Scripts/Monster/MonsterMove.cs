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
    public Transform targetTransform;

    // this ������Ʈ
    private Rigidbody2D thisRigidbody2D;
    private Transform thisTransform;

    // Ÿ�̸�
    private float lastDashTime;

    private Vector3 targetDirection;

    public enum MonsterMoveState
    {
        Follow,
        Slide
    }

    private MonsterMoveState currentMoveState = MonsterMoveState.Follow;
    */

    float TimeScale = 500f;

    #region Move Methods
    /* Follow */
    public void FollowTarget(float speed, Transform thisTransform, Transform targetTransform)
    {
        Vector3 targetDirection = targetTransform.position - thisTransform.position;
        if (targetDirection.magnitude > 1) {
            targetDirection.Normalize();
        }

        thisTransform.GetComponent<Rigidbody2D>().velocity = (targetDirection * speed * Time.deltaTime * TimeScale);
    }
    /* END Follow */

    /* Random Move */
    private Vector3 SetRandomDirection()
    {
        Vector3 targetDirection = Random.insideUnitCircle.normalized;
        targetDirection.Normalize();

        return targetDirection;
    }

    public IEnumerator RandomMove(Transform thisTransform, float speed, float randomMoveDuration)
    {
        Vector3 targetDirection = SetRandomDirection(); 

        thisTransform.GetComponent<Rigidbody2D>().velocity = (targetDirection * speed * Time.deltaTime * TimeScale);

        yield break;
    }
    /* END Random Move */

    /* Dash */
    private Vector3 SetDashPosition(Transform thisTransform, Transform targetTransform) 
    {
        Vector3 targetDirection = targetTransform.position - thisTransform.position;
        targetDirection.Normalize();

        return targetDirection;
    }

    public IEnumerator DashToTarget(Transform thisTransform, Transform targetTransform, float dashSpeed, float dashDuration) 
    {
        Vector3 targetDirection = SetDashPosition(thisTransform, targetTransform);

        thisTransform.GetComponent<Rigidbody2D>().velocity = (targetDirection * dashSpeed * Time.deltaTime * TimeScale);

        yield return new WaitForSeconds(dashDuration);
        thisTransform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

        yield break;
    }
    /* END Dash */

    /* Jump */
    private Vector3 SetJumpPosition(Transform targetTransform) {
        // ��ǥ ǥ�� �̵� (�÷��̾ ���󰣴�)

        // �����Ǹ� �����
        return targetTransform.position;
    }

    public IEnumerator JumpToTarget(Transform targetTransform, Transform thisTransform) {
        float timeCounter = 0f;

        Vector3 jumpTargetPosition = SetJumpPosition(targetTransform);

        // �׸��ڸ� �׸��� 

        // �׸��ڸ� ���ٴڿ� �ΰ� ���θ� �ϴ÷� ������
        thisTransform.GetComponent<Rigidbody2D>().isKinematic = true;
        while (timeCounter < 2f)
        {
            thisTransform.position = new Vector3(thisTransform.position.x, thisTransform.position.y + (10f * Time.deltaTime), thisTransform.position.z);
            timeCounter += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(2f);

        // �׸��ڸ� ������ �̵��ߴٰ� Ÿ�� ��ǥ�� �����ϸ�
        // while (x && z) �� target�� ���� �ʴٸ�
        // lerp 


        // ���θ� ������
        thisTransform.GetComponent<Rigidbody2D>().isKinematic = false;
        timeCounter = 0f;
        while (timeCounter < 2f)
        {
            thisTransform.position = new Vector3(thisTransform.position.x, thisTransform.position.y - (10f * Time.deltaTime), thisTransform.position.z);
            timeCounter += Time.deltaTime;
            yield return null;
        }

        yield break;
    }
    /* END Jump */
    #endregion
}