using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Test
    public float KnockBackForce = 1.0f;
    public float AttackRange = 1.0f;

    #region Private_var
    public enum eState
    {
        Idle = 0,
        Move,
        Hit,

        ePlayerState_Max
    }

    public enum eState_Dir
    {
        Right = 0,
        Left,
        //Up,
        //Down,

        ePlayerState_Dir_Max
    }

    //private ePlayerState m_eState;
    private eState_Dir m_eState_Dir;
    //private float m_fMoveSpeed;
    private float m_fHorizontal;
    private float m_fVertical;
    private Vector2 m_vControlDir;
    private Vector2 m_vTempNormal;
    private Vector2 m_vMovement;
    private Vector2 m_vLastDir;

    private Animator m_Animator;
    private SpriteRenderer m_SpriteRenderer;
    private Rigidbody2D m_RigidBody;

    private List<Weapon> m_WeaponSlot;
    private Unit m_UnitStat;

    //public ePlayerState State{ get { return m_eState; } }
    public eState_Dir State_Dir { get { return m_eState_Dir; } }
    #endregion // Private_var

    #region Logic_Basic
    void Start()
    {
        m_WeaponSlot = new List<Weapon>();

        Reset();
        SetWeapon(BulletManager.eBulletType.BulletType_EnergyBall);
        //SetWeapon(BulletManager.eBulletType.BulletType_Melee);
        //SetWeapon(BulletManager.eBulletType.BulletType_HolyBomb);
        //SetWeapon(BulletManager.eBulletType.BulletType_RuneTracer);

    }
    private void SetWeapon(BulletManager.eBulletType _BulletType)
    {
        Weapon TempWeapon = new Weapon();
        BulletManager Bulletmgr = GameObject.FindObjectOfType<BulletManager>();
        TempWeapon.WeaponType = (int)_BulletType;

        switch (_BulletType)
        {
            case BulletManager.eBulletType.BulletType_EnergyBall:
                {
                    TempWeapon.SetAttackDelay = 1.0f;
                    TempWeapon.SetAttackCount = 1;
                    TempWeapon.SetAttackRange = 0.0f;
                    //TempWeapon.SetAttackSpeed = 1.0f / (1.0f); // sec
                    TempWeapon.SetAttackSpeed = 1.0f / (0.5f); // sec

                    TempWeapon.SetBullet(Bulletmgr.EnergyBall, BulletManager.eBulletType.BulletType_EnergyBall);

                    //TempWeapon.SetBulletDamage = 5.0f;
                    TempWeapon.SetBulletDamage = 10.0f;
                    TempWeapon.SetBulletKnockback = 5.0f;
                    TempWeapon.SetBulletSpeed = 5.0f;
                    TempWeapon.SetBulletLifeTime = 3.0f;
                    TempWeapon.SetBulletPierce = 0;
                }
                break;

            case BulletManager.eBulletType.BulletType_Melee:
                {
                    TempWeapon.SetAttackDelay = 2.0f;
                    TempWeapon.SetAttackCount = 1;
                    TempWeapon.SetAttackRange = 1.0f;
                    TempWeapon.SetAttackSpeed = 1.0f / (2.0f); // sec


                    TempWeapon.SetBullet(Bulletmgr.Melee, BulletManager.eBulletType.BulletType_Melee);

                    TempWeapon.SetBulletDamage = 5.0f;
                    TempWeapon.SetBulletKnockback = 40.0f;
                    TempWeapon.SetBulletSpeed = 7.0f;
                    TempWeapon.SetBulletLifeTime = -1.0f;
                    TempWeapon.SetBulletPierce = 10000;
                }
                break;
            case BulletManager.eBulletType.BulletType_HolyBomb:
                {
                    TempWeapon.SetAttackDelay = 1.0f;
                    TempWeapon.SetAttackCount = 5;
                    TempWeapon.SetAttackRange = 1.0f;
                    //TempWeapon.SetAttackSpeed = 1.0f / 3.0f;
                    TempWeapon.SetAttackSpeed = 1.0f / (5.0f); // sec

                    
                    TempWeapon.SetBullet(Bulletmgr.HolyBomb, BulletManager.eBulletType.BulletType_HolyBomb);

                    TempWeapon.SetBulletDamage = 1.0f;
                    TempWeapon.SetBulletKnockback = 0.0f;
                    TempWeapon.SetBulletSpeed = 0.0f;
                    TempWeapon.SetBulletLifeTime = 2.0f;
                    TempWeapon.SetBulletPierce = 10000;
                }
                break;

            case BulletManager.eBulletType.BulletType_RuneTracer:
                {
                    TempWeapon.SetAttackDelay = 1.0f;
                    TempWeapon.SetAttackCount = 1;
                    TempWeapon.SetAttackRange = 0.0f;
                    TempWeapon.SetAttackSpeed = 1.0f / (3.0f); // sec

                    TempWeapon.SetBullet(Bulletmgr.RuneTracer, BulletManager.eBulletType.BulletType_RuneTracer);

                    TempWeapon.SetBulletDamage = 3.0f;
                    TempWeapon.SetBulletKnockback = 0.0f;
                    TempWeapon.SetBulletSpeed = 20.0f;
                    TempWeapon.SetBulletLifeTime = 10.0f;
                    TempWeapon.SetBulletPierce = 10000;
                }
                break;
            default:

                break;
        }

        TempWeapon.SetMaster(gameObject);
        //TempWeapon.SetWeaponSlot = m_WeaponSlot.Count;
        m_WeaponSlot.Add(TempWeapon);
    }
    private void Reset()
    {
        m_fHorizontal = m_fVertical = 0;
        //m_eState = ePlayerState.Idle;
        m_eState_Dir = eState_Dir.Right;

        m_vControlDir = m_vMovement = m_vTempNormal = Vector2.zero;
        m_vLastDir = Vector2.right;

        m_Animator = GetComponent<Animator>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_RigidBody = GetComponent<Rigidbody2D>();

        m_UnitStat = GetComponent<Unit>();
        m_UnitStat.m_stStat = default;
        m_UnitStat.m_stStat.fMoveSpeed_Base = 5.0f;
        m_UnitStat.m_stStat.fHp_Base = 10.0f;

        m_UnitStat.ResetHp();

        m_UnitStat.m_stStat.fAttackSpeed_Base = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //m_WeaponSlot[0].SetBulletKnockback = KnockBackForce;
        //m_WeaponSlot[0].SetAttackRange = AttackRange;

        CheckDir();

        SetState();

        CharacterMove();

        Attack();

        #region Test

        // Attack Speed +
        if (Input.GetKeyDown(KeyCode.O))
        {
            m_UnitStat.m_stStat.fAttackSpeed_Buf += 50;

            Debug.Log("\nAttack Speed : " + m_UnitStat.m_stStat.fAttackSpeed_Buf);
        }
        // Attack Speed -
        if (Input.GetKeyDown(KeyCode.P))
        {
            m_UnitStat.m_stStat.fAttackSpeed_Buf -= 50;

            Debug.Log("\nAttack Speed : " + m_UnitStat.m_stStat.fAttackSpeed_Buf);
        }
        #endregion //Test

    }
    #endregion //Logic_Basic

    #region publicFunc


    #endregion // publicFunc

    #region privateFunc
    private Vector2 FindCloseEnemy_Dir()
    {
        Vector2 vRes = new Vector2();
        vRes = transform.position;

        GameObject Enemy = SpawnManager.Instance.FindCloseMonster(gameObject);

        Vector3 vPos_dir = Enemy.transform.position - transform.position;

        #region Old_Ver
        /*
        GameObject[] Enemys = GameObject.FindGameObjectsWithTag("tag_Enemy");

        GameObject Enemy = default;

        Vector3 temp = default;

        float fCompare = 1000000.0f;
        float fTemp = 0.0f;
        foreach (GameObject iter in Enemys)
        {
            temp = iter.transform.position;
            fTemp = Vector3.Distance(iter.transform.position, transform.position);
            if (fCompare > fTemp)
            {
                fCompare = fTemp;
                Enemy = iter;
            }
        }
        */
        #endregion Old_Ver
        vRes = vPos_dir;
        vRes = vRes.normalized;

        return vRes;
    }

    private Vector2 RandomDir()
    {

        Vector2 vRes = new Vector2();
        float fCameraLeft = SpawnManager.Instance.GetCameraEdgePos(SpawnManager.eCameraEdgePos.Left);
        float fCameraRight = SpawnManager.Instance.GetCameraEdgePos(SpawnManager.eCameraEdgePos.Right);
        float fCameraBottom = SpawnManager.Instance.GetCameraEdgePos(SpawnManager.eCameraEdgePos.Bottom);
        float fCameraTop = SpawnManager.Instance.GetCameraEdgePos(SpawnManager.eCameraEdgePos.Top);

        vRes.x = Random.Range(fCameraLeft, fCameraRight);
        vRes.y = Random.Range(fCameraBottom, fCameraTop);

        vRes.Normalize();

        return vRes;
    }

    private void Attack()
    {
        Vector2 vAttackDir = default;
        foreach (Weapon iter in m_WeaponSlot)
        {
            
            switch(iter.WeaponType)
            {
                case (int)BulletManager.eBulletType.BulletType_EnergyBall:
                    {
                        if (!SpawnManager.Instance.CheckActiveMonster())
                            return;
                        vAttackDir = FindCloseEnemy_Dir();
                    }
                    break;
                case (int)BulletManager.eBulletType.BulletType_Melee:
                    {
                        vAttackDir = m_vLastDir;
                    }
                    break;
                case (int)BulletManager.eBulletType.BulletType_HolyBomb:
                    {
                        //vAttackDir = m_vLastDir;
                    }
                    break;
                case (int)BulletManager.eBulletType.BulletType_RuneTracer:
                    {
                        //vAttackDir = m_vLastDir;
                        vAttackDir = RandomDir();
                    }
                    break;
                default:
                    break;
            }
            iter.Attack(transform.position, vAttackDir, m_UnitStat.fCurAttackSpeed);
        }
    }

    private void CheckDir()
    {
        m_fHorizontal = m_fVertical = 0;

        m_fHorizontal = Input.GetAxis("Horizontal");
        m_fVertical = Input.GetAxis("Vertical");

        m_vTempNormal.x = m_fHorizontal;
        m_vTempNormal.y = m_fVertical;

        m_vControlDir = m_vTempNormal.normalized;
        if(m_vLastDir != m_vControlDir && m_vControlDir != Vector2.zero)
        {
            m_vLastDir = m_vControlDir;
        }
    }

    private void CharacterMove()
    {
        //m_vMovement = m_vControlDir * m_UnitStat.m_stStat.fMoveSpeed_Base * Time.deltaTime;
        m_vMovement = m_vControlDir * m_UnitStat.m_stStat.fMoveSpeed_Base;

        //transform.Translate(m_vMovement);
        m_RigidBody.velocity = m_vMovement;
    }

    private void SetState()
    {
        //if (Mathf.Abs(m_fHorizontal) > 0 || Mathf.Abs(m_fVertical) > 0)
        if (isPressArrowKey())
            {
            //m_eState = ePlayerState.Move;
            m_Animator.SetBool("isWalk", true);
            m_eState_Dir = m_fHorizontal > 0 ? eState_Dir.Right : eState_Dir.Left;
        }
        else
        {
            if (Mathf.Abs(m_fHorizontal) > 0 || Mathf.Abs(m_fVertical) > 0)
            {
                //m_Animator.SetBool("isWalk", true);
            }
            else
            {
                m_Animator.SetBool("isWalk", false);
            }
        }

        //Debug.Log(m_fHorizontal);
        //Debug.Log(m_fVertical);
    }
    private void Flip()
    {
        switch (m_eState_Dir)
        {
            case eState_Dir.Left:
                {
                    m_SpriteRenderer.flipX = true;
                }
                break;
            case eState_Dir.Right:
                {
                    m_SpriteRenderer.flipX = false;
                }
                break;
        }
    }
    private bool isPressArrowKey()
    {
        if( Input.GetKey(KeyCode.UpArrow) ||
            Input.GetKey(KeyCode.DownArrow) ||
            Input.GetKey(KeyCode.LeftArrow) ||
            Input.GetKey(KeyCode.RightArrow))
        {
            return true;
        }
        return false;
    }
    #endregion // privateFunc

}
