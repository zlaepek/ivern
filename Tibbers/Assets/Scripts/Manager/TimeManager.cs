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

    private const float fTotalTime = 30f; // 5 min
    private float fCurrentTime = 0f;
    private GameObject TextObject_Time;
    private float Origin_TimeScale = 0f;

    public void PauseTime()
    {
        Origin_TimeScale = Time.timeScale;
        Time.timeScale = 0f;
    }

    public void ResumeTime()
    {
        Time.timeScale = Origin_TimeScale;
    }

    public void HideTime()
    {
        GameObject TimeObject = GameObject.Find("Time_Normal");
        if (!TimeObject)
            return;
        TimeObject.SetActive(false);
    }

    public void OpenTime()
    {
        GameObject Root = GameManager.Instance.FindAllObject("Current_Stage");
        if (!Root)
            return;
        Root.SetActive(true);

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
        if (!StageManager.Instance)
            return;
        if(!StageManager.Instance.IsBossStage())
        {
            if (fCurrentTime < 0)
            {
                fCurrentTime = 0f;

                PauseTime();

                GameObject Root = GameManager.Instance.FindAllObject("Clear_PopUp");
                if (!Root)
                    return;
                Root.SetActive(true);

            }
            else
            {
                GameObject Root = GameManager.Instance.FindAllObject("Clear_PopUp");
                if (!Root)
                    return;
                Root.SetActive(false);
            }
        }
        
    }

    private void  InitTime()
    {
        fCurrentTime = fTotalTime;
        Update_TimerDisPlay();
    }

    private void Update_TimerDisPlay()
    {
        GameObject Root = GameObject.Find("Current_Stage");
        if (!Root)
            return;
        Root.SetActive(true);

        TextObject_Time = GameObject.Find("Current_Time");
        
        if(!TextObject_Time)
            return;
        
        if (StageManager.Instance.IsBossStage())
            return;

        TextObject_Time.GetComponent<TextMeshProUGUI>().text = "" + fCurrentTime.ToString("F2");
    }
}
