using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAreaManager : MonoBehaviour
{
    public static EffectAreaManager instance = null;
    public enum eEffectAreaType 
    {
        Fire,
        Ice
    }

    public GameObject iceArea = null;
    public GameObject fireArea = null;

    #region Life Cycle

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

}
