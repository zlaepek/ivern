using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    // Buff table
    public enum eBuff_Type
    {
        // 이동 속도 감소
        Slow_MoveSpeed,
        // 조종 불가
        NoControl,

        Max,
    }

    #region Static TimeManager
    private static BuffManager _instance = null;
    public static BuffManager Instance
    {
        get
        {
            return _instance;

        }
    }
    #endregion

    #region Life Cycle (Initialize)
    private void Awake()
    {
        if (Instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


    }
    #endregion

    Structs.BuffData[] BuffList;

    public void ActiveBuff(int _BuffType, float _Time_Sec)
    {
        BuffList[_BuffType].isActive = true;
        BuffList[_BuffType].fRemain_Time = _Time_Sec;

    }

    private void BuffProc()
    {
        for (int i = 0; i < (int)eBuff_Type.Max; ++i)
        {
            BuffList[i].fRemain_Time -= Time.deltaTime;

            if(BuffList[i].fRemain_Time < 0f)
            {
                BuffList[i].fRemain_Time = 0f;
                BuffList[i].isActive = false;
            }
        }
    }

    #region BuffProc
    //  BuffProc : 

    private void Proc_SlowMoveSpeed()
    {

    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        BuffList = new Structs.BuffData[(int)eBuff_Type.Max];

        for(int i = 0; i < (int)eBuff_Type.Max; ++i)
        {
            BuffList[i] = default;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
