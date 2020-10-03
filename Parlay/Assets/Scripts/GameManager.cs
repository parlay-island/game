using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject gameOverImage;
    public Text gameOverText;
	public Timer timerManager;
    public GameObject player;
    public GameObject ground;
    public Text distanceText;
    public Text finalDistanceText;
    
    public static GameManager instance = null;

    private float playerDistance;

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

    private void gameOver() {
        gameOverImage.SetActive(true);
        gameOverText.gameObject.SetActive(true);
        finalDistanceText.gameObject.SetActive(true);
        finalDistanceText.text = "You have travelled " + playerDistance.ToString("0.00") + "m";

        distanceText.gameObject.SetActive(false);
        timerManager.hideTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (timerManager.isTimeUp()) {
            gameOver();
            return;
        } else {
            timerManager.updateTime();

            playerDistance = player.GetComponent<PlayerMovement>().getDistanceTravelled();
            distanceText.text = "Distance: " + playerDistance.ToString("0.00");
        }
    }
}
