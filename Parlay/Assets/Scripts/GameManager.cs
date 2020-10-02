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
    public Text distanceLabel;
    public Text distanceText;
    
    public static GameManager instance = null;

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


        timerManager.initTimer(gameTime);
    }

    public void setGameTime(float time) {
        initGame(time);
    }

    private void gameOver() {
        gameOverImage.SetActive(true);
        gameOverText.gameObject.SetActive(true);
        distanceLabel.gameObject.SetActive(false);
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

            float playerDistance = player.GetComponent<PlayerMovement>().getDistanceTravelled();
            distanceText.text = playerDistance.ToString("0.00");
        }
    }
}
