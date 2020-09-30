using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class WebRetriever : MonoBehaviour
{
    private List<QuestionModel> _questions;
    private readonly HttpClient _httpClient = new HttpClient();

    [SerializeField] public string apiBaseUrl;

    public async Task<List<QuestionModel>> GetQuestions()
    {
        string getString = await _httpClient.GetStringAsync(apiBaseUrl + "/questions");
        QuestionList questionList = JsonConvert.DeserializeObject<QuestionList>(getString);
        return questionList.questions;
    }
}
