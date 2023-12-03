using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon_Mob : MonoBehaviour
{
    public int SerialNumber;
    private MonsterMove monsterMove;

    private GameObject playerCharacter;

    private Unit m_Unit;
    // Start is called before the first frame update
    void Start()
    {
        

        //Init(1);
        //int iLayerNum = LayerMask.NameToLayer("Monster_Mob");
        //Physics2D.IgnoreLayerCollision(iLayerNum, iLayerNum);
    }

    #region Collision
    //private void OnTriggerEnter2D(Collider2D _Collision)
    //{
    //    if (_Collision.tag == "tag_Player")
    //        _Collision.GetComponent<Unit>().GetDamage(m_Unit.m_stStat.fDamage_Base);
    //}
    //private void OnCollisionEnter2D(Collision2D collision)
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("tag_Player"))
        {
            collision.gameObject.GetComponent<Unit>().GetDamage(m_Unit.m_stStat.fDamage_Base);
        }
    }
    #endregion Collision

    public void Init(int _iType)
    {
        monsterMove = new MonsterMove();
        playerCharacter = GameObject.FindGameObjectWithTag("tag_Player");
        m_Unit = GetComponent<Unit>();

        m_Unit.m_stStat.fDamage_Base = 2.0f;
        m_Unit.m_stStat.fHp_Base = 10.0f;
        m_Unit.m_stStat.fMoveSpeed_Base = 1.0f * 0.1f;
        m_Unit.ResetHp();
    }


    // Update is called once per frame
    //void Update()
    //{
    //    if(!m_Unit.isKnockBack)
    //        monsterMove.FollowTarget(m_Unit.fCurMoveSpeed, transform, playerCharacter.transform);
    //}
    void FixedUpdate()
    {
        if (!m_Unit.isKnockBack)
            monsterMove.FollowTarget(m_Unit.fCurMoveSpeed, transform, playerCharacter.transform);
    }
}
