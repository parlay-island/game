using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private GameObject gameOverImage;
    private Text gameOverText;
	private Timer timerManager;

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
        timerManager = GetComponent<Timer>();
    	initGame();
    }

    private void initGame() {
        gameOverImage = GameObject.Find("GameOverImage");
        gameOverText = GameObject.Find("GameOverText").GetComponent<Text>();
        gameOverImage.SetActive(false);

        timerManager.initTimer();
    }

    private void gameOver() {
        gameOverImage.SetActive(true);
        timerManager.hideTimer();
    }

    // Update is called once per frame
    void Update()
    {
        float currTime = timerManager.updateTime();
        if (currTime <= 0) {
            gameOver();
            return;
        }
        
    }
}
