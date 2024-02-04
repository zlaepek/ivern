using UnityEngine;

public class MandooAnimation : MonoBehaviour
{
    #region ����
    // Animation
    public Animator mandooAnimator = null;
    public Animator mandooHeadAnimator = null;
    #endregion

    #region Head
    public void StartHeadJump()
    {
        mandooHeadAnimator.SetBool("isJumping", true);
    }

    public void EndHeadJump()
    {
        mandooHeadAnimator.SetBool("isJumping", false);
    }
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

    #region 
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








    public void StartMad(GameObject madMandooHead)
    {
        // ���� �ִϸ��̼� ����
        mandooAnimator.SetBool("isNoHead", true);

        // ���� �Ӹ� �ִϸ��̼�
        if (mandooHeadAnimator == null)
        {
            mandooHeadAnimator = madMandooHead.GetComponent<Animator>();
        }
    }



}
