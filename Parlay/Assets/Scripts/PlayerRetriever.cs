using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;

public abstract class AbstractPlayerRetriever : MonoBehaviour
{
  public abstract void LoginPlayer(LoginModel loginModel, System.Action successCallback, System.Action<string> errorCallback, Player player);
    public abstract void LogoutPlayer(System.Action successCallback, System.Action<string> errorCallback, Player player);
}


public class PlayerRetriever : AbstractPlayerRetriever
{
    // TODO: currently using stage url
    [SerializeField] private string apiBaseUrl;

    private const int TIMEOUT = 5000;

    public override void LoginPlayer(LoginModel loginModel, System.Action successCallback, System.Action<string> errorCallback, Player player)
    {
      var json = JsonConvert.SerializeObject(loginModel);
      string url = apiBaseUrl + "/auth/token/login/?format=json";
      StartCoroutine(PostLoginRequest(url, json, successCallback, errorCallback, player));
    }

    public override void LogoutPlayer(System.Action successCallback, System.Action<string> errorCallback, Player player)
    {
        var json = JsonConvert.SerializeObject(player.GetAuthToken());
        string url = apiBaseUrl + "/auth/token/logout/?format=json";
        StartCoroutine(PostLogoutRequest(url, json, successCallback, errorCallback, player));
    }

    IEnumerator PostLoginRequest(string url, string json, System.Action successCallback, System.Action<string> errorCallback, Player player)
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
             // unity editor doesn't close the session until expires
             // this is a work around because developers don't need to login when playing the game
              #if UNITY_EDITOR
              if(webRequest.responseCode == 403)
              {
                 successCallback();
                 yield break;
              }
              #endif
              errorCallback(webRequest.responseCode == 400 ? "username and password combination is invalid" : "a problem occurred when logging in");
              Debug.Log(webRequest.error);
         } else {
           string postRequestResult = webRequest.downloadHandler.text;
           LoginResponseModel login_response = JsonConvert.DeserializeObject<LoginResponseModel>(postRequestResult);
           player.SetAuthToken(login_response.auth_token);
           yield return StartCoroutine(GetPlayer(successCallback, errorCallback, player));
         }
     }

    IEnumerator PostLogoutRequest(string url, string json, System.Action successCallback, System.Action<string> errorCallback, Player player)
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
            // unity editor doesn't close the session until expires
            // this is a work around because developers don't need to login when playing the game
            #if UNITY_EDITOR
            if (webRequest.responseCode == 403)
            {
                successCallback();
                yield break;
            }
            #endif
            errorCallback("a problem occurred when logging out");
            Debug.Log(webRequest.error);
        }
        else
        {
            print("logout success!");
        }
    }


    IEnumerator GetPlayer(System.Action successCallback, System.Action<string> errorCallback, Player player)
      {
          string url = apiBaseUrl + "/players/me/";
          var webRequest = new UnityWebRequest(url, "GET");
          webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
          webRequest.SetRequestHeader("Content-Type", "application/json");
          webRequest.SetRequestHeader("Authorization", "Token " + player.GetAuthToken());
          webRequest.timeout = TIMEOUT;
          yield return webRequest.SendWebRequest();

          if (webRequest.isNetworkError || webRequest.isHttpError)
          {
              errorCallback("a problem occurred when logging in");
              Debug.Log(webRequest.error);
          } else {
            string postRequestResult = webRequest.downloadHandler.text;
            PlayerModel player_fetched = JsonConvert.DeserializeObject<PlayerModel>(postRequestResult);
            player.SetPlayer(player_fetched);
            successCallback();
          }

      }
}
