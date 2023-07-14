using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaSelectList : MonoBehaviour
{
    public List<GachaOption> gachaOptionList;

    public RectTransform container;

    public GameObject buttonPrefab;

    public Sprite eeeeeeee;
    // Start is called before the first frame update
    void Start()
    {
        // TODO: ¼­¹ö¿Í ¿¬µ¿
        gachaOptionList = new List<GachaOption>
        {
            // ÀÓ½Ã Å×½ºÆ® ÄÉÀÌ½º
            new GachaOption("ÀÏ¹Ý °¡Ã­", false, Color.red, Color.yellow, Color.black, eeeeeeee),
            new GachaOption("ÀÏ¹Ý2 °¡Ã­", false, Color.yellow, Color.yellow, Color.black, eeeeeeee),
            new GachaOption("È®·ü¾÷ °¡Ã­", true, Color.green, Color.gray, Color.black, eeeeeeee),
            new GachaOption("È®·ü¾÷2 °¡Ã­", true, Color.blue, Color.cyan, Color.black, eeeeeeee),
            new GachaOption("È®·ü¾÷3 °¡Ã­", true, Color.blue, Color.cyan, Color.black, eeeeeeee),
            new GachaOption("È®·ü¾÷4 °¡Ã­", true, Color.blue, Color.cyan, Color.black, eeeeeeee),
            new GachaOption("È®·ü¾÷5 °¡Ã­", true, Color.blue, Color.cyan, Color.black, eeeeeeee),
            new GachaOption("È®·ü¾÷6 °¡Ã­", true, Color.blue, Color.cyan, Color.black, eeeeeeee),
            new GachaOption("È®·ü¾÷7 °¡Ã­", true, Color.blue, Color.cyan, Color.black, eeeeeeee),
            new GachaOption("È®·ü¾÷8 °¡Ã­", true, Color.blue, Color.cyan, Color.black, eeeeeeee)
        };

        renderGachaEventButtons();
    }

    private void renderGachaEventButtons()
    {
        for (int i = 0; i < gachaOptionList.Count; i++)
        {
            GameObject buttonObject = Instantiate(buttonPrefab, new Vector3(0, i * 200, 0), Quaternion.identity);
            buttonObject.transform.SetParent(container.transform);
            buttonObject.GetComponentInChildren<TMPro.TMP_Text>().text = gachaOptionList[i].GetGachaOption();
            buttonObject.GetComponentInChildren<TMPro.TMP_Text>().color = gachaOptionList[i].GetGachaTextColor();
            buttonObject.GetComponent<Image>().color = gachaOptionList[i].GetGachaBackGround();
            // RectTransform buttonTransform = buttonObject.GetComponent<RectTransform>();
            // gachaOption;
            // ¹öÆ° À§Ä¡ ¹× Å©±â ¼³Á¤
            // buttonTransform.anchoredPosition = new Vector2(0, -i * (buttonPrefab.transform.x + spacing));
            // buttonTransform.sizeDelta = new Vector2(container.sizeDelta.x, buttonHeight);

            // ¹öÆ° ÅØ½ºÆ® ¼³Á¤ (¿¹½Ã)
            // Text buttonText = buttonObj.GetComponentInChildren<Text>();
            // buttonText.text = "Button " + i;
        }
    }
}
