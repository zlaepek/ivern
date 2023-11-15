using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Gacha : MonoBehaviour
{
    public GameObject gachaResultPrefab = null;

    private IObjectPool<GameObject> _gachaResultPool;
    //public IObjectPool<GameObject> GachaResultPool
    //{
    //    get
    //    {
    //        if (_gachaResultPool == null)
    //        {
    //            if (poolType == PoolType.Stack)
    //                _gachaResultPool = new ObjectPool<ParticleSystem>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);
    //            else
    //                _gachaResultPool = new LinkedPool<ParticleSystem>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, maxPoolSize);
    //        }
    //        return _gachaResultPool;
    //    }
    //}

    public GameObject resultPrefab = null;
    public void ShowGachaListResult(List<GachaJson> gachaResults)
    {

    }

    public void ShowGachaResult(GachaJson gachaResult)
    {

    }
}
