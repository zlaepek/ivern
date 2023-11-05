using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public static BulletManager Instance { get; private set; }
    public enum eBulletType
    {
        // ����
        BulletType_Melee,
        // ���� �ϵ�
        BulletType_EnergyBall,
        //HolyBomb_0
        BulletType_HolyBomb,
        // RuneTracer
        BulletType_RuneTracer,

        BulletType_MAX,
    }

    // ���� ���̾��Ű�� ��ũ��Ʈ�� ������ ����ؾ���
    public GameObject HolyBomb;
    public GameObject EnergyBall;
    public GameObject Melee;
    public GameObject RuneTracer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

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