using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public partial class NetworkManager : MonoBehaviour
{

    // #if UNITY_EDITOR
    //public bool isOfflineMode = true;
    private bool isOfflineMode = false;
    // #endif
    #region Url Path
    public const string serverUrl = "http://3.34.48.46:8000";

    public const string chapter = "chapter";
    public const string stage = "stage";
    public const string gacha = "gatcha";


    #endregion

    #region Login Variables
    public string token = "token";
    #endregion

    #region Static NetworkManager
    private static NetworkManager _instance = null;
    public static NetworkManager Instance {
        get 
        {
            if (_isServerAlive)
            {
                return _instance;
            }
            else {
                Debug.Log("오프라인 모드 입니다.");
                return null;
            }
        }
        private set
        {

        }
    }

    private static bool _isServerAlive = false;
    #endregion

    #region Life Cycle (Initialize)
    private void Awake()
    {
        if (Instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            if (!isOfflineMode)
            {
                CheckServerLive();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region Methods Web Request CRUD (Get, Post)
    public IEnumerator RequestGet(string path, string parameters, Action<string> callback, bool sendToken = false)
    {
        string url = path;
        if (!string.IsNullOrWhiteSpace(parameters))
        { 
            url = $"{path}?{parameters}";
        }

        Debug.Log($"request get : {url}");
        UnityWebRequest www = UnityWebRequest.Get(url);
        if (!sendToken || _AddToken(www))
        {
            yield return www.SendWebRequest();

            _RespondMessage(www, callback);
        }
        www.Dispose();
    }

    public IEnumerator RequestPost(string path, WWWForm formData, Action<string> callback = null, bool sendToken = false)
    {
        Debug.Log($"request post : {path}");

        UnityWebRequest www = UnityWebRequest.Post(path, formData);
        if (!sendToken || _AddToken(www))
        {
            yield return www.SendWebRequest();

            _RespondMessage(www, callback);
        }
        www.Dispose();
    }

    public IEnumerator RequestPost(string path, string jsonString, Action<string> callback = null, bool sendToken = false)
    {
        Debug.Log($"request post : {path}");

        UnityWebRequest www = UnityWebRequest.PostWwwForm(path, jsonString);

        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonString);
        www.uploadHandler = new UploadHandlerRaw(jsonToSend);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        if (!sendToken || _AddToken(www))
        {
            yield return www.SendWebRequest();

            _RespondMessage(www, callback);
        }
        www.Dispose();
    }

    public IEnumerator RequestPut(string path, string stringData, Action<string> callback = null, bool sendToken = false) {
        Debug.Log($"request put : {path}");

        UnityWebRequest www = UnityWebRequest.Put(path, stringData);
        if (!sendToken || _AddToken(www)) {
            yield return www.SendWebRequest();

            _RespondMessage(www, callback);
        }
        www.Dispose();
    }

    public IEnumerator RequestDelete(string path, Action<string> callback = null, bool sendToken = false) {
        Debug.Log($"request put : {path}");

        UnityWebRequest www = UnityWebRequest.Delete(path);
        if (!sendToken || _AddToken(www)) {
            yield return www.SendWebRequest();

            _RespondMessage(www, callback);
        }
        www.Dispose();
    }
    #endregion

    #region Private Common Methods
    private void _RespondMessage(UnityWebRequest www, Action<string> callback)
    {
        if (www.result == UnityWebRequest.Result.ConnectionError ||
            www.result == UnityWebRequest.Result.ProtocolError)
        {
            try
            {
                Debug.Log(www.downloadHandler.text);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                Debug.Log(www.downloadHandler.text);

                return;
            }


            try
            {
                callback?.Invoke(string.Empty);
            }
            catch (Exception e)
            {
                Debug.LogError(e + "에러");
            }
        }
        else
        {
            try
            {
                Debug.Log(www.downloadHandler.text);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);

                callback?.Invoke(string.Empty);

                return;
            }
            callback?.Invoke(www.downloadHandler.text);
        }
    }

    private bool _AddToken(UnityWebRequest www)
    {
        if (string.IsNullOrEmpty(token))
        {
            // 오류처리
            return false;
        }
        else
        {
            www.SetRequestHeader("x-access-token", token);
            return true;
        }
    }
    #endregion
}