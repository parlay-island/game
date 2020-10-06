using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;

public class GameEndRequestHelper 
{
	private AbstractWebRetriever webRetriever;
	private const int TIMEOUT = 5000;
	private string postEndResultContent;

    public GameEndRequestHelper(AbstractWebRetriever webRetriever) {
    	this.webRetriever = webRetriever;
    }

    public async void postGameEndResults(float playerDistance, int level, int playerID) {
        try {
            var postResultResponsePromise = postEndResultOrTimeoutBy(playerDistance, level, playerID);
            var postResultResponse = await postResultResponsePromise;
            handlePostEndResultResponse(postResultResponse);
        }
        catch (Exception exception) {
            Debug.Log(exception);
            Debug.LogWarningFormat("There was an error when making a post request for end results for player" + playerID.ToString());
        }
    }

    public string getPostEndResultContent() {
    	return this.postEndResultContent;
    }

    private void handlePostEndResultResponse(HttpResponseMessage postResultResponse) {
        if (postResultResponse.IsSuccessStatusCode) {
            Debug.Log(postResultResponse.StatusCode);

            if (postResultResponse.Content != null) {
                postEndResultContent = postResultResponse.Content.ReadAsStringAsync().Result;
                Debug.Log(postEndResultContent);
            }
            
        } else {
            Debug.LogFormat("ERROR");
            Debug.Log(postResultResponse.StatusCode);
        }
    }

    private async Task<HttpResponseMessage> postEndResultOrTimeoutBy(float playerDistance, int level, int playerID) {
        ResultModel endResult = new ResultModel(playerDistance, level);
        var attemptPostEndResult = webRetriever.PostEndResult(endResult, playerID);
         using (var timeoutCancellationTokenSource = new CancellationTokenSource())
        {
            if (await Task.WhenAny(attemptPostEndResult, 
                Task.Delay(TIMEOUT, timeoutCancellationTokenSource.Token)) == attemptPostEndResult)
            {
                timeoutCancellationTokenSource.Cancel();
                return await attemptPostEndResult;
            }
            throw new TimeoutException($"Post end results request was not made in {TIMEOUT} seconds");
        }
    }
}
