using GooglePlayGames.BasicApi;
using GooglePlayGames;
using UnityEngine;

public class GoogleLogin : MonoBehaviour
{
    public void Start()
    {
        
    }
    /* ���� �α��� ��ư�� Ŭ���ϸ� ����Ǵ� �Լ� */
    public void OnClickGoogleLoginButton()
    {
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate((bool success) =>
        {
            if (success) //�α��ο� �����ϸ�
            {
                Debug.Log(Social.localUser.userName + "�� ȯ���մϴ�");
                Debug.Log("����� ID�� " + Social.localUser.id + "�Դϴ�");
            }
            else //�α��ο� �����ϸ�
            {
                Debug.Log("�α��ο� �����߽��ϴ�");
            }
        });
    }

}
