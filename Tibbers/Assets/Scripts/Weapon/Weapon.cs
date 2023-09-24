using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Structs;

public class Weapon
{
    public GameObject m_BulletPrefab;

    private Structs.WeaponStat m_stStat;
    private float m_fNextAttack;
    private float m_fNextDelay;
    private Quaternion m_BulletRotation;
    private Vector3 m_vBulletPos;
    private GameObject m_Master;

    private BulletManager.eBulletType m_eType;
     //Start is called before the first frame update
    public Weapon()
    {
        m_fNextAttack = Time.time;

    }


    public void Attack(Vector3 _vShotterPos, Vector2 _vAttackDir, float _fBaseAttackSpeed)
    {
        
        // 일정 주기마다 발사하기
        if (Time.time > m_fNextAttack)
        {
            // 공격속도 = 기본공격속도 * ( 1 + 장비보너스 / 100 ) * ( 1 + 능력치보너스 / 100 )
            float fTotalSpeed = m_stStat.fAttackSpeed * (1 + (_fBaseAttackSpeed / 100));
            float fAttackInterval = 1 / fTotalSpeed;

            // 코루틴으로 바꾸기
            m_fNextAttack = Time.time + fAttackInterval;


            switch (m_eType)
            {
                case BulletManager.eBulletType.BulletType_EnergyBall:
                    {
                        RotateBullet(_vAttackDir);
                        CreateAttackPos(_vShotterPos, _vAttackDir);

                        for (int i = 0; i < m_stStat.iAttackCount; ++i)
                        {
                            if (Time.time > m_fNextDelay)
                            {
                                m_fNextDelay = Time.time + m_stStat.fAttackDelay;

                                GameObject tempBullet = GameObject.Instantiate(m_BulletPrefab, m_vBulletPos, m_BulletRotation);
                                tempBullet.GetComponent<Bullet>().SetDir(_vAttackDir);
                            }
                        }
                    }
                    break;

                case BulletManager.eBulletType.BulletType_Melee:
                    {

                    }
                    break;
                default:
                    break;
            }
        }
    }

    private void RotateBullet(Vector2 _vAttackDir)
    {
        float fAngle = Mathf.Atan2(_vAttackDir.y, _vAttackDir.x) * Mathf.Rad2Deg;
        m_BulletRotation = Quaternion.AngleAxis(fAngle, Vector3.forward);
    }
    private void CreateAttackPos(Vector3 _vShotterPos, Vector2 _vAttackDir)
    {
        m_vBulletPos = _vShotterPos + (Vector3)(_vAttackDir * m_stStat.fAttackRange);
    }

    #region Setter
    public int SetAttackCount { set { m_stStat.iAttackCount = value; } }
    public float SetAttackSpeed { set { m_stStat.fAttackSpeed = value; } }
    public float SetAttackRange { set { m_stStat.fAttackRange = value; } }
    public float SetAttackDelay { set { m_stStat.fAttackDelay = value; } }
    public float SetBulletSpeed { set { m_BulletPrefab.GetComponent<Bullet>().m_fMoveSpeed = value; } }
    public float SetBulletDamage { set { m_BulletPrefab.GetComponent<Bullet>().m_fBulletDamage = value; } }
    public float SetBulletKnockback { set { m_BulletPrefab.GetComponent<Bullet>().m_fKnockbackForce = value; } }
    public void SetMaster(GameObject _Master) { m_Master = _Master; }
    public void SetBullet(GameObject _BulletPrefab, BulletManager.eBulletType _eType) 
    { 
        m_BulletPrefab = _BulletPrefab;
        m_eType = _eType;
    }
    #endregion
}
