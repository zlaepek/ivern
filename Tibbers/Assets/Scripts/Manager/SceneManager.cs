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
    private void _SceneAdd_Completed(string _strSceneName, System.Action<AsyncOperation> _OnSceneLoaded)
    {
        UnitySceneManager.LoadSceneAsync(_strSceneName, LoadSceneMode.Additive).completed += _OnSceneLoaded;
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

    public void SceneChange_Stage_Normal()
    {
        StageManager.Instance.SetCurrentStage(0, 0);
        _SceneChange("Scene_Stage");
        // Boss
        _SceneAdd_Completed("UI_Scene_Stage", OnSceneLoaded_UI_Stage);
        // Setting
        _SceneAdd("UI_Scene_Setting");
        NetworkManager.Instance?.RequestPostChapterStart(UserDataManager.Instance.User_ID,1);
        NetworkManager.Instance?.RequestPostStageStart(UserDataManager.Instance.User_ID,1,1);

        //OnSceneLoaded_UI_Stage();
    }

    public void SceneChange_Stage_Normal_2()
    {
        StageManager.Instance.SetCurrentStage(0, 1);
        _SceneChange("Scene_Stage");
        // Boss
        _SceneAdd_Completed("UI_Scene_Stage", OnSceneLoaded_UI_Stage);
        // Setting
        _SceneAdd("UI_Scene_Setting");
        NetworkManager.Instance?.RequestPostChapterStart(UserDataManager.Instance.User_ID, 1);
        NetworkManager.Instance?.RequestPostStageStart(UserDataManager.Instance.User_ID, 1, 2);

        //OnSceneLoaded_UI_Stage();
    }

    public void SceneChange_Stage_Boss()
    {
        StageManager.Instance.SetCurrentStage(0, 2);
        _SceneChange("Scene_Stage");
        // Boss
        _SceneAdd_Completed("UI_Scene_Stage", OnSceneLoaded_UI_Stage);
        // Setting
        _SceneAdd("UI_Scene_Setting");
        NetworkManager.Instance?.RequestPostChapterStart(UserDataManager.Instance.User_ID, 1);
        NetworkManager.Instance?.RequestPostStageStart(UserDataManager.Instance.User_ID, 1, 3);
    }

    public bool IsActiveScene(string _SceneName)
    {
        Scene loadedScene = UnitySceneManager.GetSceneByName(_SceneName);

        return loadedScene.IsValid() ? true : false;
    }
    private void OnSceneLoaded_UI_Stage(AsyncOperation asyncOperation)
    {
        if (StageManager.Instance.IsBossStage())
        {
            // 보스 스테이지 일때의 씬로드 완료 후 처리
            TimeManager.Instance.HideTime();
        }
        else
        {
            // 일반 스테이지 일때의 씬로드 완료 후 처리
            TimeManager.Instance.OpenTime();
        }

        StageManager.Instance.Setting_UI_Stage_Scene();
    }

    public void GameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        GameObject.Find("Btn_GameExit").SetActive(false);
#else
        //GameObject.Find("Btn_GameExit").SetActive(false);

        Application.Quit();

        Application.CancelQuit();
        System.Diagnostics.Process.GetCurrentProcess().Kill();
#endif
    }
    #endregion
}
