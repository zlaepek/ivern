using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Play("Pop", AudioManager.MixerTarget.SFX);
        AudioManager.Play("Demo", AudioManager.MixerTarget.BGM);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
