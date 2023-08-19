using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string PanelName;

    public void OpenPanel()
    {
        // SceneManager.LoadScene(PanelName, LoadSceneMode.Additive);
    }

    private void SceneChange(string _strSceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(_strSceneName, LoadSceneMode.Additive);
    }

    public void SceneChange_Title()
    {
        SceneChange("Scene_Title");
    }

    public void SceneChange_Stage()
    {
        SceneChange("Scene_Stage");
    }
}
