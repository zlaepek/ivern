using UnityEngine;

public partial class NetworkManager : MonoBehaviour
{
    #region Public Web Request Methods
    public void RequestPostChapterStart(int chapterId)
    {
        WWWForm wwwForm = new WWWForm();
        string url = serverUrl + "/" + chapter + "/start/v1" + "/" + chapterId;
        StartCoroutine(Instance?.RequestPost(url, wwwForm, CallBackPostChapterStart));
    }

    public void CallBackPostChapterStart(string json)
    {
        ChapterStartJson info = ChapterStartJson.FromJSON(json);
        Debug.Log(info.chapter_id);
        Debug.Log(info.created_at);
        Debug.Log(info.updated_at);
    }

    public void RequestPostChapterClear(int chapterId)
    {
        WWWForm wwwForm = new WWWForm();
        string url = serverUrl + "/" + chapter + "/clear/v1" + "/" + chapterId;
        StartCoroutine(Instance?.RequestPost(url, wwwForm));
    }

    public void RequestPostStageStart(int stageId)
    {
        WWWForm wwwForm = new WWWForm();
        string url = serverUrl + "/" + stage + "/start/v1" + "/" + stageId;
        StartCoroutine(Instance?.RequestPost(url, wwwForm));
    }

    public void RequestPostStageClear(int stageId, int coin)
    {
        WWWForm wwwForm = new WWWForm();
        string url = serverUrl + "/" + stage + "/clear/v1" + "/" + stageId + "/" + coin;
        StartCoroutine(Instance?.RequestPost(url, wwwForm));
    }
    #endregion
}

[System.Serializable]
public class ChapterStartJson
{
    public int chapter_id;
    public string created_at;
    public string updated_at;

    public static ChapterStartJson FromJSON(string jsonString)
    {
        return JsonUtility.FromJson<ChapterStartJson>(jsonString);
    }
}

[System.Serializable]
public class ChapterClearJson
{
    public int chapter_id;
    public string created_at;
    public string updated_at;

    public static ChapterClearJson FromJson(string jsonString)
    {
        return JsonUtility.FromJson<ChapterClearJson>(jsonString);
    }
}
