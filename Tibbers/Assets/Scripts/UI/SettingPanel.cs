using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{

    public Slider sfxSlider;
    public Slider bgmSlider;

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
}
