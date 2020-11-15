using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/**
 * This file contains the business logic for the Game Manager.
 * Holds business logic to start game by loading up all necessary objects.
 * Holds business logic to reset game by resetting all necessary objects.
 * Ends game when timer runs out of time.
 * Tracks or holds references to global data such as distance traveled,
 * time, questions asnwered, number of retries, etc.
 * 
 * @author: Holly Ansel, Jake Derry, Jessica Su, Andres Montoya
 */

public class GameManager : MonoBehaviour
{

    [SerializeField] public GameObject gameOverImage;
    [SerializeField] public Timer timerManager;
    [SerializeField] public GameObject player;
    [SerializeField] public Text distanceText;
    [SerializeField] public Text finalDistanceText;
    [SerializeField] public AbstractWebRetriever webRetriever;
    [SerializeField] public GameObject enemySpawner;
    [SerializeField] public GameObject questionUI;
    [SerializeField] public GameObject leaderBoard;
    [SerializeField] public GameObject endGameScreen;
    [SerializeField] public GameObject resetButton;
    [SerializeField] public GameObject exitButton;
    [SerializeField] public LevelGenerator levelGenerator;
    [SerializeField] public Text powerUpText;
    [SerializeField] public AwardManager awardManager;

    public static GameManager instance = null;
    public GameEndRequestHelper gameEndRequestHelper;
    public bool canRetry = false;

    public float playerDistance = 0f;
    public float bonusDistance = 0f;
    private bool sentRequest = false;
    public ArrayList retries = new ArrayList();
    public bool playerFallen = false;
    private List<AnsweredQuestion> _answeredQuestions;
    private PlayerAuth _playerAuth;


    void Awake()
    {
    	// make sure GameManager is a singleton
    	if (instance==null) {
    		instance = this;
    	} else if (instance != this) {
    		Destroy(gameObject);
    	}
    	DontDestroyOnLoad(gameObject);
      try
      {
          _playerAuth = GameObject.Find("PlayerInfo")?.GetComponent<PlayerAuth>();
      }
      catch (NullReferenceException e)
      {
          Debug.Log("Encountered null error when finding player info");
      }
    }

    public void SetUpWebRetriever()
    {
      int level = GetLevel();
      string player_auth_token = _playerAuth?.GetAuthToken() ?? "";
      webRetriever.gameObject.SetActive(true);
      webRetriever.SetUp(player_auth_token, level);
    }

    public void exitGame()
    {
        hideGameEndElements();
        GameObject.Destroy(GameObject.Find("LevelObj"));
        StartCoroutine(loadStartScreen());
    }

    IEnumerator loadStartScreen()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string mode_scene_name = "ModeSelection";

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(mode_scene_name, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        yield return SceneManager.UnloadSceneAsync(currentScene);
    }

    private void initGame(float gameTime) {
        hideGameEndElements();
        enemySpawner.SetActive(true);
        timerManager.initTimer(gameTime);
        distanceText.gameObject.SetActive(true);
        gameEndRequestHelper = new GameEndRequestHelper(webRetriever, GetLevel());
        leaderBoard.GetComponent<Leaderboard>().SetGameEndRequestHelper(gameEndRequestHelper);
        leaderBoard.GetComponent<Leaderboard>().SetPlayer(_playerAuth);
        enabled = true;
        _answeredQuestions = new List<AnsweredQuestion>();
    }

    private void hideGameEndElements() {
        endGameScreen.SetActive(false);
        gameOverImage.SetActive(false);
        finalDistanceText.gameObject.SetActive(false);
        leaderBoard.gameObject.SetActive(false);
        resetButton.SetActive(false);
        exitButton.SetActive(false);
    }

    public void Reset(float time)
    {
        playerDistance = 0f;
        bonusDistance = 0f;
        levelGenerator.Reset();
        playerFallen = false;
        initGame(time);
    }

    public void addAnsweredQuestion(AnsweredQuestion question)
    {
        _answeredQuestions.Add(question);
    }

    public List<AnsweredQuestion> getPlayerAnsweredQuestions() {
        return _answeredQuestions;
    }

    public void setGameTime(float time) {
        initGame(time);
    }

    public void gameOver() {
        showGameOverUIElements();
        hideUIElementsWhenGameOver();
        sendPostRequestWithGameEndResults();
    }

    private void showGameOverUIElements() {
        endGameScreen.SetActive(true);
        gameOverImage.SetActive(true);
        resetButton.SetActive(true);
        leaderBoard.gameObject.SetActive(true);
        finalDistanceText.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);
        finalDistanceText.text = "You have travelled " + playerDistance.ToString("0.00") + " m";
    }

    private void hideUIElementsWhenGameOver() {
        Rigidbody2D player_rigidbody = player.GetComponent<Rigidbody2D>();
        player_rigidbody.position = new Vector2(-6, 5);
        player_rigidbody.MovePosition(new Vector2(-6, 5));
        enemySpawner.SetActive(false);
        distanceText.gameObject.SetActive(false);
        questionUI.SetActive(false);
        timerManager.hideTimer();
    }

    private void sendPostRequestWithGameEndResults() {
      int playerID = _playerAuth?.GetId() ?? 1;
      gameEndRequestHelper.postGameEndResults(playerDistance, GetLevel(), playerID, _answeredQuestions, awardManager.top_award);
    }

    private int GetLevel()
    {
      return GameObject.Find("LevelObj")?.GetComponent<Level>()?.GetId() ?? 1;
    }

    void Update()
    {
        if (timerManager.isTimeUp() || playerFallen) {
          if(!sentRequest)
          {
            webRetriever.FetchResults();
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
        return player.GetComponent<PlayerMovement>().getDistanceTravelled();
    }

    public void IncreaseTimeByPowerUp(PowerUp powerUp)
    {
        timerManager.AddTime(powerUp.GetTimeBoost());
        powerUpText.text = "Time boost";
        Invoke("resetText", 2);
    }

    public void IncreaseDistanceByPowerUp(PowerUp powerUp)
    {
        bonusDistance += powerUp.GetDistanceBoost();
        powerUpText.text = "Score Boost";
        Invoke("resetText", 2);
    }

    public void AddRetry(PowerUp powerUp)
    {
        retries.Add(powerUp);
        powerUpText.text = "Found Retry";
        Invoke("resetText", 2);
    }

    private void resetText()
    {
        powerUpText.text = "";
    }

    public bool IsQuestionShown()
    {
      return questionUI.activeSelf;
    }
}
