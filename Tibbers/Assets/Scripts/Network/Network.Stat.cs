using System;
using System.Collections.Generic;
using UnityEngine;

public partial class NetworkManager : MonoBehaviour
{
    // Get, Update(스탯 클릭), Reset
    // int User_id (아마 기기 고유 번호)
    // string Stat

    string stat = "/stat/v1";

    public void RequestGetStat(Action<string> callBack, int _user_id)
    {
        string url = serverUrl + "/get" + stat;
        string parameters = "/" + _user_id;

        StartCoroutine(Instance?.RequestGet(url + parameters,"", callBack));
    }
    public void RequestUpdateStat(int _user_id, string _stat)
    {
        WWWForm wwwForm = new WWWForm();
        string url = serverUrl + "/update" + stat;
        string parameters = "/" + _user_id + "/" + _stat;

        StartCoroutine(Instance?.RequestPost(url + parameters, wwwForm));
    }

    public void RequestResetStat(int _user_id)
    {
        WWWForm wwwForm = new WWWForm();
        string url = serverUrl + "/update" + stat;
        string parameters = "/" + _user_id;

        StartCoroutine(Instance?.RequestPost(url + parameters, wwwForm));
    }

   

}
