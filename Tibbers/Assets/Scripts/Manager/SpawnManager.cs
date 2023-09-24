using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject Monster_1;

    private Camera mainCamera;
    private GameObject playerCharacter;

    public enum eSpawnPos
    {
        Top,
        Bottom,
        Left,
        Right,

        Max
    }
    float[] fPositions = new float[4];


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
     
    private void SetCameraPos()
    {
        float fcamHeight = mainCamera.orthographicSize * 2;
        float fcamWidth = fcamHeight * mainCamera.aspect;
        float fHalfHeight = fcamHeight / 2;
        float fHalfWidth = fcamWidth / 2;

        fPositions[(int)eSpawnPos.Top] = playerCharacter.transform.position.y + fHalfHeight;
        fPositions[(int)eSpawnPos.Bottom] = playerCharacter.transform.position.y - fHalfHeight;
        fPositions[(int)eSpawnPos.Left] = playerCharacter.transform.position.x - fHalfWidth;
        fPositions[(int)eSpawnPos.Right] = playerCharacter.transform.position.x + fHalfWidth;
    }

    private void SpawnObject(GameObject _object, Vector2 _vPoint)
    {
        GameObject gameObject = GameObject.Instantiate(_object, _vPoint, Quaternion.identity);

        Debug.Log("\n MonsterSpawn : " + _vPoint.x + ", " + _vPoint.y);
    }
    private void RandomSpawn(GameObject _object)
    {
        // 실수 min , max 모두포함
        // 정수 max 제외
        //Random.Range
        Vector2 vSpawnPoint = default;

        int iRand = Random.Range(0, (int)eSpawnPos.Max);
        switch (iRand)
        {
            case (int)eSpawnPos.Top:
            case (int)eSpawnPos.Bottom:
                {
                    vSpawnPoint.x = Random.Range(fPositions[(int)eSpawnPos.Left], fPositions[(int)eSpawnPos.Right]);
                    vSpawnPoint.y = fPositions[iRand];
                }
                break;
            case (int)eSpawnPos.Left:
            case (int)eSpawnPos.Right:
                {
                    vSpawnPoint.x = fPositions[iRand];
                    vSpawnPoint.y = Random.Range(fPositions[(int)eSpawnPos.Bottom], fPositions[(int)eSpawnPos.Top]);
                }
                break;

            default:
                break;
        }

        SpawnObject(_object, vSpawnPoint);
    }
}
