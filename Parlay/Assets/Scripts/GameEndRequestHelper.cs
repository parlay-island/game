using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndRequestHelper
{
	private AbstractWebRetriever webRetriever;
	private const int TIMEOUT = 5000;
	private string postEndResultContent;

    public GameEndRequestHelper(AbstractWebRetriever webRetriever) {
    	this.webRetriever = webRetriever;
    }

    public void postGameEndResults(float playerDistance, int level, int playerID) {
        try {
						ResultModel endResult = new ResultModel(level, playerDistance, playerID);
						webRetriever.PostEndResult(endResult, playerID);
        }
        catch (Exception exception) {
        		Debug.LogWarningFormat("There was an error when making a post request for end results for player" + playerID.ToString());
        }
    }

    public string getPostEndResultContent() {
    	return webRetriever.GetMostRecentPostRequestResult();
    }
}
