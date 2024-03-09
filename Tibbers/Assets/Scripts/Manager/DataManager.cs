using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        // Ȯ�� �հ� ���
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

        return "No Item"; // �⺻�� �Ǵ� ���� ó��
    }
    
#endif
    public void ResetPlayerData()
    {
        stPlayerData = default;

        stPlayerData.iLevel = 1;
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

        ResetPlayerData();
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
            // �ӽ÷� Exp ���� ����
            //stPlayerData.ulExp_Total = ExpDataTable[ExpDataTable.Count - 1].ulTotal;
        }

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
            
            // ������ �̺�Ʈ ( ���� ���� ��� )
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
