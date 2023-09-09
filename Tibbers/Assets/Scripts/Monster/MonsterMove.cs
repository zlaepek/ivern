using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        // 좌표 표식 이동 (플레이어를 따라간다)

        // 지정되면 멈춘다
        return targetTransform.position;
    }

    public IEnumerator JumpToTarget(Transform targetTransform, Transform thisTransform) {
        float timeCounter = 0f;

        Vector3 jumpTargetPosition = SetJumpPosition(targetTransform);

        // 그림자를 그린다 

        // 그림자를 땅바닥에 두고 만두를 하늘로 보낸다
        thisTransform.GetComponent<Rigidbody2D>().isKinematic = true;
        while (timeCounter < 2f)
        {
            thisTransform.position = new Vector3(thisTransform.position.x, thisTransform.position.y + (10f * Time.deltaTime), thisTransform.position.z);
            timeCounter += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(2f);

        // 그림자를 서서히 이동했다가 타겟 좌표에 도착하면
        // while (x && z) 가 target과 같지 않다면
        // lerp 


        // 만두를 떨군다
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