using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class WebRetriever : AbstractWebRetriever
{
    private const int TIMEOUT = 5000;
    private string postRequestResult = "";
    private List<QuestionModel> questions = new List<QuestionModel>();

    [SerializeField] public string apiBaseUrl;

    void Start()
    {
      StartCoroutine(GetRequest(apiBaseUrl + "/questions/"));
    }

    public override List<QuestionModel> GetQuestions()
    {
        return questions;
    }

    IEnumerator GetRequest(string uri)
    {
      UnityWebRequest webRequest = UnityWebRequest.Get(uri);
      webRequest.timeout = TIMEOUT;
      webRequest.SetRequestHeader("Content-Type", "application/json");
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


    public override void PostEndResult(ResultModel result, int playerID)
    {
        // temporary mock URL
        var postURL = "https://httpbin.org/post";

        // will switch to this URL once the apiBaseUrl is working
        // var postURL = apiBaseUrl + "/players/" + playerID.ToString() + "/result";
    	   var json = JsonConvert.SerializeObject(result);
         if (isActiveAndEnabled)
        {
            StartCoroutine(PostRequest(postURL, json, playerID));
        }
        else
        {
            Debug.Log("Web retriever was not active when starting post request coroutine");
        }
    }

    IEnumerator PostRequest(string url, string json, int playerID)
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
              Debug.Log(webRequest.error);
         }
         postRequestResult = webRequest.downloadHandler.text;
     }

     public override string GetMostRecentPostRequestResult()
     {
       return postRequestResult;
     }
}
