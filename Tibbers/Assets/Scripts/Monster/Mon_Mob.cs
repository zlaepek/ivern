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
        monsterMove = new MonsterMove();
        playerCharacter = GameObject.FindGameObjectWithTag("tag_Player");
        m_Unit = GetComponent<Unit>();

        Init(1);
        //int iLayerNum = LayerMask.NameToLayer("Monster_Mob");
        //Physics2D.IgnoreLayerCollision(iLayerNum, iLayerNum);
    }

    public void Init(int _iType)
    {
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
