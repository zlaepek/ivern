using System;
using System.Collections.Generic;
using UnityEngine;

public partial class NetworkManager : MonoBehaviour
{
    #region Gacha Server Call
    public void RequestGetDancha(Action<string> callBack)
    {
        string url = serverUrl + "/" + gacha + "/1";
        StartCoroutine(RequestGet(url, "", callBack));
    }


    public void RequestGetGacha10(Action<string> callBack)
    {
        string url = serverUrl + "/" + gacha + "/10";
        StartCoroutine(RequestGet(url, "", callBack));
    }
    #endregion

    private List<GachaJson> ParseGachaJsonList(string jsonString)
    {
        List<GachaJson> gachaJsonList = new List<GachaJson>();

        string[] jsonArray = jsonString.Split(new[] { "[" }, StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < 10; i++)
        {
            GachaJson gachaJson = new GachaJson(jsonArray[i]);

            gachaJsonList.Add(gachaJson);
        }

        return gachaJsonList;
    }

}

[System.Serializable]
public class GachaJson
{
    public string type;
    public string part;

    public GachaJson (string jsonString)
    {
        string[] subStrings = jsonString.Split("\"");
        type = subStrings[1];
        part = subStrings[3];
    }
}

