using System.Collections.Generic;
using UnityEngine;

/**
* This file contains the abstract class for the web retriever.
* The web retriever runs the api requests to fetch and upload question data
* from the backend servers.
* 
* @author: Jake Derry
*/

public abstract class AbstractWebRetriever : MonoBehaviour
{
    public abstract void SetUp(string auth_token, int level);

    public abstract List<QuestionModel> GetQuestions();

    public abstract void PostEndResult(EndResult result, int playerID);

    public abstract string GetMostRecentPostRequestResult();

    public abstract void FetchResults();

    public abstract List<ResultModel> GetMostRecentResults();

    public abstract bool IsLoading();

}
