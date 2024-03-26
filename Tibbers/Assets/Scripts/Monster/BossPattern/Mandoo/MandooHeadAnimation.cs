using UnityEngine;

public class MandooHeadAnimation : MonoBehaviour
{
    #region ����
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
}
