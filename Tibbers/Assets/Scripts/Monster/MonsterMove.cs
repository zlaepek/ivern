using System.Collections;
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

    private float timeScale = 500f;
    public GameObject targetPositionMarker = null;

    private float pi = Mathf.PI;
    public void Initialize(GameObject targetPositionMarker)
    {
        this.targetPositionMarker = targetPositionMarker;
    }

    private void ActiveTargetMarker(bool active, Vector3 position = default)
    {
        targetPositionMarker.SetActive(active);
        if (active)
        {
            targetPositionMarker.transform.position = position;
        }
    }

    #region Move Methods
    /* Follow */
    public void FollowTarget(float speed, Transform thisTransform, Transform targetTransform)
    {
        Vector3 targetDirection = targetTransform.position - thisTransform.position;
        if (targetDirection.magnitude > 1) {
            targetDirection.Normalize();
        }

        thisTransform.GetComponent<Rigidbody2D>().velocity = (targetDirection * speed * Time.deltaTime * timeScale);
    }
    /* END Follow */

    /* Random Move */
    public Vector3 SetRandomDirection()
    {
        Vector3 targetDirection = Random.insideUnitCircle.normalized;
        targetDirection.Normalize();

        return targetDirection;
    }

    public IEnumerator RandomMove(Transform thisTransform, float speed, float randomMoveDuration)
    {
        Vector3 targetDirection = SetRandomDirection(); 

        thisTransform.GetComponent<Rigidbody2D>().velocity = (targetDirection * speed * Time.deltaTime * timeScale);

        yield break;
    }
    /* END Random Move */

    /* Dash */
    public Vector3 SetDashPosition(Transform thisTransform, Transform targetTransform) 
    {
        Vector3 targetDirection = targetTransform.position - thisTransform.position;
        targetDirection = targetDirection.normalized;

        return targetDirection;
    }

    public IEnumerator DashToTarget(Transform thisTransform, Vector3 targetDirection, float dashSpeed, float dashDuration) 
    {
        thisTransform.GetComponent<Rigidbody2D>().velocity = (targetDirection * dashSpeed * Time.deltaTime * timeScale);

        yield return new WaitForSeconds(dashDuration);
        thisTransform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

        yield break;
    }
    /* END Dash */

    /* Jump */
    public IEnumerator JumpToTarget(CircleCollider2D collider ,Transform targetTransform, Transform thisTransform, float jumpSpeed, float jumpDuration) {

        // �ݶ��̴� ����
        collider.enabled = false;

        Vector3 jumpDirection = targetTransform.position - thisTransform.position;
        Vector3 velocity = jumpDirection * jumpSpeed * Time.deltaTime * timeScale;
        thisTransform.GetComponent<Rigidbody2D>().velocity = new Vector3 (velocity.x, velocity.y + Mathf.Cos(pi * jumpDuration), velocity.z);
        thisTransform.GetComponent<Rigidbody2D>().AddForce(Vector3.up * 5f);
        yield return new WaitForSeconds(jumpDuration);

        // �ݶ��̴� Ű��
        collider.enabled = true;
        yield break;
    }
    /* END Jump */
    #endregion
}