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


public enum BossBulletType : int
{
    mandooBullet = 0,
}

[Serializable]
public class BossBulletDictionary
{
    public BossBulletType bulletType;
    public GameObject bulletPrefab;
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


    // [����ü]
    public List<BossBulletDictionary> bossBulletPrefabs = new List<BossBulletDictionary>();

    ///////// �׽�Ʈ�� Spawn()
    
    private void Start()
    {
        if(BossUI)
            BossUI.HideBossUI();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Spawn(BossName.Mandoo);
        }
    }

    public void Spawn(BossName bossName)
    {
        // BossUI �̰� � ��Ȳ���� nullptr �� ���� �׻� null �� ä���ְ� �ٲ����
        FindBossUI();
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

    public void FindBossUI()
    {
        BossUI = GameManager.Instance.FindAllObject("BossUI").GetComponent<BossUI>();
    }

    public void BossClear()
    {
        BossUI.HideBossUI();
        currentBoss = null;
    }

    public GameObject ShotBullet(BossBulletType bulletType, Vector3 direction, Transform startPosition) {
        float fAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(fAngle, Vector3.forward);

        return Instantiate(bossBulletPrefabs[(int)bulletType].bulletPrefab, startPosition.position, rotation);
    }
}