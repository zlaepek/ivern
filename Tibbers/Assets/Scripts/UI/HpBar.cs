using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// UI 에 쓰이는 HP Bar 
public class HpBar : MonoBehaviour
{
    [SerializeField]
    public float Test_Position_Y = 0f;

    private GameObject TargetObject;
    private Unit TargetUnit;
    private Image BarImage;

    public void SetTarget(GameObject _Target)
    {
        TargetObject = _Target;
        TargetUnit = _Target.GetComponent<Unit>();

        BarImage = GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!TargetObject.activeSelf)
        {
            gameObject.SetActive(TargetObject.activeSelf);
            return;
        }

        MoveHpBarPos();
        HpBar_Proc();
    }

    private void MoveHpBarPos()
    {
        //Vector3 screenPos = Camera.main.WorldToScreenPoint(monsterHead.position + Vector3.up * 2f); // 몬스터 머리에서 약간 위로 올려줌
        //hpBarUI.position = screenPos;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(TargetObject.transform.position + Vector3.up * Test_Position_Y); // 몬스터 머리에서 약간 위로 올려줌
        transform.position = screenPos;
    }

    private void HpBar_Proc()
    {
        

        float fAmount = TargetUnit.m_stStat.fHp_Cur / TargetUnit.m_stStat.fHp_Max;
        BarImage.fillAmount = fAmount;
    }
}
