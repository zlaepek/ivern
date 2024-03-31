using UnityEngine;
using Boss;

public enum EffectAreaType
{
    None,
    Fire,
    Ice
}

public class EffectArea : MonoBehaviour
{
    public EffectAreaType thisEffectAreaType = EffectAreaType.None;

    private float fireDotDamage = 0.1f;
    private float fireEnterDamage = 5f;

    #region  Trigger
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("tag_Enemy_Invincible")) {
            switch (thisEffectAreaType) {
                case EffectAreaType.Fire:
                    FireAreaEnterMeltMandoo(collision);
                    break;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {

        if (collision.CompareTag("tag_Enemy_Invincible")) {
            switch (thisEffectAreaType) {
                case EffectAreaType.Fire:
                    FireAreaStayMeltMandoo(collision);
                    break;
            }
        } else if (collision.CompareTag("tag_Player")) {
            switch (thisEffectAreaType) {
                case EffectAreaType.Fire:
                    FireAreaStayAttackPlayer(collision);
                    break;
            }
        }
    }
    #endregion

    #region damage methods
    public void FireAreaStayMeltMandoo(Collider2D collision) {
        MandooTheBoss mandooTheBossTemp = collision.GetComponent<MandooTheBoss>();
        if (mandooTheBossTemp != null)
        {
            mandooTheBossTemp.GetMelt(fireDotDamage);
        }
    }
    public void FireAreaEnterMeltMandoo(Collider2D collision) {
        MandooTheBoss mandooTheBossTemp = collision.GetComponent<MandooTheBoss>();
        if (mandooTheBossTemp != null)
        {
            mandooTheBossTemp.GetMelt(fireEnterDamage);
        }
    }

    public void FireAreaStayAttackPlayer(Collider2D collision) {
        collision.gameObject.GetComponent<Unit>().GetDamage(fireDotDamage);
    }

    public void IceAreaSlowPlayer(Collider2D collision) {

    }
    #endregion
}
