using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    [SerializeField] public GameObject enemySpawner;
    [SerializeField] public GameObject questionUI;
    [SerializeField] public GameObject leaderBoard;

    public static GameManager instance = null;
    public GameEndRequestHelper gameEndRequestHelper;

    public float playerDistance = 0f;
    private int level = 1;
    private int playerID = 1;
    private string postEndResultContent;
    private bool sentRequest = false;

    void Awake()
    {
    	// make sure GameManager is a singleton
    	if (instance==null) {
    		instance = this;
    	} else if (instance != this) {
    		Destroy(gameObject);
    	}
    	DontDestroyOnLoad(gameObject);
      GameObject levelObj = GameObject.Find("LevelObj");
      level = levelObj != null ? levelObj.GetComponent<Level>().GetId() : 1;
    }

    private void initGame(float gameTime) {
        hideGameEndElements();
        enemySpawner.SetActive(true);
        timerManager.initTimer(gameTime);
        distanceText.gameObject.SetActive(true);
        gameEndRequestHelper = new GameEndRequestHelper(webRetriever);
        enabled = true;
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
        leaderBoard.gameObject.SetActive(true);
        finalDistanceText.gameObject.SetActive(true);
        finalDistanceText.text = "You have travelled " + playerDistance.ToString("0.00") + " m";
    }

    private void hideUIElementsWhenGameOver() {
        enemySpawner.SetActive(false);
        distanceText.gameObject.SetActive(false);
        timerManager.hideTimer();
    }

    private void sendPostRequestWithGameEndResults() {
        gameEndRequestHelper.postGameEndResults(playerDistance, level, playerID);
    }

    void Update()
    {
        if (timerManager.isTimeUp()) {
          if(!sentRequest)
          {
            webRetriever.FetchResults(level);
            sentRequest = true;
          }
          if(!webRetriever.IsLoading())
          {
            gameOver();
            enabled = false;
            return;
          }
        } else {
          timerManager.updateTime();
          playerDistance = player.GetComponent<PlayerMovement>().getDistanceTravelled();
          distanceText.text = "Distance: " + playerDistance.ToString("0.00");
        }
    }

    public void DeductTimeByEnemy(Enemy enemy)
    {
      timerManager.AddTime(enemy.GetTimeReduction());
    }

    public void IncreaseTimeByPowerUp(PowerUp powerUp)
    {
        timerManager.AddTime(powerUp.GetTimeBoost());
    }

    public bool IsQuestionShown()
    {
      return questionUI.activeSelf;
    }
}
