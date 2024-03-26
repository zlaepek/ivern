using UnityEngine;
using System.Collections;
using System;

public class MandooAnimation : MonoBehaviour
{
    #region 선언
    // Animation
    public Animator mandooAnimator = null;
    #endregion



    #region Body
    public void StartBodyJump()
    {
        mandooAnimator.SetBool("isNoHeadJumping", true);
    }

    public void EndBodyJump()
    {
        mandooAnimator.SetBool("isNoHeadJumping", false);
    }

    public void StartBodyThrow()
    {
        mandooAnimator.SetBool("isNoHeadThrowing", true);
    }

    public void EndBodyThrow()
    {
        mandooAnimator.SetBool("isNoHeadThrowing", false);
    }
    #endregion

    #region Mandoo Animation
    public void FrozenAnimation(bool isFrozen)
    {
        mandooAnimator.SetBool("isFrozen", isFrozen);
    }

    public void StartJump()
    {
        mandooAnimator.SetBool("isJumping", true);
    }

    public void EndJump()
    {
        mandooAnimator.SetBool("isJumping", false);
    }

    public void StartThrow()
    {
        mandooAnimator.SetBool("isThrowing", true);
    }

    public void EndThrow()
    {
        mandooAnimator.SetBool("isThrowing", false);
    }
    #endregion


    public IEnumerator StartMad(float delay)
    {
        yield return new WaitForSeconds(delay);

        // 몸통 애니메이션 시작
        mandooAnimator.SetBool("isNoHead", true);
    }
}
