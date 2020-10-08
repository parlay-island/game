using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

public class WebRetriever : AbstractWebRetriever
{
    private List<QuestionModel> _questions;
    private readonly HttpClient _httpClient = new HttpClient();

    [SerializeField] public string apiBaseUrl;

    public override async Task<List<QuestionModel>> GetQuestions()
    {
        string getString = await _httpClient.GetStringAsync(apiBaseUrl + "/questions");
        QuestionList questionList = JsonConvert.DeserializeObject<QuestionList>(getString);
        return questionList.questions;
    }

    public override async Task<HttpResponseMessage> PostEndResult(ResultModel result, int playerID) 
    {
        // temporary mock URL
        var postURL = "https://httpbin.org/post";
    	
        // will switch to this URL once the apiBaseUrl is working
        // var postURL = apiBaseUrl + "/players/" + playerID.ToString() + "/result";
    	var json = JsonConvert.SerializeObject(result);
		var data = new StringContent(json, Encoding.UTF8, "application/json");
		var response = await _httpClient.PostAsync(postURL, data);
		return response;
    }
}
