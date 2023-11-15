using UnityEngine;

public partial class NetworkManager : MonoBehaviour
{
    [System.Serializable]
    public class ServerJson
    {
        public string message;
        public static ServerJson FromJson(string jsonString)
        {
            return JsonUtility.FromJson<ServerJson>(jsonString);
        }
    }

    private void CheckServerLive()
    {
        string url = serverUrl + "/";
        StartCoroutine(RequestGet(url, "", CheckServerLiveCallBack));
    }

    private void CheckServerLiveCallBack(string json)
    {
        ServerJson info = ServerJson.FromJson(json);
        if (info.message == "Hello World")
        {
            _isServerAlive = true;
        }
    }

}
