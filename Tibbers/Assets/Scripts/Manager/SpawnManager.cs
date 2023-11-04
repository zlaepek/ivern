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

    public enum eCameraEdgePos
    {
        Top,
        Bottom,
        Left,
        Right,

        Max
    }
    float[] fPositions = new float[4];

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
        GameObject Camera = GameObject.FindGameObjectWithTag("MainCamera");

        mainCamera = Camera.GetComponent<Camera>();

        playerCharacter = GameObject.FindGameObjectWithTag("tag_Player");
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

    private void SpawnObject(GameObject _object, Vector2 _vPoint)
    {
        

        GameObject gameObject = GameObject.Instantiate(_object, _vPoint, Quaternion.identity);
        _object.GetComponent<Mon_Mob>().SerialNumber = iSerial;
        ++iSerial;
        Debug.Log("\n MonsterSpawn : " + _vPoint.x + ", " + _vPoint.y);
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
