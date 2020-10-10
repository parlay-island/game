using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractWebRetriever : MonoBehaviour
{
    public abstract List<QuestionModel> GetQuestions();

    public abstract void PostEndResult(ResultModel result, int playerID);

    public abstract string GetMostRecentPostRequestResult();
}
