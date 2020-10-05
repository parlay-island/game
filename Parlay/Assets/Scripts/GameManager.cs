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
    public GameEndRequestHelper gameEndRequestHelper;

    public float playerDistance = 0f;
    // hardcoded for now for the purpose of mocking game end result
    // TODO: make this value based on login information & mode selection
    public int level = 1;
    private int playerID = 1;
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
        hideGameEndElements();

        timerManager.initTimer(gameTime);
        distanceText.gameObject.SetActive(true);
        gameEndRequestHelper = new GameEndRequestHelper(webRetriever);
    }

    private void hideGameEndElements() {
        gameOverImage.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        finalDistanceText.gameObject.SetActive(false);
    }

    public void setGameTime(float time) {
        initGame(time);
    }

    public void gameOver() {
        showGameOverUIElements();
        hideUIElementsWhenGameOver();
        
        sendPostRequestWithGameEndResults();
    }

    public int getLevel() {
        return level;
    }

    private void showGameOverUIElements() {
        gameOverImage.SetActive(true);
        gameOverText.gameObject.SetActive(true);
        finalDistanceText.gameObject.SetActive(true);
        finalDistanceText.text = "You have travelled " + playerDistance.ToString("0.00") + " m";
    }

    private void hideUIElementsWhenGameOver() {
        distanceText.gameObject.SetActive(false);
        timerManager.hideTimer();
    }

    private async void sendPostRequestWithGameEndResults() {
        gameEndRequestHelper.postGameEndResults(playerDistance, level, playerID);
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
