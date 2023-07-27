using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaEventList : MonoBehaviour
{
    public List<GachaEvent> gachaEventList;

    public RectTransform container;

    public GameObject buttonPrefab;

    public Sprite eeeeeeee;
    // Start is called before the first frame update
    void Start()
    {
        // TODO: ¼­¹ö¿Í ¿¬µ¿
        gachaEventList = new List<GachaEvent>
        {
            // ÀÓ½Ã Å×½ºÆ® ÄÉÀÌ½º
            new GachaEvent("ÀÏ¹Ý °¡Ã­", System.DateTime.Now, Color.red,  Color.black, eeeeeeee),
            new GachaEvent("ÀÏ¹Ý2 °¡Ã­", System.DateTime.Now, Color.yellow,  Color.black, eeeeeeee),
            new GachaEvent("È®·ü¾÷ °¡Ã­", System.DateTime.Now, Color.green, Color.black, eeeeeeee),
            new GachaEvent("È®·ü¾÷2 °¡Ã­", System.DateTime.Now, Color.blue, Color.black, eeeeeeee),
            new GachaEvent("È®·ü¾÷3 °¡Ã­", System.DateTime.Now, Color.blue, Color.black, eeeeeeee),
            new GachaEvent("È®·ü¾÷4 °¡Ã­", System.DateTime.Now, Color.blue, Color.black, eeeeeeee),
            new GachaEvent("È®·ü¾÷5 °¡Ã­", System.DateTime.Now, Color.blue, Color.black, eeeeeeee),
            new GachaEvent("È®·ü¾÷6 °¡Ã­", System.DateTime.Now, Color.blue, Color.black, eeeeeeee),
            new GachaEvent("È®·ü¾÷7 °¡Ã­", System.DateTime.Now, Color.blue, Color.black, eeeeeeee),
            new GachaEvent("È®·ü¾÷8 °¡Ã­", System.DateTime.Now, Color.blue, Color.black, eeeeeeee)
        };

        renderGachaEventButtons();
    }

    private void renderGachaEventButtons()
    {
        for (int i = 0; i < gachaEventList.Count; i++)
        {
            GameObject buttonObject = Instantiate(buttonPrefab, new Vector3(0, i * 200, 0), Quaternion.identity);
            buttonObject.transform.SetParent(container.transform);
            buttonObject.GetComponentInChildren<TMPro.TMP_Text>().text = gachaEventList[i].GetGachaEventName();
            buttonObject.GetComponentInChildren<TMPro.TMP_Text>().color = gachaEventList[i].GetGachaEventTextColor();
            buttonObject.GetComponent<Image>().color = gachaEventList[i].GetGachaEventBackGroundColor();
        }
    }
}
