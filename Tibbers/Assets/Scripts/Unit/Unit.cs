using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Structs;

public class Unit : MonoBehaviour
{
    // ü�� �ְ���� ������ m_stStat.fHp_Base �������ֱ�

    // Test
    public float Mass = 1.0f;

    #region ����
    public Structs.UnitStat m_stStat;

    public float fCurMoveSpeed { get { return m_stStat.fMoveSpeed_Base * ((100 + m_stStat.fMoveSpeed_Buf - m_stStat.fMoveSpeed_DeBuf) / 100); } }

    public float fCurAttackSpeed { get { return m_stStat.fAttackSpeed_Base * ((100 + m_stStat.fAttackSpeed_Buf - m_stStat.fAttackSpeed_DeBuf) / 100); } }
    public bool isKnockBack { get { return m_fAcceleration > 0.001f; } }
    //float m_fBaseHp;
    //float m_fBaseMoveSpeed;
    //float m_fBaseMass;
    //float m_fBaseDamage;
    //float m_fBaseAttackSpeed;
    // Start is called before the first frame update

    private float m_fAcceleration;
    private float m_fDecelerationRate;

    private Vector2 m_vForcePoint;
    #endregion  ����
    void Awake()
    {
        //m_stStat = default;

        m_fDecelerationRate = 0.9f;
        //m_stStatus = default;
    }

    // Update is called once per frame
    void Update()
    {
        m_stStat.fMass_Base = Mass;

        if (m_fAcceleration > 0.001f)
        {
            transform.GetComponent<Rigidbody2D>().velocity = (m_vForcePoint * m_fAcceleration);
            m_fAcceleration *= m_fDecelerationRate;
        }
        else
        {
            m_fAcceleration = 0.0f;
        }
    }

    void Knockback()
    {
        if(m_fAcceleration > 0.001f)
        {
            transform.GetComponent<Rigidbody2D>().velocity = (m_vForcePoint * m_fAcceleration);
            m_fAcceleration *= m_fDecelerationRate;
        }
        else
        {
            m_fAcceleration = 0.0f;
        }
    }

    private void SetKnockback(float _fKnockbackForce, Vector2 _vForcePoint)
    {
        m_fAcceleration = _fKnockbackForce / m_stStat.fMass_Base;
        m_vForcePoint = _vForcePoint;
    }

    private void Death()
    {
        // ��� �ִϸ��̼� + �״� ó��

        //Destroy(gameObject);
        gameObject.SetActive(false);
    }

    public void GetDamage(float _Damage, float _fKnockbackForce = default , Vector2 _vForcePoint = default)
    {
        if(m_stStat.fHp_Cur <= 0 )
        {
            if(gameObject.tag == "tag_Player")
            {
                return;
            }
            else
            {
                // ���߿� �״� �ִϸ��̼� �ҷ����� ������ �װ� ����
                Death();
            }
        }
        m_stStat.fHp_Cur -= _Damage;
        

        SetKnockback(_fKnockbackForce, _vForcePoint);
        Knockback();
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        Debug.Log("\nfForce : " + _fKnockbackForce + " ,vPoint : " + _vForcePoint + ", Dmage : "  + _Damage);
    }

    public void ResetHp()
    {
        m_stStat.fHp_Max = m_stStat.fHp_Base;
        m_stStat.fHp_Cur = m_stStat.fHp_Max;
    }
}
