using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    #region Static TimeManager
    private static TimeManager _instance = null;
    public static TimeManager Instance
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

    private const float fTotalTime = 300f; // 5 min
    private float fCurrentTime = 0f;
    private GameObject TextObject_Time;

    public void HideTime()
    {
        GameObject TimeObject = GameObject.Find("Time_Normal");
        if (!TimeObject)
            return;
        TimeObject.SetActive(false);
    }

    public void OpenTime()
    {
        GameObject TimeObject = GameObject.Find("Time_Normal");
        if (!TimeObject)
            return;
        TimeObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        TextObject_Time = GameObject.Find("Current_Time");
        InitTime();
    }

    // Update is called once per frame
    void Update()
    {
        ProcTime();
        Update_TimerDisPlay();
    }

    private void ProcTime()
    {
        fCurrentTime -= Time.deltaTime;

        if(fCurrentTime < 0 )
        {
            fCurrentTime = 0f;
        }
    }

    private void  InitTime()
    {
        fCurrentTime = fTotalTime;
        Update_TimerDisPlay();
    }

    private void Update_TimerDisPlay()
    {
        TextObject_Time = GameObject.Find("Current_Time");
        
        if(!TextObject_Time)
            return;
        
        if (StageManager.Instance.IsBossStage())
            return;

        TextObject_Time.GetComponent<TextMeshProUGUI>().text = "" + fCurrentTime.ToString("F2");
    }
}
