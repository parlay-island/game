using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

public class WebRetriever : MonoBehaviour
{
    private List<Question> _questions;
    private readonly HttpClient _httpClient = new HttpClient();

        [SerializeField] public string apiBaseUrl;

        public async Task<List<Question>> GetQuestions()
    {
        string getString = await _httpClient.GetStringAsync(apiBaseUrl + "/questions");
        _questions = new List<Question>
        {
            new Question(getString, new List<string> {"0", "1", "2", "3"}, 0)
        };
        return _questions;
    }
}
