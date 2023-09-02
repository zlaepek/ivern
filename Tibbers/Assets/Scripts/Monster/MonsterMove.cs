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
    public Transform targetTransfrom;

    // this 오브젝트
    private Rigidbody2D thisRigidbody2D;
    private Transform thisTransform;

    // 타이머
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
        // 좌표 표식 이동 (플레이어를 따라간다)
        // 지정되면 멈춘다
    }
    public void JumpToTarget() {
        // 그림자를 그린다 

        // 그림자를 땅바닥에 두고 만두를 하늘로 보낸다

        // 그림자를 서서히 이동했다가 타겟 좌표에 도착하면

        // 만두를 떨군다

    }
    #endregion
}
