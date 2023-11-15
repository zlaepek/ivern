using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public enum BossName
{
    Mandoo,
}

[Serializable]
public class BossDictionary
{
    public BossName bossName;
    public GameObject bossPrefab;
}

public class BossManager : MonoBehaviour
{
    #region Instance
    private static BossManager instance = null;

    public static BossManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BossManager>();
            }
            return instance;
        }
    }
    #endregion

    #region Get Methods
    public Transform PlayerTransform
    {
        get
        {
            if (_playerTransform == null)
            {
                _playerTransform = GameObject.FindGameObjectWithTag("tag_Player").transform;
            }
            return _playerTransform;

        }
    }
    #endregion

    // UI
    public BossUI bossUI = null;


    // [����]
    [HideInInspector]
    public GameObject currentBoss = null;
    private GameObject _currentBossPrefab = null;
    // ���� ������ ����
    public List<BossDictionary> bossPrefabs = new List<BossDictionary>();

    // [�ٿ����]
    // ���� ��ȯ ��ġ
    public GameObject spawnPositionMarker = null;
    public Boundary boundary = null;
    // �÷��̾� ��ġ�� �ٿ���� �����ϱ� ����
    private Transform _playerTransform = null;



    ///////// �׽�Ʈ�� Spawn()
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            Spawn(BossName.Mandoo);
        }
    }

    public void Spawn(BossName bossName)
    {
        bossUI.SpawnNewBoss(bossName);
        boundary.SpawnNewBoss(PlayerTransform.position);

        _currentBossPrefab = bossPrefabs.Find(boss => boss.bossName == bossName).bossPrefab;
        StartCoroutine(InstantiateBossPrefab());
    }

    public IEnumerator InstantiateBossPrefab()
    {
        spawnPositionMarker.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        spawnPositionMarker.SetActive(false);
        currentBoss = Instantiate(_currentBossPrefab);

        yield return null;
    }

    public void BossClear()
    {
        bossUI.HideBossUI();
        currentBoss = null;
    }

}

