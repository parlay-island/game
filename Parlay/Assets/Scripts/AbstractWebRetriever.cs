using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractWebRetriever : MonoBehaviour
{
    public abstract List<QuestionModel> GetQuestions();

    public abstract void PostEndResult(EndResult result, int playerID);

    public abstract string GetMostRecentPostRequestResult();

    public abstract void FetchResults(int level);

    public abstract List<ResultModel> GetMostRecentResults();

    public abstract bool IsLoading();

}
