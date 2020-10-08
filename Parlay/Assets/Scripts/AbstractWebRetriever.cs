using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Net.Http;

public abstract class AbstractWebRetriever : MonoBehaviour
{
    public abstract Task<List<QuestionModel>> GetQuestions();

    public abstract Task<HttpResponseMessage> PostEndResult(ResultModel result, int playerID);
}
