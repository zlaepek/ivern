using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
   

    private int m_iType;

    private float m_fRadius;

    private SpriteRenderer spriteRenderer;

    private Bounds bounds;

    public float Radius { get { return m_fRadius; } }
    public int Type { get { return m_iType; } }

    public void Init(int _iType)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        bounds = spriteRenderer.bounds;

        m_fRadius = Mathf.Sqrt((bounds.size.x * bounds.size.x) + (bounds.size.y * bounds.size.y));

        m_iType = _iType;
    }

    public void ItemEffect(int _iType)
    {
        switch(_iType)
        {
            case (int)ItemManager.eItemType.EXP_ball:
                {
                    // 뭐 경험치 증가 효과 구현
                    //Debug.Log("Get EXP_ball !");

                    //DataManager.Instance.Get_Exp(Random.Range(0, (int)DataManager.eExpBall_Type.eExpBall_Max));
                    
                    DataManager.Instance.Get_Exp((int)DataManager.eExpBall_Type.D);

                    gameObject.SetActive(false);
                }
                break;

            default:
                {

                }
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Init(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
