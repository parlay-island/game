using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;

public class GameManager : MonoBehaviour
{

    [SerializeField] public GameObject gameOverImage;
    [SerializeField] public Text gameOverText;
	[SerializeField] public Timer timerManager;
    [SerializeField] public GameObject player;
    [SerializeField] public GameObject ground;
    [SerializeField] public Text distanceText;
    [SerializeField] public Text finalDistanceText;
    [SerializeField] public AbstractWebRetriever webRetriever;
    
    public static GameManager instance = null;

    private float playerDistance;
    // hardcoded for now for the purpose of mocking game end result
    // TODO: make this value based on login information & mode selection
    private int playerID = 1;
    private int level = 1;
    private string postEndResultContent; 

    void Awake()
    {
    	// make sure GameManager is a singleton
    	if (instance==null) {
    		instance = this; 
    	} else if (instance != this) {
    		Destroy(gameObject);
    	}

    	DontDestroyOnLoad(gameObject);
    }

    private void initGame(float gameTime) {
        gameOverImage.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        finalDistanceText.gameObject.SetActive(false);

        timerManager.initTimer(gameTime);
        distanceText.gameObject.SetActive(true);
    }

    public void setGameTime(float time) {
        initGame(time);
    }

    public void gameOver() {
        showGameOverUIElements();
        hideUIElementsWhenGameOver();
        
        postGameEndResults();
    }

    public string getPostEndResultContent() {
        return postEndResultContent;
    }

    public int getLevel() {
        return level;
    }

    private void showGameOverUIElements() {
        gameOverImage.SetActive(true);
        gameOverText.gameObject.SetActive(true);
        finalDistanceText.gameObject.SetActive(true);
        finalDistanceText.text = "You have travelled " + playerDistance.ToString("0.00") + "m";
    }

    private void hideUIElementsWhenGameOver() {
        distanceText.gameObject.SetActive(false);
        timerManager.hideTimer();
    }

    private async void postGameEndResults() {
        try {
            var postResultResponsePromise = postEndResultOrTimeoutBy(5000);
            var postResultResponse = await postResultResponsePromise;
            handlePostEndResultResponse(postResultResponse);
        }
        catch (Exception exception) {
            Debug.Log(exception);
            Debug.LogWarningFormat("There was an error when making a post request for end results for player" + playerID.ToString());
        }
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

    private async Task<HttpResponseMessage> postEndResultOrTimeoutBy(int timeout) {
        ResultModel endResult = new ResultModel(playerDistance, level);
        var attemptPostEndResult = webRetriever.PostEndResult(endResult, playerID);
         using (var timeoutCancellationTokenSource = new CancellationTokenSource())
        {
            if (await Task.WhenAny(attemptPostEndResult, 
                Task.Delay(timeout, timeoutCancellationTokenSource.Token)) == attemptPostEndResult)
            {
                timeoutCancellationTokenSource.Cancel();
                return await attemptPostEndResult;
            }
            throw new TimeoutException($"Post end results request was not made in {timeout} seconds");
        }
    }

    // Update is called once per frame
    async void Update()
    {
        if (timerManager.isTimeUp()) {
            gameOver();
            enabled = false;
            return;
        } else {
            timerManager.updateTime();

            playerDistance = player.GetComponent<PlayerMovement>().getDistanceTravelled();
            distanceText.text = "Distance: " + playerDistance.ToString("0.00");
        }
    }
}
