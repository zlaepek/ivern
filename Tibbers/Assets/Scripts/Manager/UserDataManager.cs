using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class UserDataManager : MonoBehaviour
{
    [SerializeField]
    //public byte nScale = 0;

    private int iUser_ID = 4566;

    private static GameObject _container;
    public static GameObject Container
    {
        get
        {
            return _container;
        }
    }
    public static UserDataManager _intance;
    public static UserDataManager Instance
    {
        //private set
        //{

        //}

        //get
        //{
        //    if (!_intance)
        //    {
        //        _container = new GameObject();
        //        _container.name = "UserData";
        //        _intance = _container.AddComponent(typeof(UserData)) as UserData;
        //    }

        //    return _intance;
        //}

        private set; get;

    }
    private Structs.OutGameStatData _stOutGameData;
    private Structs.OutGameStatData _stOutGameData_Max;

    private Structs.OutGameStatData_Value _stOutGameData_Table;
    private Structs.OutGameStatData_Value _stOutGameData_Value;

    public Structs.OutGameStatData_Value stOutGameData_Value
    {
        get
        {
            return _stOutGameData_Value;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        _stOutGameData = default;
        _stOutGameData.iDamage = 4;
        _stOutGameData.iHealth = 5;
        

        InitDataMax();

        TempTableData();

        GetStat();

        InitOutGameData();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SendID()
    {
        string UserID;
        switch (Application.platform)
        {
#if UNITY_ANDROID
            case RuntimePlatform.Android:
                {
                    // Android ID 가져오기
                    UserID = GetAndroidID();

                    // 가져온 Android ID를 사용하거나 저장
                    Debug.Log("Android ID: " + UserID);
                }
                break;
#endif
#if UNITY_IOS
            case RuntimePlatform.IPhonePlayer:
                {
                    UserID = GetIOSVendorID();

                    Debug.Log("Android ID: " + UserID);
                }
                break;
#endif
            default:
                {
                    Debug.Log("This code is intended for Mobile platforms.");
                    UserID = "No Platform";
                }
                break;
        }

        GameObject TextID = GameObject.Find("Text_ID");

        TextID.GetComponent<TextMeshProUGUI>().text = UserID;
    }
#if UNITY_ANDROID
    private string GetAndroidID()
    {
        AndroidJavaClass androidUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject androidCurrentActivity = androidUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject androidContentResolver = androidCurrentActivity.Call<AndroidJavaObject>("getContentResolver");
        AndroidJavaClass androidSecure = new AndroidJavaClass("android.provider.Settings$Secure");
        string androidID = androidSecure.CallStatic<string>("getString", androidContentResolver, "android_id");

        return androidID;
    }
#endif
#if UNITY_IOS
    private string GetIOSVendorID()
    {
        string vendorID = UnityEngine.iOS.Device.vendorIdentifier;

        return vendorID;
    }
#endif
    public void TempTableData()
    {
        Structs.OutGameStatData_Value Temp = new Structs.OutGameStatData_Value();

        Temp.fDamage = 1.0f;
        Temp.fHealth = 1.0f;
        Temp.fMove_Speed = 1.0f;
        Temp.fProjectile_Scale = 10.0f;     // 퍼센트
        Temp.fProjectile_Speed = 10.0f;     // 퍼센트
        Temp.fAttack_Speed = 10.0f;         // 퍼센트
        Temp.iAttack_Count = 1;

        InitOutGameDataTable(Temp);
    }

    public void InitOutGameDataTable(Structs.OutGameStatData_Value _Data)
    {
        _stOutGameData_Table = _Data;
    }


    // _stOutGameData_Max
    public void InitDataMax()
    {
        // Temp Stat Max Table
        //Damage	5
        //Health	5
        //Attack_Count	2
        //Attack_Speed	5
        //Move_Speed	5
        //Projectile_Speed	5
        //Projectile_Scale	2

        // sheet 불러와서 값 저장하기
        _stOutGameData_Max.iDamage = 5;
        _stOutGameData_Max.iHealth = 5;
        _stOutGameData_Max.iAttack_Count = 2;
        _stOutGameData_Max.iAttack_Speed = 5;
        _stOutGameData_Max.iMove_Speed = 5;
        _stOutGameData_Max.iProjectile_Speed = 5;
        _stOutGameData_Max.iProjectile_Scale = 2;
    }

    private void InitOutGameData()
    {
        // 실제 적용할 값 = 테이블 기준 값 * 아웃게임 데이터
        _stOutGameData_Value.fDamage = _stOutGameData_Table.fDamage * _stOutGameData.iDamage;
        _stOutGameData_Value.fHealth = _stOutGameData_Table.fHealth * _stOutGameData.iHealth;
        _stOutGameData_Value.fMove_Speed = _stOutGameData_Table.fMove_Speed * _stOutGameData.iMove_Speed;
        _stOutGameData_Value.fAttack_Speed = _stOutGameData_Table.fAttack_Speed * _stOutGameData.iAttack_Speed;
        _stOutGameData_Value.fProjectile_Speed = _stOutGameData_Table.fProjectile_Speed * _stOutGameData.iProjectile_Speed;
        _stOutGameData_Value.fProjectile_Scale = _stOutGameData_Table.fProjectile_Scale * _stOutGameData.iProjectile_Scale;
        _stOutGameData_Value.iAttack_Count = _stOutGameData_Table.iAttack_Count * _stOutGameData.iAttack_Count;
    }

#region BtnEvent
    public void Btn_Stat_Reset()
    {
        NetworkManager.Instance?.RequestResetStat(CallBackGetStat, iUser_ID);
    }

    //Debug.Log(info.damage);
    public void Btn_Stat_Damage()
    {
        //Stat_Apply("Damage", _stOutGameData.iDamage, _stOutGameData_Max.iDamage);
        //Stat_Apply("Health", _stOutGameData.iHealth, _stOutGameData_Max.iHealth);
        //Stat_Apply("Attack_Count", _stOutGameData.iAttack_Count, _stOutGameData_Max.iAttack_Count);
        //Stat_Apply("Attack_Speed", _stOutGameData.iAttack_Speed, _stOutGameData_Max.iAttack_Speed);
        //Stat_Apply("Move_Speed", _stOutGameData.iMove_Speed, _stOutGameData_Max.iMove_Speed);
        //Stat_Apply("Projectile_Speed", _stOutGameData.iProjectile_Speed, _stOutGameData_Max.iProjectile_Speed);
        //Stat_Apply("Projectile_Scale", _stOutGameData.iProjectile_Scale, _stOutGameData_Max.iProjectile_Scale);

        //Debug.Log("Damage" +  _stOutGameData.iDamage + _stOutGameData_Max.iDamage);
        //Debug.Log("Health" +  _stOutGameData.iHealth +  _stOutGameData_Max.iHealth);
        //Debug.Log("Attack_Count" + _stOutGameData.iAttack_Count + _stOutGameData_Max.iAttack_Count);
        //Debug.Log("Attack_Speed"+ _stOutGameData.iAttack_Speed+ _stOutGameData_Max.iAttack_Speed);
        //Debug.Log("Move_Speed"+ _stOutGameData.iMove_Speed+ _stOutGameData_Max.iMove_Speed);
        //Debug.Log("Projectile_Speed"+ _stOutGameData.iProjectile_Speed+ _stOutGameData_Max.iProjectile_Speed);
        //Debug.Log("Projectile_Scale"+ _stOutGameData.iProjectile_Scale+ _stOutGameData_Max.iProjectile_Scale);

        NetworkManager.Instance?.RequestUpdateStat(CallBackGetStat, iUser_ID, "damage");
    }
    // btn 6
    //Debug.Log(info.health);
    public void Btn_Stat_Health()
    {
        NetworkManager.Instance?.RequestUpdateStat(CallBackGetStat, iUser_ID, "health");
    }
    //Debug.Log(info.attack_count);
    public void Btn_Stat_Attack_count()
    {
        NetworkManager.Instance?.RequestUpdateStat(CallBackGetStat, iUser_ID, "attack_count");
    }
    //Debug.Log(info.attack_speed);
    public void Btn_Stat_Attack_speed()
    {
        NetworkManager.Instance?.RequestUpdateStat(CallBackGetStat, iUser_ID, "attack_speed");
    }
    //Debug.Log(info.move_speed);
    public void Btn_Stat_Move_speed()
    {
        NetworkManager.Instance?.RequestUpdateStat(CallBackGetStat, iUser_ID, "move_speed");
    }
    //Debug.Log(info.projectile_speed);
    public void Btn_Stat_Projectile_speed()
    {
        NetworkManager.Instance?.RequestUpdateStat(CallBackGetStat, iUser_ID, "projectile_speed");
    }
    //Debug.Log(info.projectile_scale);
    public void Btn_Stat_Projectile_scale()
    {
        NetworkManager.Instance?.RequestUpdateStat(CallBackGetStat, iUser_ID, "projectile_scale");
    }

#endregion

#region Data Proc

    private void Stat_Apply(string _Name, int _Cur, int _Max)
    {
        string TempName = "Text_Step_";
        GameObject TextObject = GameObject.Find(TempName + _Name);

        if(TextObject)
            TextObject.GetComponent<TextMeshProUGUI>().text = _Cur + " / " + _Max;
    }

#endregion

    // Network
#region NetWorkCall

    // sheet_name
    // user_stat
    // user_stat_cost


    public void GetStat()
    {
        NetworkManager.Instance?.RequestGetStat(CallBackGetStat, iUser_ID);
    }

    public void CallBackGetStat(string json)
    {
        Debug.Log("###CallBackGetStat### ==== Start");

        StatJson info = StatJson.FromJson(json);
        Debug.Log(info.damage);
        Debug.Log(info.health);
        Debug.Log(info.attack_count);
        Debug.Log(info.attack_speed);
        Debug.Log(info.move_speed);
        Debug.Log(info.projectile_speed);
        Debug.Log(info.projectile_scale);

        _stOutGameData.iDamage = info.damage;
        _stOutGameData.iHealth = info.health;
        _stOutGameData.iAttack_Count = info.attack_count;
        _stOutGameData.iAttack_Speed = info.attack_speed;
        _stOutGameData.iMove_Speed = info.move_speed;
        _stOutGameData.iProjectile_Speed = info.projectile_speed;
        _stOutGameData.iProjectile_Scale = info.projectile_scale;

        Stat_Apply("Damage", _stOutGameData.iDamage, _stOutGameData_Max.iDamage);
        Stat_Apply("Health", _stOutGameData.iHealth, _stOutGameData_Max.iHealth);
        Stat_Apply("Attack_Count", _stOutGameData.iAttack_Count, _stOutGameData_Max.iAttack_Count);
        Stat_Apply("Attack_Speed", _stOutGameData.iAttack_Speed, _stOutGameData_Max.iAttack_Speed);
        Stat_Apply("Move_Speed", _stOutGameData.iMove_Speed, _stOutGameData_Max.iMove_Speed);
        Stat_Apply("Projectile_Speed", _stOutGameData.iProjectile_Speed, _stOutGameData_Max.iProjectile_Speed);
        Stat_Apply("Projectile_Scale", _stOutGameData.iProjectile_Scale, _stOutGameData_Max.iProjectile_Scale);

        Debug.Log("###CallBackGetStat### ==== End");
    }


    [System.Serializable]
    public class StatJson
    {
        //[Required]
        //user_id *
        public int health;
        public int attack_count;
        public int attack_speed;
        public int move_speed;
        public int projectile_speed;
        public int projectile_scale;
        public int damage;

        public static StatJson FromJson(string jsonString)
        {
            return JsonUtility.FromJson<StatJson>(jsonString);
        }
    }
#endregion
}
