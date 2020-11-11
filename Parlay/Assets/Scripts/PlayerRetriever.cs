using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;

public abstract class AbstractPlayerRetriever : MonoBehaviour
{
  public abstract void LoginPlayer(LoginModel loginModel, System.Action successCallback, System.Action<string> errorCallback, PlayerAuth playerAuth);
  public abstract void CreateAccount(CreateAccountModel createAccountModel, System.Action successCallback, System.Action<string> errorCallback);
  public abstract void LogoutPlayer(System.Action successCallback, System.Action<string> errorCallback, PlayerAuth playerAuth);
}


public class PlayerRetriever : AbstractPlayerRetriever
{
    // TODO: currently using stage url
    [SerializeField] private string apiBaseUrl;

    private const int TIMEOUT = 5000;

    public override void LoginPlayer(LoginModel loginModel, System.Action successCallback, System.Action<string> errorCallback, PlayerAuth playerAuth)
    {
      UnityWebRequest.ClearCookieCache();
      var json = JsonConvert.SerializeObject(loginModel);
      string url = apiBaseUrl + "/auth/token/login/?format=json";
      StartCoroutine(PostLoginRequest(url, json, successCallback, errorCallback, playerAuth));
    }

    public override void LogoutPlayer(System.Action successCallback, System.Action<string> errorCallback, PlayerAuth playerAuth)
    {
        var json = JsonConvert.SerializeObject(playerAuth.GetAuthToken());
        string url = apiBaseUrl + "/auth/token/logout/";
        UnityWebRequest.ClearCookieCache();
        StartCoroutine(PostLogoutRequest(url, json, successCallback, errorCallback, playerAuth));
    }

    IEnumerator PostLoginRequest(string url, string json, System.Action successCallback, System.Action<string> errorCallback, PlayerAuth playerAuth)
     {
         var webRequest = new UnityWebRequest(url, "POST");
         byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
         webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
         webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
         webRequest.SetRequestHeader("Content-Type", "application/json");
         webRequest.timeout = TIMEOUT;

         yield return webRequest.SendWebRequest();
         if (webRequest.isNetworkError || webRequest.isHttpError)
         {
              errorCallback(webRequest.responseCode == 400 ? "username and password combination is invalid" : "a problem occurred when logging in");
              Debug.Log(webRequest.error);
         } else {
           string postRequestResult = webRequest.downloadHandler.text;
           LoginResponseModel login_response = JsonConvert.DeserializeObject<LoginResponseModel>(postRequestResult);
           playerAuth.SetAuthToken(login_response.auth_token);
           UnityWebRequest.ClearCookieCache();
           yield return StartCoroutine(GetPlayer(successCallback, errorCallback, playerAuth));
         }
     }

    IEnumerator PostLogoutRequest(string url, string json, System.Action successCallback, System.Action<string> errorCallback, PlayerAuth playerAuth)
    {
        var webRequest = new UnityWebRequest(url, "POST");
        webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", "Token " + playerAuth.GetAuthToken());
        webRequest.timeout = TIMEOUT;

        yield return webRequest.SendWebRequest();
        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            errorCallback("a problem occurred when logging out");
            Debug.Log(webRequest.error);
        }
        else {
            successCallback();
        }
    }


    IEnumerator GetPlayer(System.Action successCallback, System.Action<string> errorCallback, PlayerAuth playerAuth)
      {
          string url = apiBaseUrl + "/players/me/";
          var webRequest = new UnityWebRequest(url, "GET");
          webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
          webRequest.SetRequestHeader("Content-Type", "application/json");
          webRequest.SetRequestHeader("Authorization", "Token " + "c1ee5b88354ead063b351d71aeea7e2bbb477667");
          webRequest.timeout = TIMEOUT;
          yield return webRequest.SendWebRequest();

          if (webRequest.isNetworkError || webRequest.isHttpError)
          {
              errorCallback("a problem occurred when logging in");
              Debug.Log(webRequest.error);
          } else {
            string postRequestResult = webRequest.downloadHandler.text;
            PlayerModel player_fetched = JsonConvert.DeserializeObject<PlayerModel>(postRequestResult);
            playerAuth.SetPlayer(player_fetched);
            successCallback();
          }

      }

      public override void CreateAccount(CreateAccountModel createAccountModel, System.Action successCallback, System.Action<string> errorCallback)
      {
        var json = JsonConvert.SerializeObject(createAccountModel);
        string url = apiBaseUrl + "/auth/users/?format=json";
        StartCoroutine(PostCreateAccountRequest(url, json, successCallback, errorCallback));
      }

      IEnumerator PostCreateAccountRequest(string url, string json, System.Action successCallback, System.Action<string> errorCallback)
       {
           var webRequest = new UnityWebRequest(url, "POST");
           byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
           webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
           webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
           webRequest.SetRequestHeader("Content-Type", "application/json");
           webRequest.timeout = TIMEOUT;

           yield return webRequest.SendWebRequest();
           if (webRequest.isNetworkError || webRequest.isHttpError)
           {
                errorCallback(webRequest.responseCode == 400 ? "one of the following problems occurred: your password was not strong enough, your username is already taken, or your class code was invalid" : "a problem occurred while creating your account");
                Debug.Log(webRequest.error);
           } else {
             successCallback();
           }
       }

}
