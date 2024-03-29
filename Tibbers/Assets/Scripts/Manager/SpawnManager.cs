using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    static int iSerial = 0;
    public static SpawnManager Instance { get; private set; }

    public enum eMonsterMobType
    {
        Test,

        Mob_Egg_Man,
        Mob_Egg_Man_Tall,
        Mob_Eye,

        Max,
    }

    public GameObject Monster_Test;
    public GameObject Monster_Mob_Egg_Man;
    public GameObject Monster_Mob_Egg_Man_Tall;
    public GameObject Monster_Mob_Eye;

    private GameObject[] arrMonster;

    private Camera mainCamera;
    private GameObject playerCharacter;
    private List<GameObject> MonsterPool;

    private bool isSpawning;

    Coroutine CoRt_Spawn;
    public enum eCameraEdgePos
    {
        Top,
        Bottom,
        Left,
        Right,

        Max
    }

    float[] fPositions = new float[(int)eCameraEdgePos.Max];

    private void Awake()
    {
        MonsterPool = new List<GameObject>();

        arrMonster = new GameObject[(int)eMonsterMobType.Max];

        arrMonster[(int)eMonsterMobType.Test] = Monster_Test;
        arrMonster[(int)eMonsterMobType.Mob_Egg_Man] = Monster_Mob_Egg_Man;
        arrMonster[(int)eMonsterMobType.Mob_Egg_Man_Tall] = Monster_Mob_Egg_Man_Tall;
        arrMonster[(int)eMonsterMobType.Mob_Eye] = Monster_Mob_Eye;


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
        GameObject Camera = GameObject.FindGameObjectWithTag("MainCamera");

        mainCamera = Camera.GetComponent<Camera>();

        playerCharacter = GameObject.FindGameObjectWithTag("tag_Player");


        GameObject[] Enemys = GameObject.FindGameObjectsWithTag("tag_Enemy");

        foreach (GameObject iter in Enemys)
        {
            AddPool(iter);
        }

        isSpawning = true;
        CoRt_Spawn = StartCoroutine(SpawnMonster_Routine());
    }

    // Update is called once per frame
    void Update()
    {
        SetCameraPos();


        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    RandomSpawn((int)eMonsterMobType.Test);
        //}

        if (Input.GetKeyDown(KeyCode.T))
        {
            //RandomSpawn((int)eMonsterMobType.Test);
            if(!isSpawning)
            {
                StartSpawn();
            }
            else
            {
                StopSpawn();
            }

        }

        GameObject TempBoss = BossManager.Instance.currentBoss;
        if (TempBoss)
        {
            StopSpawn();
        }
    }

    public float GetCameraEdgePos(eCameraEdgePos _ePos)
    {
        if (_ePos == eCameraEdgePos.Max)
            return 0.0f;


        return fPositions[(int)_ePos];
    }

    public bool CheckActiveMonster()
    {
        foreach (GameObject it in MonsterPool)
        {
            if (it.activeSelf == true)
            {
                return true;
            }
        }
        return false;
    }

    public GameObject FindCloseMonster(GameObject _Object)
    {
        GameObject Res = null;
        
        float fCompare = 0f;
        float fDistance = 10000000.0f;
        Vector3 vPoint = _Object.transform.position;

        // todo : 보스 몬스터 풀? 같은거 만들기
        //BossManager.Instance.currentBoss
        GameObject TempBoss = BossManager.Instance.currentBoss;
        if (TempBoss)
        {
            GameObject[] Monsters = GameObject.FindGameObjectsWithTag("tag_Enemy");

            foreach (GameObject it in Monsters)
            {
                if (it.activeSelf == true)
                {
                    fCompare = Vector3.Magnitude(it.transform.position - vPoint);
                    if (fCompare <= fDistance)
                    {
                        Res = it;
                        fDistance = fCompare;
                    }
                }
            }
        }

        foreach (GameObject it in MonsterPool)
        {
            if(it.activeSelf == true)
            {
                fCompare = Vector3.Magnitude(it.transform.position - vPoint);
                if (fCompare <= fDistance)
                {
                    Res = it;
                    fDistance = fCompare;
                }
            }
        }

        return Res;
    }
     
    private void SetCameraPos()
    {
        float fcamHeight = mainCamera.orthographicSize * 2;
        float fcamWidth = fcamHeight * mainCamera.aspect;
        float fHalfHeight = fcamHeight / 2;
        float fHalfWidth = fcamWidth / 2;

        fPositions[(int)eCameraEdgePos.Top] = playerCharacter.transform.position.y + fHalfHeight;
        fPositions[(int)eCameraEdgePos.Bottom] = playerCharacter.transform.position.y - fHalfHeight;
        fPositions[(int)eCameraEdgePos.Left] = playerCharacter.transform.position.x - fHalfWidth;
        fPositions[(int)eCameraEdgePos.Right] = playerCharacter.transform.position.x + fHalfWidth;
    }

    private GameObject FindNoActiveMonster()
    {
        foreach(GameObject it in MonsterPool)
        {
            if(it.activeSelf == false)
            {
                return it;
            }
        }
        return null;
    }

    private void SpawnObject(int _objectNumber, Vector2 _vPoint)
    {
        GameObject Monster = FindNoActiveMonster();

        if( !Monster)
        {
            Monster = GameObject.Instantiate(arrMonster[_objectNumber], _vPoint, Quaternion.identity);
            AddPool(Monster);
        }
        else
        {
            Monster.SetActive(true);
            Monster.transform.position = _vPoint;
        }
        //InGameUIManager.Instance.SetHpBar(Monster);
        //Debug.Log("\n MonsterSpawn : " + _vPoint.x + ", " + _vPoint.y);


        //InGameUIManager.Instance.fPos_Y
        Mon_Mob Temp_Mob = Monster.GetComponent<Mon_Mob>();
        Temp_Mob.Init(_objectNumber);
        //Debug.Log("Type : " + _objectNumber + ", Mass :" + Temp_Mob.Stat.Mass + ", Speed : " + Temp_Mob.Stat.fCurMoveSpeed);

        //Debug.Log("\n MonsterSpawn : " + _vPoint.x + ", " + _vPoint.y);
    }
    private void AddPool(GameObject _Monster)
    {
        MonsterPool.Add(_Monster);
        _Monster.GetComponent<Mon_Mob>().SerialNumber = iSerial;
        ++iSerial;
    }
    private void RandomSpawn(int _objectNumber)
    {
        // 실수 min , max 모두포함
        // 정수 max 제외
        //Random.Range
        Vector2 vSpawnPoint = default;

        int iRand = Random.Range(0, (int)eCameraEdgePos.Max);
        switch (iRand)
        {
            case (int)eCameraEdgePos.Top:
            case (int)eCameraEdgePos.Bottom:
                {
                    vSpawnPoint.x = Random.Range(fPositions[(int)eCameraEdgePos.Left], fPositions[(int)eCameraEdgePos.Right]);
                    vSpawnPoint.y = fPositions[iRand];
                }
                break;
            case (int)eCameraEdgePos.Left:
            case (int)eCameraEdgePos.Right:
                {
                    vSpawnPoint.x = fPositions[iRand];
                    vSpawnPoint.y = Random.Range(fPositions[(int)eCameraEdgePos.Bottom], fPositions[(int)eCameraEdgePos.Top]);
                }
                break;

            default:
                break;
        }

        SpawnObject(_objectNumber, vSpawnPoint);
    }

    private void StartSpawn()
    {
        Debug.Log("Start Spawn");
        CoRt_Spawn = StartCoroutine(SpawnMonster_Routine());
        isSpawning = true;
    }

    private void StopSpawn()
    {
        Debug.Log("Stop Spawn");
        StopCoroutine(CoRt_Spawn);
        DataManager.Instance.ResetPlayerData();
        isSpawning = false;
    }

    IEnumerator SpawnMonster_Routine()
    {
        float fTime = 5.0f;
        ulong ulCheck_Score = 5;

        while (true)
        {
            // 플레이어 킬수가 정해진 만큼 킬하면 정지
            if (ulCheck_Score <= DataManager.Instance.PlayerData.ulStageKill)
            {
                GameObject[] Monsters = GameObject.FindGameObjectsWithTag("tag_Enemy");

                foreach(GameObject iter in Monsters)
                {
                    iter.GetComponent<Unit>().Death();
                }


                BossManager.Instance.Spawn(BossName.Mandoo);
                yield break;
            }

            yield return new WaitForSeconds(fTime); // 1초 마다 스폰

            int iRand = Random.Range((int)eMonsterMobType.Mob_Egg_Man, (int)eMonsterMobType.Max);

            RandomSpawn(iRand);

            
        }
    }
}
