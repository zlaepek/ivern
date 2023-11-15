using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Gacha : MonoBehaviour
{
    public GameObject gachaResultPrefab = null;

    public IObjectPool<GameObject> GachaResultPool { get; set; }

    public GameObject resultPrefab = null;
    public void ShowGachaListResult(List<GachaJson> gachaResults)
    {

    }

    public void ShowGachaResult(GachaJson gachaResult)
    {

    }
}
