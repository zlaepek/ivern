using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStickController : MonoBehaviour 
    /*, IDragHandler, IPointerUpHandler, IPointerDownHandler */
{
    private Camera uiCamera; //UI 카메라를 담을 변수
    private Canvas canvas; //캔버스를 담을 변수

    public RectTransform background; // 조이스틱 배경 이미지
    private RectTransform joystick;   // 조이스틱 이미지
    public float joystickRadius = 50f; // 조이스틱 이동 반경

    private Vector3 inputDirection;  // 입력 방향


    public static JoyStickController Instance { get; private set; }

    public Vector2 InputDirection { get { return inputDirection; } }

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
        joystick = this.GetComponent<RectTransform>();

        canvas = GetComponentInParent<Canvas>(); 
        uiCamera = canvas.worldCamera;

    }
    #region OnClickEvents
    //public void OnDrag(PointerEventData eventData)
    //{
    //    //RectTransform 내부의 스크린포인트를 로컬포인트로 변환해 주는 메서드입니다.
    //    //드래그 중인 스크린 좌표 값을 현재 RectTransform의 드래그 중인 좌표값으로 변환 후
    //    //Vector2 값을 localVector변수에 반환합니다. 만약 드래그되어 값이 발생한다면 true이므로
    //    //아래 기능을 실행합니다.

    //    //Vector2 pos;
    //    //if (RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, eventData.pressEventCamera, out pos))
    //    //{
    //    //    //// 조이스틱 위치 조정
    //    //    //joystick.localPosition = Vector2.ClampMagnitude(pos, joystickRadius);

    //    //    //// 입력 방향 설정
    //    //    //inputDirection = (joystick.localPosition / joystickRadius).normalized;
    //    //}
    //    //Debug.Log("Drag : " + eventData.pressEventCamera);

    //    //Debug.Log("background : " + background.position.x + ", " + background.position.y);
    //}

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    //OnDrag(eventData);
    //}

    //public void OnPointerUp(PointerEventData eventData)
    //{
    //    //// 조이스틱을 초기 위치로 되돌립니다.
    //    //joystick.localPosition = Vector2.zero;

    //    //// 조이스틱 입력을 초기화합니다.
    //    //inputDirection = Vector2.zero;

    //    //InGameUIManager.Instance.HideUI_Joystick();
    //}

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        JoystickDrag();
    }

    private void JoystickDrag()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
            case RuntimePlatform.IPhonePlayer:
                {
                    if(Input.touchCount > 0)
                    {
                        //Debug.Log("RuntimePlatform Phone JoystickDrag touch");

                        Touch touch = Input.GetTouch(0);
                        if ((touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)) // 마우스 왼쪽 버튼을 눌렀을 때
                        {
                            //Debug.Log("RuntimePlatform Phone JoystickDrag if in");

                            Vector2 pos;
                            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(background, Input.mousePosition, uiCamera, out pos))
                            {
                                joystick.localPosition = Vector2.ClampMagnitude(pos, joystickRadius);
                                //Debug.Log("pos : " + pos);
                                inputDirection = (joystick.localPosition / joystickRadius).normalized;
                            }
                        }

                        if (touch.phase == TouchPhase.Ended) // 마우스 왼쪽 버튼을 떼었을 때
                        {
                            joystick.localPosition = Vector2.zero;
                            inputDirection = Vector2.zero;
                            InGameUIManager.Instance.HideUI_Joystick();
                            //Debug.Log("GetMouseButtonUp");

                        }
                    }
                }
                break;

            default:
                {
                    if (Input.GetMouseButton(0)) // 마우스 왼쪽 버튼을 눌렀을 때
                    {
                        Vector2 pos;

                        //RectTransform 내부의 스크린포인트를 로컬포인트로 변환해 주는 메서드입니다.
                        //드래그 중인 스크린 좌표 값을 현재 RectTransform의 드래그 중인 좌표값으로 변환 후
                        //Vector2 값을 localVector변수에 반환합니다. 만약 드래그되어 값이 발생한다면 true이므로
                        //아래 기능을 실행합니다.

                        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(background, Input.mousePosition, uiCamera, out pos))
                        {
                            // 조이스틱 위치 조정
                            joystick.localPosition = Vector2.ClampMagnitude(pos, joystickRadius);
                            //Debug.Log("pos : " + pos);
                            // 입력 방향 설정
                            inputDirection = (joystick.localPosition / joystickRadius).normalized;
                        }
                    }

                    if (Input.GetMouseButtonUp(0)) // 마우스 왼쪽 버튼을 떼었을 때
                    {
                        // 조이스틱을 초기 위치로 되돌립니다.
                        joystick.localPosition = Vector2.zero;
                        // 조이스틱 입력을 초기화합니다.
                        inputDirection = Vector2.zero;
                        InGameUIManager.Instance.HideUI_Joystick();
                        //Debug.Log("GetMouseButtonUp");

                    }
                }
                break;
        }
    }
}
