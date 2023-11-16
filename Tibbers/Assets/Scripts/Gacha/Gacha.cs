using System;
using System.Collections.Generic;
using UnityEngine;

public class Gacha : MonoBehaviour
{
    public GameObject singleResultPrefab = null;

    public int gachaMaxNumber = 10;
    public GameObject gachaResultsPrefab = null;
    public List<GameObject> gachaResults = new List<GameObject>();

    public Transform gachaResultsTransform = null;

    private void Start()
    {
        InitGachaResultList();
    }
    private void InitGachaResultList()
    {
        for (int i = 0; i < gachaMaxNumber; i++)
        {
            gachaResults.Add(Instantiate(gachaResultsPrefab, gachaResultsTransform));
        } 
    }

    public void ButtonDancha()
    {
        NetworkManager.Instance?.RequestGetDancha(CallBackGetDancha);
    }

    public void ButtonGacha10()
    {
        NetworkManager.Instance?.RequestGetGacha10(CallBackGetGacha10);
    }

    public void CallBackGetDancha(string json)
    {
        GachaJson info = new GachaJson(json);
        Debug.Log(info.type + info.part);
    }

    public void CallBackGetGacha10(string json)
    {
        List<GachaJson> gachaJsonList = new List<GachaJson>();

        string[] jsonSubStringArray = json.Split(new[] { "[" }, StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < 10; i++)
        {
            GachaJson gachaJson = new GachaJson(jsonSubStringArray[i]);

            gachaJsonList.Add(gachaJson);
        }

        ShowGachaListResult(gachaJsonList);
    }

    public void ShowGachaListResult(List<GachaJson> gachaJsonList)
    {
        for (int i = 0; i < 10; i++)
        {
            gachaResults[i].GetComponent<GachaItem>().SetPartText(gachaJsonList[i].part);
            gachaResults[i].GetComponent<GachaItem>().SetTypeText(gachaJsonList[i].type);
            
        }
    }

    public void ShowGachaResult(GachaJson gachaResult)
    {

    }
}
