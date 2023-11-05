using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaButton : MonoBehaviour
{
    #region Gacha Server Call
    public void RequestGetDancha()
    {
        string url = NetworkManager.serverUrl + "/" + NetworkManager.gacha + "/1";
        StartCoroutine(NetworkManager.Instance.RequestGet(url,  "", CallBackGetDancha));
    }

    public void CallBackGetDancha(string json)
    {
        GachaJson info = GachaJson.FromJSON(json);
        Debug.Log(json);
    }

    public void RequestGetGacha100()
    {
        string url = NetworkManager.serverUrl + "/" + NetworkManager.gacha + "/10";
        StartCoroutine(NetworkManager.Instance.RequestGet(url, "", CallBackGetGacha100));
    }

    public void CallBackGetGacha100(string json)
    {
        GachaJson info = GachaJson.FromJSON(json);
        Debug.Log(json);
    }
    #endregion
}

[System.Serializable]   
public class GachaJson
{
    public string type;
    public string part;

    public static GachaJson FromJSON(string jsonString)
    {
        return JsonUtility.FromJson<GachaJson>(jsonString);
    }
}
