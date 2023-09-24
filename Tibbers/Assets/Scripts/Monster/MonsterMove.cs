using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MonsterMove
{
    /* 다른 스크립트에 긁어서 선언하려구...
    
    // 기본 이속
    public float speed = 1.0f;

    // 돌진 관련
    public float dashSpeed = 10f;   // 돌진 속도
    public float dashInterval = 2f; // 돌진 간격
    public float dashDuration = 1f; // 돌진 지속 시간

    // 이동 타겟
    public Transform targetTransform;

    // this 오브젝트
    private Rigidbody2D thisRigidbody2D;
    private Transform thisTransform;

    // 타이머
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
    private Vector3 SetRandomDirection()
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
    private Vector3 SetJumpPosition(Transform targetTransform) {
        // 좌표 표식 이동 (플레이어를 따라간다)

        // 지정되면 멈춘다
        return targetTransform.position;
    }

    public IEnumerator JumpToTarget(Transform targetTransform, Transform thisTransform) {
        float timeCounter = 0f;


        float jumpHeight = 25.0f;
        // 그림자를 그린다 

        // 그림자를 땅바닥에 두고 만두를 하늘로 보낸다
        thisTransform.GetComponent<Rigidbody2D>().isKinematic = true;
        while (timeCounter < 1f)
        {
            thisTransform.position = new Vector3(thisTransform.position.x, thisTransform.position.y + (jumpHeight * Time.deltaTime), thisTransform.position.z);
            timeCounter += Time.deltaTime;
            yield return null;
        }
        timeCounter = 0f;

        Vector3 jumpTargetPosition = SetJumpPosition(targetTransform);
        ActiveTargetMarker(true, jumpTargetPosition);

        Vector3 lerpStartPosition = thisTransform.position;
        Vector3 lerpEndPosition1 = new Vector3(jumpTargetPosition.x, thisTransform.position.y, jumpTargetPosition.z);
        Vector3 lerpEndPosition2 = new Vector3(jumpTargetPosition.x, jumpTargetPosition.y, jumpTargetPosition.z);

        while (timeCounter < 1f)
        {
            thisTransform.position = Vector3.Lerp(lerpStartPosition, lerpEndPosition1, timeCounter);
            timeCounter += Time.deltaTime;
            yield return null;
        }
        timeCounter = 0f;

        // 만두를 떨군다
        thisTransform.GetComponent<Rigidbody2D>().isKinematic = false;

        while (timeCounter < 1f)
        {
            thisTransform.position = Vector3.Lerp(lerpEndPosition1, lerpEndPosition2, timeCounter);
            timeCounter += Time.deltaTime;
            yield return null;
        }
        ActiveTargetMarker(false);
        timeCounter = 0f;
        // 그림자를 서서히 이동했다가 타겟 좌표에 도착하면

        yield break;
    }
    /* END Jump */
    #endregion
}