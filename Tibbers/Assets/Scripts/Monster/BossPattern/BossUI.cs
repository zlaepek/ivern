using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
    #region variables
    // common
    public TMP_Text bossNameText;
    public Slider bossHpSlider;

    // specific
    public Slider bossFrozenSlider;
    #endregion

    #region common
    public void SpawnNewBoss(BossName bossName)
    {
        bossNameText.gameObject.SetActive(true);
        bossNameText.text = bossName.ToString();

        bossHpSlider.gameObject.SetActive(true);

        if (bossName == BossName.Mandoo)
        {
            bossFrozenSlider.gameObject.SetActive(true);
        }
    }

    public void HideBossUI()
    {
        bossNameText.gameObject.SetActive(false);
        bossHpSlider.gameObject.SetActive(false);
        bossFrozenSlider.gameObject.SetActive(false);
    }

    public void UpdateHPSlider(float value)
    {
        bossHpSlider.value = value;
    }

    public void SetMaxHP(float value)
    {
        bossHpSlider.maxValue = value;
        bossHpSlider.value = value;
    }
    #endregion

    #region specific
    public void UpdateFrozenSlider(float value)
    {
        bossFrozenSlider.value = value;
    }

    public void InitFrozenSlider(float minValue, float maxValue)
    {
        bossFrozenSlider.maxValue = maxValue;
        bossFrozenSlider.minValue = minValue;
        bossFrozenSlider.value = maxValue;
    }

    #endregion
}
