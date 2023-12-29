using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class SceneManager : MonoBehaviour
{
    #region Static SceneManager
    private static SceneManager _instance = null;
    public static SceneManager Instance
    {
        get
        {
            return _instance;

        }
    }
    #endregion

    #region Life Cycle (Initialize)
    private void Awake()
    {
        if (Instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            _CommonScene();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion
    private void _SceneChange(string _strSceneName)
    {
        UnitySceneManager.LoadScene(_strSceneName);
    }

    private void _SceneAdd(string _strSceneName)
    {
        UnitySceneManager.LoadScene(_strSceneName, LoadSceneMode.Additive);
    }
    private AsyncOperation _LoadSceneAsync(string _strSceneName)
    {
        return UnitySceneManager.LoadSceneAsync(_strSceneName);
    }

    #region Each Scene Load Method
    private void _CommonScene()
    {
        _SceneAdd("Scene_Common");
    }

    public void SceneChange_Title()
    {
        _SceneChange("Scene_Title");
    }

    public void SceneChange_Stage()
    {
        _SceneChange("Scene_Stage");
        NetworkManager.Instance?.RequestPostChapterStart(1);
    }
    #endregion
}
