using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject gameOverImage;
    public Text gameOverText;
	public Timer timerManager;
    public float gameTime;

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
    	initGame();
    }

    private void initGame() {
        gameOverImage.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        timerManager.initTimer(gameTime);
    }

    private void gameOver() {
        gameOverImage.SetActive(true);
        gameOverText.gameObject.SetActive(true);
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
        }
    }
}
