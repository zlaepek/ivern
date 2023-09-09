using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Structs;

public class Unit : MonoBehaviour
{
    public Structs.UnitStat m_stStat;

    //float m_fBaseHp;
    //float m_fBaseMoveSpeed;
    //float m_fBaseMass;
    //float m_fBaseDamage;
    //float m_fBaseAttackSpeed;
    // Start is called before the first frame update

    private float m_fAcceleration;
    private float m_fDecelerationRate;

    private Vector2 m_vForcePoint;
    void Start()
    {
        m_stStat = default;

        m_fDecelerationRate = 0.9f;
        //m_stStatus = default;
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    void Knockback()
    {
        if(m_fAcceleration > 0.001f)
        {
            GetComponent<Transform>();

            m_fAcceleration *= m_fDecelerationRate;
        }
    }

    private void SetKnockback(float _fKnockbackForce, Vector2 _vForcePoint)
    {
        m_fAcceleration = _fKnockbackForce / m_stStat.fBaseMass;
        m_vForcePoint = _vForcePoint;
    }

    public void GetDamage(float _Damage, float _fKnockbackForce, Vector2 _vForcePoint)
    {
        SetKnockback(_fKnockbackForce, _vForcePoint);

        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        Debug.Log("\nfForce : " + _fKnockbackForce + " ,vPoint : " + _vForcePoint );
    }
}
