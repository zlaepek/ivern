using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Structs;

public class Unit : MonoBehaviour
{
    // 체력 주고싶은 유닛은 m_stStat.fHp_Base 설정해주기

    // Test
    public float Mass = 1.0f;

    #region 변수
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

    private bool m_isBlinking = false;

    private Vector2 m_vForcePoint;

    private SpriteRenderer m_SpriteRenderer;

    #endregion  변수
    void Awake()
    {
        //m_stStat = default;
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
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

    public void Death()
    {
        // 사망 애니메이션 + 죽는 처리

        //Destroy(gameObject);
        gameObject.SetActive(false);
    }

    public void GetDamage(float _Damage, float _fKnockbackForce = default , Vector2 _vForcePoint = default)
    {
        if(m_isBlinking)
        {
            return;
        }
        m_stStat.fHp_Cur -= _Damage;

        if (m_stStat.fHp_Cur <= 0 )
        {
            if(gameObject.tag == "tag_Player")
            {
                return;
            }
            else if (gameObject.tag == "tag_Enemy")
            {
                // 나중에 죽는 애니메이션 불러오고 끝나면 죽게 설정
                ItemManager.Instance.DropItem( ItemManager.Instance.EXP_Ball, gameObject.transform.position, (int)ItemManager.eItemType.EXP_ball);
                DataManager.Instance.Add_Kill_Score(1);
                Death();
            }
        }

        if (gameObject.tag == "tag_Player")
        {
            m_isBlinking = true;
            StartCoroutine(BlinkEffect());
        }
        

        SetKnockback(_fKnockbackForce, _vForcePoint);
        Knockback();
        //Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        //Debug.Log("\nfForce : " + _fKnockbackForce + " ,vPoint : " + _vForcePoint + ", Dmage : "  + _Damage);
        //Debug.Log("\nCurHp : " + m_stStat.fHp_Cur);
    }

    public void ResetHp()
    {
        m_stStat.fHp_Max = m_stStat.fHp_Base;
        m_stStat.fHp_Cur = m_stStat.fHp_Max;
    }

    IEnumerator BlinkEffect()
    {
        float fTime = Time.time + 2.0f;
        while (true)
        {
            m_SpriteRenderer.enabled = !m_SpriteRenderer.enabled; // 스프라이트 깜빡임

            yield return new WaitForSeconds(0.1f); // 0.1초간 대기

            // 2초가 지나면 깜빡임 중지
            if (fTime - Time.time <= 0f)
            {
                m_SpriteRenderer.enabled = true; // 스프라이트 활성화
                m_isBlinking = false;
                yield break;
            }
        }
    }
}
