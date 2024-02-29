using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Structs;

public class Weapon
{
    public GameObject m_BulletPrefab;

    private Structs.WeaponStat m_stStat;
    private int m_iBulletCount_Cur;
    //private int m_iWeaponSlot;
    private int m_iWeaponType;
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
        // 공격속도 = 기본공격속도 * ( 1 + 장비보너스 / 100 ) * ( 1 + 능력치보너스 / 100 )
        //float fTotalSpeed = m_stStat.fAttackSpeed * (1 + (_fBaseAttackSpeed / 100));
        float fTotalSpeed = m_stStat.fAttackSpeed * _fBaseAttackSpeed;
        float fAttackInterval = 1 / fTotalSpeed;

        // 코루틴으로 바꾸기

        GameObject tempBullet;

        if (m_iBulletCount_Cur >= m_stStat.iAttackCount)
        {
            // 일정 주기마다 발사하기
            if (Time.time > m_fNextAttack)
            {
                m_iBulletCount_Cur = 0;
            }
        }
        else
        {
            m_fNextAttack = Time.time + fAttackInterval;

            if (Time.time > m_fNextDelay)
            {
                m_fNextDelay = Time.time + m_stStat.fAttackDelay * fAttackInterval;
                tempBullet = GameObject.Instantiate(m_BulletPrefab, m_vBulletPos, m_BulletRotation);

                switch (m_eType)
                {
                    case BulletManager.eBulletType.BulletType_EnergyBall:
                        {
                            RotateBullet(_vAttackDir);
                            CreateAttackPos(_vShotterPos, _vAttackDir);

                            tempBullet.GetComponent<Bullet>().SetDir(_vAttackDir);
                            tempBullet.GetComponent<Bullet>().m_stStat = m_BulletPrefab.GetComponent<Bullet>().m_stStat;
                        }
                        break;

                    case BulletManager.eBulletType.BulletType_Melee:
                        {
                            RotateBullet(_vAttackDir);
                            CreateAttackPos(_vShotterPos, _vAttackDir);

                            tempBullet.GetComponent<Bullet>().SetDir(_vAttackDir);
                            tempBullet.GetComponent<Bullet>().m_stStat = m_BulletPrefab.GetComponent<Bullet>().m_stStat;
                        }
                        break;

                    case BulletManager.eBulletType.BulletType_HolyBomb:
                        {
                            CreateAttackPos_CameraRand();

                            tempBullet.GetComponent<Bullet>().m_stStat = m_BulletPrefab.GetComponent<Bullet>().m_stStat;
                            //Debug.Log(m_iBulletCount_Cur);
                        }
                        break;

                    case BulletManager.eBulletType.BulletType_RuneTracer:
                        {
                            //RotateBullet(_vAttackDir);
                            CreateAttackPos(_vShotterPos, _vAttackDir);

                            tempBullet.GetComponent<Bullet>().SetDir(_vAttackDir);
                            tempBullet.GetComponent<Bullet>().m_stStat = m_BulletPrefab.GetComponent<Bullet>().m_stStat;
                        }
                        break;
                    default:
                        break;
                }

                tempBullet.GetComponent<Bullet>().SetType(m_eType);
                tempBullet.GetComponent<Bullet>().SetMaster(m_Master);

                ++m_iBulletCount_Cur;
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
    private void CreateAttackPos_CameraRand()
    {
        // 
        float fCameraLeft = SpawnManager.Instance.GetCameraEdgePos(SpawnManager.eCameraEdgePos.Left);
        float fCameraRight = SpawnManager.Instance.GetCameraEdgePos(SpawnManager.eCameraEdgePos.Right);
        float fCameraBottom = SpawnManager.Instance.GetCameraEdgePos(SpawnManager.eCameraEdgePos.Bottom);
        float fCameraTop = SpawnManager.Instance.GetCameraEdgePos(SpawnManager.eCameraEdgePos.Top);

        m_vBulletPos.x = Random.Range(fCameraLeft, fCameraRight);
        m_vBulletPos.y = Random.Range(fCameraBottom, fCameraTop);
    }

    #region Setter
    //public int SetWeaponSlot { set { m_iWeaponSlot = value; } }
    public int SetAttackCount { set { m_stStat.iAttackCount = value; } }
    public float SetAttackSpeed { set { m_stStat.fAttackSpeed = value; } }
    public float SetAttackRange { set { m_stStat.fAttackRange = value; } }
    public float SetAttackDelay { set { m_stStat.fAttackDelay = value; } }
    public float SetBulletSpeed { set { m_BulletPrefab.GetComponent<Bullet>().m_fMoveSpeed = value; } }
    public float SetBulletDamage { set { m_BulletPrefab.GetComponent<Bullet>().m_fBulletDamage = value; } }
    public float SetBulletKnockback { set { m_BulletPrefab.GetComponent<Bullet>().m_fKnockbackForce = value; } }
    public float SetBulletLifeTime { set { m_BulletPrefab.GetComponent<Bullet>().m_fLifeTime = value; } }
    public uint SetBulletPierce { set { m_BulletPrefab.GetComponent<Bullet>().m_nPierce = value; } }

    public int WeaponType { set { m_iWeaponType = value; } get { return m_iWeaponType; } }

    public void SetMaster(GameObject _Master) { m_Master = _Master; }
    public void SetBullet(GameObject _BulletPrefab, BulletManager.eBulletType _eType) 
    { 
        m_BulletPrefab = _BulletPrefab;
        m_eType = _eType;
        m_BulletPrefab.GetComponent<Bullet>().SetType(_eType);
    }
    
    #endregion
}
