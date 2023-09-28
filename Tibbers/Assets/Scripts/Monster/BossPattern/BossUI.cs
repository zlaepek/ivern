using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
    public TMP_Text bossNameText;
    public Slider bossHpSlider;

    public string bossName;

    void Start()
    {
        bossNameText.text = bossName;
    }

    public void updateHPSlider(float value) {
        bossHpSlider.value = value;
    }
}
