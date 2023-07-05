using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    #region VARIABLES
    public Slider sfxSlider;
    public Slider bgmSlider;

    public TMP_Dropdown langunageDropdown;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Audio
    public void SetVolumeSFX(float value)
    {
        AudioManager.SetVolumeSFX(value);
    }
    public void SetVolumeBGM(float value)
    {
        AudioManager.SetVolumeBGM(value);
    }
    #endregion

    #region Language
    public void LocaleSelected(/*int index*/)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[langunageDropdown.value];
    }
    #endregion
}
