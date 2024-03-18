using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStickController : MonoBehaviour 
    /*, IDragHandler, IPointerUpHandler, IPointerDownHandler */
{
    private Camera uiCamera; //UI ī�޶� ���� ����
    private Canvas canvas; //ĵ������ ���� ����

    public RectTransform background; // ���̽�ƽ ��� �̹���
    private RectTransform joystick;   // ���̽�ƽ �̹���
    public float joystickRadius = 50f; // ���̽�ƽ �̵� �ݰ�

    private Vector3 inputDirection;  // �Է� ����


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
    //    //RectTransform ������ ��ũ������Ʈ�� ��������Ʈ�� ��ȯ�� �ִ� �޼����Դϴ�.
    //    //�巡�� ���� ��ũ�� ��ǥ ���� ���� RectTransform�� �巡�� ���� ��ǥ������ ��ȯ ��
    //    //Vector2 ���� localVector������ ��ȯ�մϴ�. ���� �巡�׵Ǿ� ���� �߻��Ѵٸ� true�̹Ƿ�
    //    //�Ʒ� ����� �����մϴ�.

    //    //Vector2 pos;
    //    //if (RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, eventData.pressEventCamera, out pos))
    //    //{
    //    //    //// ���̽�ƽ ��ġ ����
    //    //    //joystick.localPosition = Vector2.ClampMagnitude(pos, joystickRadius);

    //    //    //// �Է� ���� ����
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
    //    //// ���̽�ƽ�� �ʱ� ��ġ�� �ǵ����ϴ�.
    //    //joystick.localPosition = Vector2.zero;

    //    //// ���̽�ƽ �Է��� �ʱ�ȭ�մϴ�.
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
                        if ((touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)) // ���콺 ���� ��ư�� ������ ��
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

                        if (touch.phase == TouchPhase.Ended) // ���콺 ���� ��ư�� ������ ��
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
                    if (Input.GetMouseButton(0)) // ���콺 ���� ��ư�� ������ ��
                    {
                        Vector2 pos;

                        //RectTransform ������ ��ũ������Ʈ�� ��������Ʈ�� ��ȯ�� �ִ� �޼����Դϴ�.
                        //�巡�� ���� ��ũ�� ��ǥ ���� ���� RectTransform�� �巡�� ���� ��ǥ������ ��ȯ ��
                        //Vector2 ���� localVector������ ��ȯ�մϴ�. ���� �巡�׵Ǿ� ���� �߻��Ѵٸ� true�̹Ƿ�
                        //�Ʒ� ����� �����մϴ�.

                        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(background, Input.mousePosition, uiCamera, out pos))
                        {
                            // ���̽�ƽ ��ġ ����
                            joystick.localPosition = Vector2.ClampMagnitude(pos, joystickRadius);
                            //Debug.Log("pos : " + pos);
                            // �Է� ���� ����
                            inputDirection = (joystick.localPosition / joystickRadius).normalized;
                        }
                    }

                    if (Input.GetMouseButtonUp(0)) // ���콺 ���� ��ư�� ������ ��
                    {
                        // ���̽�ƽ�� �ʱ� ��ġ�� �ǵ����ϴ�.
                        joystick.localPosition = Vector2.zero;
                        // ���̽�ƽ �Է��� �ʱ�ȭ�մϴ�.
                        inputDirection = Vector2.zero;
                        InGameUIManager.Instance.HideUI_Joystick();
                        //Debug.Log("GetMouseButtonUp");

                    }
                }
                break;
        }
    }
}
