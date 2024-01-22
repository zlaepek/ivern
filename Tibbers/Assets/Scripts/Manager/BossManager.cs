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
    private BossUI _bossUI = null;

    public BossUI BossUI
    {
        get
        {
            if (_bossUI == null)
            {
                _bossUI = FindObjectOfType<BossUI>();
            }

            return _bossUI;

        }
        set
        {
            _bossUI = value;
        }
    }



    // [보스]
    [HideInInspector]
    public GameObject currentBoss = null;
    private GameObject _currentBossPrefab = null;
    // 보스 프리팹 관리
    public List<BossDictionary> bossPrefabs = new List<BossDictionary>();

    // [바운더리]
    // 보스 소환 위치
    public GameObject spawnPositionMarker = null;
    public Boundary boundary = null;
    // 플레이어 위치에 바운더리 생성하기 위함
    private Transform _playerTransform = null;



    ///////// 테스트용 Spawn()
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Spawn(BossName.Mandoo);
        }
    }

    public void Spawn(BossName bossName)
    {
        BossUI.SpawnNewBoss(bossName);
        boundary.SpawnNewBoss(PlayerTransform.position);

        _currentBossPrefab = bossPrefabs.Find(boss => boss.bossName == bossName).bossPrefab;
        StartCoroutine(InstantiateBossPrefab());
    }

    public IEnumerator InstantiateBossPrefab()
    {
        spawnPositionMarker.transform.position = _playerTransform.position;
        spawnPositionMarker.SetActive(true);
        yield return new WaitForSeconds(1.0f);

        spawnPositionMarker.SetActive(false);
        currentBoss = Instantiate(_currentBossPrefab);
        currentBoss.transform.position = spawnPositionMarker.transform.position;
        yield return null;
    }

    public void BossClear()
    {
        BossUI.HideBossUI();
        currentBoss = null;
    }

}

