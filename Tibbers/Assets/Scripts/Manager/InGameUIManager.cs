using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUIManager : MonoBehaviour
{
    public GameObject oBackground; // 조이스틱 배경 오브젝트

    public GameObject UI_HpBar;
    public static InGameUIManager Instance { get; private set; }

    private List<GameObject> HpBarPool;

    private void Awake()
    {
        HpBarPool = new List<GameObject>();

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

    public void HideUI_Joystick()
    {
        oBackground.SetActive(false);
    }

    public void SetHpBar(GameObject _Object)
    {
        GameObject HpBar = FindNoActiveHpBar();

        if (!HpBar)
        {
            HpBar = GameObject.Instantiate(UI_HpBar, Vector3.zero, Quaternion.identity);
            AddPool(HpBar);
        }
        else
        {
            HpBar.SetActive(true);
        }
        HpBar Temp_UI = HpBar.GetComponent<HpBar>();
        Temp_UI.SetTarget(_Object);

        if(_Object.CompareTag("tag_Player"))
        {
            Temp_UI.SetPosY(1.0f);
        }
        else if(_Object.CompareTag("tag_Enemy"))
        {
            Temp_UI.SetPosY(1.5f);
        }
        else
        {
            Temp_UI.SetPosY(1.5f);
        }

        System.String strName = "HP_Bar";
        HpBar.transform.SetParent(FindParent(strName).transform);
    }

    private void AddPool(GameObject _Object)
    {
        HpBarPool.Add(_Object);
    }

    private GameObject FindNoActiveHpBar()
    {
        foreach (GameObject it in HpBarPool)
        {
            if (it.activeSelf == false)
            {
                return it;
            }
        }
        return null;
    }

    private GameObject FindParent(string _Name)
    {
        GameObject root = GameObject.Find("Canvas");
        GameObject Parent = GameObject.Find(_Name);
        if (Parent == null)
        {
            Parent = new GameObject { name = _Name };
        }
        Parent.transform.SetParent(root.transform);

        return Parent;
    }


    private void MoveUI()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
            case RuntimePlatform.IPhonePlayer:
                {
                    if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        if (oBackground != null)
                        {
                            oBackground.SetActive(true);
                        }

                        // 터치된 위치를 가져옵니다.
                        Vector2 touchPosition = Input.GetTouch(0).position;

                        // 터치된 위치를 World 좌표로 변환합니다.
                        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(touchPosition);
                        worldPosition.z = 0f; // UI가 카메라 앞에 위치하므로 z 축을 조정합니다.

                        // UI를 터치된 위치로 이동시킵니다.
                        oBackground.GetComponent<RectTransform>().position = worldPosition;
                    }
                }
                break;
            default:
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (oBackground != null)
                        {
                            oBackground.SetActive(true);
                        }

                        ///////////////
                        oBackground.GetComponent<RectTransform>().position = Input.mousePosition;
                    }
                }
                break;
        }


    }
    // Start is called before the first frame update
    void Start()
    {
        HideUI_Joystick();
    }

    // Update is called once per frame
    void Update()
    {
        MoveUI();
    }
}
