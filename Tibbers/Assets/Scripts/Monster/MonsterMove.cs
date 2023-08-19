using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    public float speed = 1.0f;
    public float slideSpeed = 25f;
    public Transform targetTransfrom;

    private enum MoveState
    {
        Follow,
        Slide
    }
    private MoveState currentMoveState = MoveState.Follow;
    private enum MosterType
    {
        Normal,
        FrozenMandoo,
        Mandoo,
    }
    [SerializeField]private MosterType mosterType = MosterType.Normal;


    private Vector3 targetDirection;

    #region LifeCycle
    private void Update()
    {
        if (mosterType == MosterType.FrozenMandoo)
        {
            StartCoroutine(SlideTimer());
        }

        switch (currentMoveState)
        {
            case MoveState.Follow:
                FollowTarget();
                break;
            case MoveState.Slide:
                SlideToTarget();
                break;
        }
    }
    #endregion

    #region Move Methods
    private void FollowTarget()
    {
        targetDirection = targetTransfrom.position - transform.position;
        targetDirection.Normalize();

        transform.position += targetDirection * speed * Time.deltaTime;
    }

    private void SlideToTarget()
    {
        transform.position += targetDirection * slideSpeed * Time.deltaTime;
    }

    private IEnumerator SlideTimer()
    {
        if (currentMoveState == MoveState.Slide)
        {
            targetDirection = targetTransfrom.position - transform.position;
            targetDirection.Normalize();

            yield return new WaitForSeconds(0.1f);
            currentMoveState = MoveState.Follow;
        }
        else
        {
            yield return new WaitForSeconds(2.0f);

            currentMoveState = MoveState.Slide;
        }
    }
    #endregion
}
