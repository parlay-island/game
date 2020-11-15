using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndRequestHelper
{
	private AbstractWebRetriever webRetriever;
	private const int TIMEOUT = 5000;
	private string postEndResultContent;
	private EndResult _mostRecentEndResult;
	private int level;

    public GameEndRequestHelper(AbstractWebRetriever webRetriever, int level) {
    	this.webRetriever = webRetriever;
			this.level = level;
    }

    public void postGameEndResults(float playerDistance, int level, int playerID, List<AnsweredQuestion> answeredQuestions, List<String> awards) {
        try {
	        _mostRecentEndResult = new EndResult(level, playerDistance, answeredQuestions, awards);
	        webRetriever.PostEndResult(_mostRecentEndResult, playerID);
        }
        catch (Exception exception) {
        		Debug.LogWarningFormat("There was an error when making a post request for end results for player" + playerID.ToString());
        }
    }

    public string getPostEndResultContent() {
    	return webRetriever.GetMostRecentPostRequestResult();
    }

    public EndResult getMostRecentEndResult()
    {
	    return _mostRecentEndResult;
    }
}
