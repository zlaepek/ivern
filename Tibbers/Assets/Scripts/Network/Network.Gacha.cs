using System;
using System.Collections.Generic;
using UnityEngine;

public partial class NetworkManager : MonoBehaviour
{
    #region Gacha Server Call
    public void RequestGetDancha()
    {
        string url = serverUrl + "/" + gacha + "/1";
        StartCoroutine(RequestGet(url, "", CallBackGetDancha));
    }

    public void CallBackGetDancha(string json)
    {
        GachaJson info = new GachaJson(json);
        Debug.Log(info.type + info.part);
    }

    public void RequestGetGacha10()
    {
        string url = serverUrl + "/" + gacha + "/10";
        StartCoroutine(RequestGet(url, "", CallBackGetGacha10));
    }

    public void CallBackGetGacha10(string json)
    {
        List<GachaJson> gachaJsonList = ParseGachaJsonList(json);

        foreach (var gachaJson in gachaJsonList)
        {
            Debug.Log($"Type: {gachaJson.type}, Part: {gachaJson.part}");
        }
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

