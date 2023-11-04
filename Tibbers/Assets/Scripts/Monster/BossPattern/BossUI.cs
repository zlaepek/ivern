using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
    // common
    public TMP_Text bossNameText;
    public Slider bossHpSlider;

    // specific
    public Slider bossFrozenSlider;

    public void SpawnNewBoss(BossName bossName) {
        bossNameText.gameObject.SetActive(true);
        bossNameText.text = bossName.ToString();

        bossHpSlider.gameObject.SetActive(true);

        if (bossName == BossName.Mandoo) {
            bossFrozenSlider.gameObject.SetActive(true);
        }
    }

    public void HideBossUI() {
        bossNameText.gameObject.SetActive(false);
        bossHpSlider.gameObject.SetActive(false);
        bossFrozenSlider.gameObject.SetActive(false);
    }

    public void updateHPSlider(float value) {
        bossHpSlider.value = value;
    }
}
