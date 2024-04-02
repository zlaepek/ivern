using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageManager : MonoBehaviour
{
    #region Static StageManager
    private static StageManager _instance = null;
    public static StageManager Instance
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

    // Stage Table - 스테이지 정보 표기를 위해 필요
    public enum eStageType
    {
        Empty,
        
        Normal,
        Boss,

        
        ETC,

        Max,
    }

    // 0 - Game , 1 - Test
    private int iChapterSize = 2;
    private int iStageMaxSize = 10;

    private eStageType[,] StageInfo;

    public int CurChapter { get { return iCurChapter; } private set { } }
    public int CurStage { get { return iCurStage; } private set { } }
    private int iCurChapter;
    private int iCurStage;

    public bool IsBossStage()
    {

        return GetStageType() == eStageType.Boss ? true : false;
    }

    public eStageType GetStageType()
    {
        return StageInfo[iCurChapter, iCurStage];
    }

    public void SetCurrentStage(int _iChapter , int _iStage)
    {
        iCurChapter = _iChapter;
        iCurStage = _iStage;
    }

    public void NoPlayStage()
    {
        iCurChapter = -1;
        iCurStage = -1;
    }
    public void Setting_UI_Stage_Scene()
    {
        GameObject Root = GameManager.Instance.FindAllObject("Current_Stage");
        if (!Root)
            return;
        Root.SetActive(true);

        GameObject TextObject_Chapter = GameObject.Find("Text_Chapter");
        if (TextObject_Chapter)
        TextObject_Chapter.GetComponent<TextMeshProUGUI>().text = "" + CurChapter;

        GameObject TextObject_Stage = GameObject.Find("Text_Stage");
        if (TextObject_Stage)
        TextObject_Stage.GetComponent<TextMeshProUGUI>().text = "" + CurStage;
    }

    // Start is called before the first frame update
    void Start()
    {
        StageInfo = new eStageType[iChapterSize, iStageMaxSize];

        for(int i = 0; i < iChapterSize; ++i)
        {
            for(int j = 0; j < iStageMaxSize; ++j)
            {
                StageInfo[i, j] = eStageType.Empty;
            }
        }

        //StageInfo[0, 0] = eStageType.Normal;
        //StageInfo[0, 1] = eStageType.Normal;
        //StageInfo[0, 2] = eStageType.Normal;
        //StageInfo[0, 3] = eStageType.Boss;

        StageInfo[0, 0] = eStageType.Normal;
        StageInfo[0, 1] = eStageType.Boss;

        NoPlayStage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
