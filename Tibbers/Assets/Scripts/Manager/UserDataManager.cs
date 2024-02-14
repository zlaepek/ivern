using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataManager : MonoBehaviour
{
    [SerializeField]
    public byte nScale = 0;

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
        _stOutGameData_Value.fDamage = _stOutGameData_Table.fDamage * _stOutGameData.nDamage;
        _stOutGameData_Value.fHealth = _stOutGameData_Table.fHealth * _stOutGameData.nHealth;
        _stOutGameData_Value.fMove_Speed = _stOutGameData_Table.fMove_Speed * _stOutGameData.nMove_Speed;
        _stOutGameData_Value.fAttack_Speed = _stOutGameData_Table.fAttack_Speed * _stOutGameData.nAttack_Speed;
        _stOutGameData_Value.fProjectile_Speed = _stOutGameData_Table.fProjectile_Speed * _stOutGameData.nProjectile_Speed;
        _stOutGameData_Value.fProjectile_Scale = _stOutGameData_Table.fProjectile_Scale * _stOutGameData.nProjectile_Scale;
        _stOutGameData_Value.iAttack_Count = _stOutGameData_Table.iAttack_Count * _stOutGameData.nAttack_Count;
    }
}
