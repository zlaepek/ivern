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

    // ���� ���̾��Ű�� ��ũ��Ʈ�� ������ ����ؾ���
    public GameObject EnergyBall;
    public GameObject Melee;

    // Start is called before the first frame update
    void Start()
    {
        // ���߿� ��ũ��Ʈ�� �� źȯ���� �������� �������� �Լ��� ���� ��
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