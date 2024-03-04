using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class UserDataManager : MonoBehaviour
{
    [SerializeField]
    public byte nScale = 0;

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

        TempTableData();

        InitOutGameData();

        GetStat();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SendID()
    {
        string UserID;
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                {
                    // Android ID 가져오기
                    UserID = GetAndroidID();

                    // 가져온 Android ID를 사용하거나 저장
                    Debug.Log("Android ID: " + UserID);
                }
                break;

            case RuntimePlatform.IPhonePlayer:
                {
                    UserID = GetIOSVendorID();

                    Debug.Log("Android ID: " + UserID);
                }
                break;
            default:
                {
                    Debug.Log("This code is intended for Mobile platforms.");
                }
                break;
        }
    }
    private string GetAndroidID()
    {
        AndroidJavaClass androidUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject androidCurrentActivity = androidUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject androidContentResolver = androidCurrentActivity.Call<AndroidJavaObject>("getContentResolver");
        AndroidJavaClass androidSecure = new AndroidJavaClass("android.provider.Settings$Secure");
        string androidID = androidSecure.CallStatic<string>("getString", androidContentResolver, "android_id");

        return androidID;
    }

    private string GetIOSVendorID()
    {
        string vendorID = UnityEngine.iOS.Device.vendorIdentifier;

        return vendorID;
    }

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

    // btn 6
    //Debug.Log(info.health);
    public void Btn_Stat_Health()
    {

    }
    //Debug.Log(info.attack_count);
    public void Btn_Stat_Attack_count()
    {

    }
    //Debug.Log(info.attack_speed);
    public void Btn_Stat_Attack_speed()
    {

    }
    //Debug.Log(info.move_speed);
    public void Btn_Stat_Move_speed()
    {

    }
    //Debug.Log(info.projectile_speed);
    public void Btn_Stat_Projectile_speed()
    {

    }
    //Debug.Log(info.projectile_scale);
    public void Btn_Stat_Projectile_scale()
    {

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

        NetworkManager.Instance?.RequestUpdateStat(iUser_ID, "");
        NetworkManager.Instance?.RequestResetStat(iUser_ID);
    }

    public void CallBackGetStat(string json)
    {
        StatJson info = StatJson.FromJson(json);
        Debug.Log(info.health);
        Debug.Log(info.attack_count);
        Debug.Log(info.attack_speed);
        Debug.Log(info.move_speed);
        Debug.Log(info.projectile_speed);
        Debug.Log(info.projectile_scale);

        _stOutGameData.iHealth = info.health;
        _stOutGameData.iAttack_Count = info.attack_count;
        _stOutGameData.iAttack_Speed = info.attack_speed;
        _stOutGameData.iMove_Speed = info.move_speed;
        _stOutGameData.iProjectile_Speed = info.projectile_speed;
        _stOutGameData.iProjectile_Scale = info.projectile_scale;
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

        public static StatJson FromJson(string jsonString)
        {
            return JsonUtility.FromJson<StatJson>(jsonString);
        }
    }
    #endregion
}
