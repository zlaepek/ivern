using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    #region VARIABLES
    
    public enum SettingState
    {
        Audio,
        Language,
        AppInfo
    }

    [Header("SettingPanel")]
    public GameObject settingPanel;

    [Header("OptionPanel")]
    public SettingState currentSettingState;

    public GameObject audioSettingPanel;
    public GameObject languageSettingPanel;
    public GameObject appInfoSettingPanel;

    [Header("Audio")]
    public Slider sfxSlider;
    public Slider bgmSlider;

    [Header("Language")]
    public TMP_Dropdown langunageDropdown;
    #endregion

    #region LifeCycle
    void Start()
    {
        // 언어 설정 불러오기
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[GetLocalePref()];
    }
    #endregion

    #region Initialize
    public void SettingPanelOn()
    {
        settingPanel.SetActive(true);
        SettingPanelSelect();
    }
    public void SettingPanelOff()
    {
        settingPanel?.SetActive(false);
        currentSettingState = SettingState.Audio;
    }
    #endregion

    #region Panel Control
    private void AllSettingPanelOff()
    {
        audioSettingPanel.SetActive(false);
        languageSettingPanel.SetActive(false);
        appInfoSettingPanel.SetActive(false);
    }

    private void SettingPanelSelect()
    {
        AllSettingPanelOff();

        switch (currentSettingState)
        {
            case SettingState.Audio:
                audioSettingPanel.SetActive(true);
                break;
            case SettingState.Language:
                languageSettingPanel.SetActive(true);
                break;
            case SettingState.AppInfo:
                appInfoSettingPanel.SetActive(true);
                break;
            default: 
                break;
        }
    }

    public void AudioPanelSelected()
    {
        currentSettingState = SettingState.Audio;
        SettingPanelSelect();
    }
    public void LanguagePanelSelected()
    {
        currentSettingState = SettingState.Language;
        SettingPanelSelect();
    }
    public void AppInfoPanelSelected()
    {
        currentSettingState = SettingState.AppInfo;
        SettingPanelSelect();
    }
    #endregion

    #region Audio
    public void SetVolumeSFX()
    {
        AudioManager.SetVolumeSFX(sfxSlider.value);
    }
    public void SetVolumeBGM()
    {
        AudioManager.SetVolumeBGM(bgmSlider.value);
    }
    public void UIClickTest()
    {
        AudioManager.Play("Pop", AudioManager.MixerTarget.SFX);
    }
    #endregion

    #region Language
    public void LocaleSelected()
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[langunageDropdown.value];
        SetLocalePref(langunageDropdown.value);
        Debug.Log(GetLocalePref());
    }

    private void SetLocalePref(int val)
    {
        PlayerPrefs.SetInt("Language", val);
    }

    private int GetLocalePref()
    {
        int val = PlayerPrefs.GetInt("Language");
        return val;
    }
    #endregion
}
