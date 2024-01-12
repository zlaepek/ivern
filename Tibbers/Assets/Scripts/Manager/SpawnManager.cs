using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    static int iSerial = 0;
    public static SpawnManager Instance { get; private set; }
    public GameObject Monster_1;

    private Camera mainCamera;
    private GameObject playerCharacter;
    private List<GameObject> MonsterPool;

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

    }

    // Update is called once per frame
    void Update()
    {
        SetCameraPos();


        if (Input.GetKeyDown(KeyCode.T))
        {
            RandomSpawn(Monster_1);
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

    private void SpawnObject(GameObject _object, Vector2 _vPoint)
    {
        GameObject Monster = FindNoActiveMonster();

        if( !Monster)
        {
            Monster = GameObject.Instantiate(_object, _vPoint, Quaternion.identity);
            AddPool(Monster);
        }
        else
        {
            Monster.SetActive(true);
            Monster.transform.position = _vPoint;
        }
        InGameUIManager.Instance.SetHpBar(Monster);
        Debug.Log("\n MonsterSpawn : " + _vPoint.x + ", " + _vPoint.y);
        //InGameUIManager.Instance.fPos_Y
        Mon_Mob Temp_Mob = Monster.GetComponent<Mon_Mob>();
        Temp_Mob.Init(1);

        //Debug.Log("\n MonsterSpawn : " + _vPoint.x + ", " + _vPoint.y);
    }
    private void AddPool(GameObject _Monster)
    {
        MonsterPool.Add(_Monster);
        _Monster.GetComponent<Mon_Mob>().SerialNumber = iSerial;
        ++iSerial;
    }
    private void RandomSpawn(GameObject _object)
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

        SpawnObject(_object, vSpawnPoint);
    }
}
