using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUIManager : MonoBehaviour
{
    private Camera uiCamera; //UI 카메라를 담을 변수
    private Canvas canvas; //캔버스를 담을 변수

    private GameObject oBackground; // 조이스틱 배경 오브젝트

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

        GameObject root = GameObject.Find("Canvas");
        canvas = root.GetComponent<Canvas>();
        uiCamera = canvas.worldCamera;

        //Joystick_BackGround
        oBackground = GameObject.Find("Joystick_BackGround");
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
                    if(Input.touchCount > 0)
                    {
                        //Debug.Log("RuntimePlatform Phone touch");
                        Touch touch = Input.GetTouch(0);
                        //if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                        if (touch.phase == TouchPhase.Began)
                        {
                            //Debug.Log("RuntimePlatform Phone if in");
                            //if (oBackground != null)
                            {
                                oBackground.SetActive(true);
                            }

                            // 터치된 위치를 가져옵니다.
                            Vector2 touchPosition = Input.mousePosition;

                            oBackground.GetComponent<RectTransform>().position = touchPosition;

                            //Debug.Log("Move UI mousePosition : " + touchPosition);

                        }
                    }
                }
                break;
            default:
                {

                    if (Input.GetMouseButtonDown(0))
                    {
                        //Debug.Log("RuntimePlatform PC");

                        //if (oBackground != null)
                        {
                            oBackground.SetActive(true);
                        }

                        ///////////////
                        oBackground.GetComponent<RectTransform>().position = Input.mousePosition;
                        //Debug.Log("Move UI mousePosition : " + Input.mousePosition);
                        //Debug.Log("x : " + Input.mousePosition.x + ", y : " + Input.mousePosition.y + ", z : " + Input.mousePosition.z);
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
