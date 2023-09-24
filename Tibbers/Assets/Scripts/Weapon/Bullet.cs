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
    private float LifeTime;

    private float fTickDamage;

    public float m_fKnockbackForce { get { return m_stStat.fKnockbackForce;  } set { m_stStat.fKnockbackForce = value; } }
    public float m_fMoveSpeed { get { return m_stStat.fMoveSpeed; } set { m_stStat.fMoveSpeed = value; } }
    public float m_fBulletDamage { get { return m_stStat.fBulletDamage; } set { m_stStat.fBulletDamage = value; } }
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

        m_Animator.SetTrigger("Create");

        m_stStat = default;
        m_fKnockbackForce = 10.0f;
        m_fMoveSpeed = 5.0f;
        m_fBulletDamage = 5.0f;

        LifeTime = 3.0f;

        fTickDamage = 0.0f;

        Destroy(gameObject, LifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        MoveBullet();
    }

#endregion

#region Collision
    private void OnTriggerEnter2D(Collider2D _Collision)
    {
        //if(  "ÀåÆÇ")
        {
            if(Time.time > fTickDamage)
            {
                //fTickDamage = 
            }
        }

        _Collision.GetComponent<Unit>().GetDamage(m_stStat.fBulletDamage, m_stStat.fKnockbackForce, m_vDir);

        Destroy(gameObject);
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

    //public float fBulletDamage;
    //public float fKnockbackForce;
    //public float fMoveSpeed;
   

#endregion
}
