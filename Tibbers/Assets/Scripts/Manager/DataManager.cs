using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DataManager : MonoBehaviour
{
    public enum eExpBall_Type
    {
        A,
        B,
        C,
        D,

        eExpBall_Max,
    }

    public static DataManager Instance { get; private set; }

    private Structs.PlayerGameData stPlayerData;

    public Structs.PlayerGameData PlayerData { get {return stPlayerData; }  private set { stPlayerData = value; } }

    private List<Structs.ExpData> listExpDataTable;
    public List<Structs.ExpData> ExpDataTable { get { return listExpDataTable; } private set { listExpDataTable = value; } }

    private List<ulong> listExpBallTable;
    public List<ulong> ExpBallTable { get { return listExpBallTable; } private set { listExpBallTable = value; } }

    public ulong[] Exp_Require { get; private set; }

    public ulong[] ExpBall_Table { get; private set; }

    public void Add_Kill_Score(ulong _ulKill_Score)
    {
        stPlayerData.ulStageKill += _ulKill_Score;

        stPlayerData.ulTotalKill += _ulKill_Score;

        Debug.Log("Stage_Kill_Score : " + stPlayerData.ulStageKill);
    }

#if (__Not_Use__)
    public class GachaItem
    {
        public string itemName;
        public float dropRate;
    }

    public List<GachaItem> itemList = new List<GachaItem>();

    private float totalWeight = 0f;

    private void Start()
    {
        // 확률 합계 계산
        foreach (var item in itemList)
        {
            totalWeight += item.dropRate;
        }
    }

    public string DrawItem()
    {
        float randomValue = UnityEngine.Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;

        foreach (var item in itemList)
        {
            cumulativeWeight += item.dropRate;
            if (randomValue <= cumulativeWeight)
            {
                return item.itemName;
            }
        }

        return "No Item"; // 기본값 또는 에러 처리
    }
    
#endif
    public void ResetPlayerData()
    {
        stPlayerData = default;

        stPlayerData.iLevel = 1;

        ExpUI_LevelUp();
    }
    private void Awake()
    {
        listExpDataTable = new List<Structs.ExpData>();

        listExpBallTable = new List<ulong>();

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        Structs.ExpData TempData = default;
        
        ulong Exp_Total = 0;
        Exp_Require = new ulong[] { 0, 10, 15, 25, 40 };

        for (uint i = 0; i < Exp_Require.Length; ++i)
        {
            TempData.ulRequire = Exp_Require[i];
            Exp_Total += TempData.ulRequire;
            TempData.ulTotal = Exp_Total;

            listExpDataTable.Add(TempData);
        }
        
        // 2, 4, 6, 8
        for(uint i = 0; i < (uint)eExpBall_Type.eExpBall_Max ; ++i)
        {
            listExpBallTable.Add(2 * (i + 1));
        }

        ResetPlayerData();

    }

    public void Get_Exp(int _iType)
    {
        ulong TempA = listExpBallTable[_iType];

        stPlayerData.ulExp_Current += TempA;
        stPlayerData.ulExp_Total += TempA;
        //stPlayerData.ulExp_Current = _iType;
        Check_Level_Up();

        if (PlayerData.iLevel >= ExpDataTable.Count)
        {
            stPlayerData.ulExp_Current = 0;
            // 임시로 Exp 무한 누적
            //stPlayerData.ulExp_Total = ExpDataTable[ExpDataTable.Count - 1].ulTotal;

        }
        ExpUI_GetExp();

        Debug.Log("ExpValue : " + TempA + ", " + " :: Level : " + PlayerData.iLevel + " :: Exp_Cur : " + PlayerData.ulExp_Current + ":: Exp_Total : " + PlayerData.ulExp_Total);
    }

    private void Check_Level_Up()
    {
        if (PlayerData.iLevel >= ExpDataTable.Count)
        {
            return;
        }

        if(PlayerData.ulExp_Current >= ExpDataTable[PlayerData.iLevel].ulRequire)
        {
            stPlayerData.ulExp_Current -= ExpDataTable[PlayerData.iLevel].ulRequire;
            ++stPlayerData.iLevel;

            // 레벨업 이벤트 ( 무기 고르기 등등 )
            ExpUI_LevelUp();
        }
    }

    private void ExpUI_LevelUp()
    {
        GameObject Root = GameManager.Instance.FindAllObject("ExpUI");
        if (!Root)
            return;
        Root.SetActive(true);


        GameObject Obj_Slider_Exp = GameObject.Find("Slider_Exp");
        if (!Obj_Slider_Exp)
            return;
        Slider Slider_Exp = Obj_Slider_Exp.GetComponent<Slider>();

        Slider_Exp.maxValue = ExpDataTable[PlayerData.iLevel].ulRequire;
        Slider_Exp.minValue = 0;
        Slider_Exp.value = PlayerData.ulExp_Current;

        //Text_Exp
        TextMeshProUGUI Text_lvl = GameObject.Find("Text_Exp").GetComponent<TextMeshProUGUI>();
        Text_lvl.text = "Lv. " + PlayerData.iLevel;
    }

    private void ExpUI_GetExp()
    {
        GameObject Root = GameObject.Find("ExpUI");
        if (!Root)
            Root = GameManager.Instance.FindAllObject("ExpUI");
        Root.SetActive(true);

        Slider Slider_Exp = GameObject.Find("Slider_Exp").GetComponent<Slider>();

        Slider_Exp.value = PlayerData.ulExp_Current;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
