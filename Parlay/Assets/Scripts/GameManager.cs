﻿using System;
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
    [SerializeField] public GameObject endGameScreen;

    public static GameManager instance = null;
    public GameEndRequestHelper gameEndRequestHelper;
    public bool canRetry = false;

    public float playerDistance = 0f;
    private int level = 1;
    public float bonusDistance = 0f;
    private int playerID = 1;
    private string postEndResultContent;
    private bool sentRequest = false;
    public ArrayList retries = new ArrayList();
    public bool playerFallen = false;

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
        endGameScreen.SetActive(false);
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
        endGameScreen.SetActive(true);
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
        if (timerManager.isTimeUp() || playerFallen) {
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
          playerDistance = Mathf.Max(0, player.GetComponent<PlayerMovement>().getDistanceTravelled() + bonusDistance);
          distanceText.text = "Distance: " + playerDistance.ToString("0.00");
        }
    }

    public void DeductTimeByEnemy(Enemy enemy)
    {
      timerManager.AddTime(enemy.GetTimeReduction());
    }

    public float DistanceTraveledByPlayer()
    {
        print(playerDistance);
        print(player.GetComponent<PlayerMovement>().getDistanceTravelled());
        return player.GetComponent<PlayerMovement>().getDistanceTravelled();
    }

    public void IncreaseTimeByPowerUp(PowerUp powerUp)
    {
        timerManager.AddTime(powerUp.GetTimeBoost());
    }

    public void IncreaseDistanceByPowerUp(PowerUp powerUp)
    {
        bonusDistance += powerUp.GetDistanceBoost();
        print(bonusDistance);
    }

    public void AddRetry(PowerUp powerUp)
    {
        retries.Add(powerUp);
    }

    public bool IsQuestionShown()
    {
      return questionUI.activeSelf;
    }
}
