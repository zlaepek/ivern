using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

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
    private float m_fMoveSpeed;
    private float m_fHorizontal;
    private float m_fVertical;
    private Vector2 m_vControlDir;
    private Vector2 m_vTempNormal;
    private Vector2 m_vMovement;

    private Animator m_Animator;
    private SpriteRenderer m_SpriteRenderer;

    //public ePlayerState State{ get { return m_eState; } }
    public eState_Dir State_Dir { get { return m_eState_Dir; } }
    #endregion // Private_var

    #region Logic_Basic
    void Awake()
    {
        Reset();
    }

    private void Reset()
    {
        m_fHorizontal = m_fVertical = 0;
        //m_eState = ePlayerState.Idle;
        m_eState_Dir = eState_Dir.Right;

        m_fMoveSpeed = 5.0f;
        m_vControlDir = m_vMovement = m_vTempNormal = Vector2.zero;

        m_Animator = GetComponent<Animator>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        CheckDir();

        SetState();

        CharacterMove();
    }
    #endregion //Logic_Basic

    #region publicFunc


    #endregion // publicFunc

    #region privateFunc
    private void CheckDir()
    {
        m_fHorizontal = m_fVertical = 0;

        m_fHorizontal = Input.GetAxis("Horizontal");
        m_fVertical = Input.GetAxis("Vertical");

        m_vTempNormal.x = m_fHorizontal;
        m_vTempNormal.y = m_fVertical;

        m_vControlDir = m_vTempNormal.normalized;
    }

    private void CharacterMove()
    {
        m_vMovement = m_vControlDir * m_fMoveSpeed * Time.deltaTime;

        transform.Translate(m_vMovement);
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
