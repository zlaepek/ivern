using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MandooTheBossHead : MonoBehaviour
{
    MandooTheBoss mandooOrigin;

    #region
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("tag_Player"))
        {
            collision.GetComponent<Unit>().GetDamage(mandooOrigin.Unit.m_stStat.fDamage_Base);
        }
    }
    #endregion
}
