using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

/**
 * This file contains the business logic that runs the api requests to fetch question data
 * from the backend servers. Question data, upload, and retrieval functionality
 * can all be referenced and executed here.
 * 
 * @author: Holly Ansel, Jake Derry
 */

public class WebRetriever : AbstractWebRetriever
{
    private const int TIMEOUT = 5000;
    private string postRequestResult = "";
    private List<QuestionModel> questions = new List<QuestionModel>();
    private List<ResultModel> results;
    private bool isLoading;
    private string auth_token = "";
    private int level = 1;

    [SerializeField] public string apiBaseUrl;


    void Start()
    {
      GameManager.instance?.SetUpWebRetriever();
    }

    public override void SetUp(string auth_token, int level)
    {
      this.level = level;
      this.auth_token = auth_token;
      StartCoroutine(GetQuestionRequest(apiBaseUrl + "/questions/?level=" + level));
    }

    public override List<QuestionModel> GetQuestions()
    {
        return questions;
    }

    IEnumerator GetQuestionRequest(string uri)
    {
      UnityWebRequest webRequest = UnityWebRequest.Get(uri);
      webRequest.timeout = TIMEOUT;
      webRequest.SetRequestHeader("Content-Type", "application/json");
      webRequest.SetRequestHeader("Authorization", "Token " + auth_token);
      yield return webRequest.SendWebRequest();
      if (webRequest.isNetworkError || webRequest.isHttpError)
      {
            Debug.LogWarningFormat("There was an error when loading questions [{0}]", webRequest.error);
      }
      else
      {
        string getString = webRequest.downloadHandler.text;
        QuestionList questionList = JsonConvert.DeserializeObject<QuestionList>(getString);
        questions = questionList.questions;
      }
    }


    public override void PostEndResult(EndResult result, int playerID)
    {
        var postURL = apiBaseUrl + "/players/" + playerID + "/results/";
        var json = JsonConvert.SerializeObject(result);
        if (isActiveAndEnabled)
        {
          #if !UNITY_EDITOR
            StartCoroutine(PostRequest(postURL, json));
          #endif
        }
        else
        {
            Debug.Log("Web retriever was not active when starting post request coroutine");
        }
    }

    IEnumerator PostRequest(string url, string json)
     {
         var webRequest = new UnityWebRequest(url, "POST");
         byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
         webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
         webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
         webRequest.SetRequestHeader("Content-Type", "application/json");
         webRequest.SetRequestHeader("Authorization", "Token " + auth_token);
         webRequest.timeout = TIMEOUT;

         yield return webRequest.SendWebRequest();

         if (webRequest.isNetworkError || webRequest.isHttpError)
         {
              Debug.Log(webRequest.error);
         }
         postRequestResult = webRequest.downloadHandler.text;
         Debug.Log(postRequestResult);
     }

     public override string GetMostRecentPostRequestResult()
     {
       return postRequestResult;
     }

     public override List<ResultModel> GetMostRecentResults(){
       return results;
     }

     public override void FetchResults()
     {
        string url = apiBaseUrl + "/levels/" + level + "/results/?page=%1";
        StartCoroutine(GetResultRequest(url));
     }

     IEnumerator GetResultRequest(string uri)
     {
       isLoading = true;
       UnityWebRequest webRequest = UnityWebRequest.Get(uri);
       webRequest.timeout = TIMEOUT;
       webRequest.SetRequestHeader("Content-Type", "application/json");
       webRequest.SetRequestHeader("Authorization", "Token " + auth_token);
       yield return webRequest.SendWebRequest();
       if (webRequest.isNetworkError || webRequest.isHttpError)
       {
          Debug.LogWarningFormat("There was an error when loading results [{0}]", webRequest.error);
       }
       else
       {
         string getString = webRequest.downloadHandler.text;
         ResultList resultList = JsonConvert.DeserializeObject<ResultList>(getString);
         results = resultList.results;
       }
       isLoading = false;
     }

     public override bool IsLoading()
     {
       return isLoading;
     }
}
