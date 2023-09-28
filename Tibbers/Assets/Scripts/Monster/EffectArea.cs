using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EffectAreaType
{
    None,
    Fire,
    Ice
}

public class EffectArea : MonoBehaviour
{
    public EffectAreaType thisEffectAreaType = EffectAreaType.None;

    private float playerDamageTime;
    private float enemyDamageTime;

    private void Start() {
        
    }
    //#region Collision & Trigger
    //private void OnCollisionEnter2D(Collision2D collision) {

    //    if (collision.collider.CompareTag("tag_Enemy")) {
    //        switch (thisEffectAreaType) {
    //            case EffectAreaType.Fire:
    //                FireAreaEnterMeltMandoo(collision);
    //                enemyDamageTime = 0;
    //                break;
    //        }
    //    }
    //}

    //private void OnCollisionStay2D(Collision2D collision) {

    //    if (collision.collider.CompareTag("tag_Enemy")) {
    //        switch (thisEffectAreaType) {
    //            case EffectAreaType.Fire:
    //                FireAreaStayMeltMandoo(collision);
    //                break;
    //        }
    //    } else if (collision.collider.CompareTag("tag_Player")) {
    //        switch (thisEffectAreaType) {
    //            case EffectAreaType.Fire:
    //                FireAreaStayAttackPlayer(collision);
    //                break;
    //        }
    //    }
    //}
    //#endregion

    #region  Trigger
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("tag_Enemy")) {
            switch (thisEffectAreaType) {
                case EffectAreaType.Fire:
                    FireAreaEnterMeltMandoo(collision);
                    enemyDamageTime = 0;
                    break;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {

        if (collision.CompareTag("tag_Enemy")) {
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
        //TODO: ���ΰ� �ε����� �� (���� �������� ���)
        // ���� �� �� ��� 
        collision.GetComponent<MandooTheBoss>().GetMelt(0.1f);
    }
    public void FireAreaEnterMeltMandoo(Collider2D collision) {

        collision.GetComponent<MandooTheBoss>().GetMelt(5f);
    }

    public void FireAreaStayAttackPlayer(Collider2D collision) {
        // TODO: �÷��̾ �ε����� �� (��Ʈ���� �ִ´�)
        // collision.GetComponent<Unit>().GetDamage(0.01f);
    }

    public void IceAreaSlowPlayer(Collider2D collision) {

    }
    #endregion
}
