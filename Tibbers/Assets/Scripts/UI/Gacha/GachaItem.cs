using TMPro;
using UnityEngine;

public class GachaItem : MonoBehaviour
{
    public TMP_Text type = null;
    public TMP_Text part = null;

    public void SetTypeText(string text)
    {
        type.text = text;
    }

    public void SetPartText(string text)
    {
        part.text = text;
    }
}
