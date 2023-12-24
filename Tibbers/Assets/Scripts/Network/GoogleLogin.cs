using GooglePlayGames.BasicApi;
using GooglePlayGames;
using UnityEngine;

public class GoogleLogin : MonoBehaviour
{
    public void Start()
    {
        
    }
    /* 구글 로그인 버튼을 클릭하면 실행되는 함수 */
    public void OnClickGoogleLoginButton()
    {
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate((bool success) =>
        {
            if (success) //로그인에 성공하면
            {
                Debug.Log(Social.localUser.userName + "님 환영합니다");
                Debug.Log("당신의 ID는 " + Social.localUser.id + "입니다");
            }
            else //로그인에 실패하면
            {
                Debug.Log("로그인에 실패했습니다");
            }
        });
    }

}
