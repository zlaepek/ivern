using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Structs;

public class Bullet : MonoBehaviour
{
    public Structs.BulletStat m_stStat;
    //float m_fBulletDamage;
    //float m_fKnockbackForce;
    //float fMoveSpeed;

    private GameObject m_Master;
    private Rigidbody2D m_RigidBody;
    private Vector2 m_vDir;

    private Animator m_Animator;

    private float fTickDamage;
    
    private BulletManager.eBulletType m_eType;

    public float m_fKnockbackForce { get { return m_stStat.fKnockbackForce;  } set { m_stStat.fKnockbackForce = value; } }
    public float m_fMoveSpeed { get { return m_stStat.fMoveSpeed; } set { m_stStat.fMoveSpeed = value; } }
    public float m_fBulletDamage { get { return m_stStat.fBulletDamage; } set { m_stStat.fBulletDamage = value; } }
    public float m_fLifeTime { get { return m_stStat.fLifeTime; } set { m_stStat.fLifeTime = value; } }
    public uint m_nPierce { get { return m_stStat.nPierce; } set { m_stStat.nPierce = value; } }
    private void RotateBullet()
    {
        float fAngle = Mathf.Atan2(m_vDir.y, m_vDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(fAngle, Vector3.forward);
    }
    private void MoveBullet()
    {
        m_RigidBody.velocity = m_vDir * m_stStat.fMoveSpeed;

    }
    //private void FlipX()
    //{
    //    Vector3 vScale = transform.localScale;
    //    vScale.x *= -1.0f;
    //    transform.localScale = vScale;
    //}

    #region LifeCycle
    // Start is called before the first frame update
    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();

        switch(m_eType)
        {
            case BulletManager.eBulletType.BulletType_EnergyBall:
                {
                    m_Animator.SetTrigger("Create");
                }
                break;

            case BulletManager.eBulletType.BulletType_Melee:
                {

                }
                break;
            default:
                break;
        }

        //m_stStat = default;
        //m_fKnockbackForce = 10.0f;
        //m_fMoveSpeed = 5.0f;
        //m_fBulletDamage = 5.0f;

        //m_stStat.fLifeTime = 3.0f;

        fTickDamage = 0.0f;

        if(m_stStat.fLifeTime > 0.0f)
        {
            Destroy(gameObject, m_stStat.fLifeTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        MoveBullet();
        if(m_stStat.fLifeTime < 0.0f)
        {
            if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !m_Animator.IsInTransition(0))
            {
                Destroy(gameObject);
            }
        }
        
    }
#endregion

#region Collision
    private void OnTriggerEnter2D(Collider2D _Collision)
    {
        //if(  "����")
        {
            if(Time.time > fTickDamage)
            {
                //fTickDamage = 
            }
        }

        _Collision.GetComponent<Unit>().GetDamage(m_stStat.fBulletDamage, m_stStat.fKnockbackForce, m_vDir);

        if(m_stStat.nPierce == 0)
        {
            Destroy(gameObject);
        }
        else
        {
            --m_stStat.nPierce;
        }
    }
    #endregion

#region Setter
    public void SetDir(Vector2 _Vec)
    {
        m_vDir = _Vec;
    }

    public void SetMaster(GameObject _Master)
    {
        m_Master = _Master;
    }
    public void SetType (BulletManager.eBulletType _Type)
    {
        m_eType = _Type;
    }
    //public float fBulletDamage;
    //public float fKnockbackForce;
    //public float fMoveSpeed;
   

#endregion
}
