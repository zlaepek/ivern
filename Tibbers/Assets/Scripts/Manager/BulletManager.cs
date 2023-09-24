using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public enum eBulletType
    {
        BulletType_Melee,
        BulletType_EnergyBall,

        BulletType_MAX,
    }

    // 사용시 하이어아키에 스크립트에 프리팹 등록해야함
    public GameObject EnergyBall;
    public GameObject Melee;

    // Start is called before the first frame update
    void Start()
    {
        // 나중에 스크립트로 각 탄환들의 프리팹을 가져오는 함수를 만들 것
    }

    // Update is called once per frame
    void Update()
    {

    }

    //public static BulletManager Instance { get; private set; }

    //private void Awake()
    //{
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}

}