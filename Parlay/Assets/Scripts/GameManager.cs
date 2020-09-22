using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	private Slider timerSlider;
    private Text timerText;
    private Text timeLabel;
    private float gameTime = 10f;
    private GameObject gameOverImage;
    private Text gameOverText;
	
	public static GameManager instance = null;
    // Start is called before the first frame update
    void Awake()
    {
    	// make sure GameManager is a singleton (only created once)
    	if (instance==null) {
    		instance = this; 
    	} else if (instance != this) {
    		Destroy(gameObject);
    	}

    	DontDestroyOnLoad(gameObject);
    	initGame();
        
    }

    private void initGame() {
        gameOverImage = GameObject.Find("GameOverImage");
        gameOverText = GameObject.Find("GameOverText").GetComponent<Text>();
        gameOverImage.SetActive(false);

        initTimer();
    	
    }

    private void initTimer() {
        timerSlider = GameObject.Find("Timer").GetComponent<Slider>();
        timerText = GameObject.Find("TimerText").GetComponent<Text>();
        timeLabel = GameObject.Find("TimeLabel").GetComponent<Text>();


        timerSlider.maxValue = gameTime;
        timerSlider.value = gameTime;
    }

    private void gameOver() {
    	// show game over screen
        gameOverImage.SetActive(true);
        timerSlider.gameObject.SetActive(false);
        timeLabel.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    	float time = gameTime - Time.time;
    	int minutes = Mathf.FloorToInt(time / 60);
    	int seconds = Mathf.FloorToInt (time - minutes * 60);

    	string textTime = string.Format("{0:0}:{1:00}", minutes, seconds);
    	if (time <= 0) {
            gameOver();
    		return;
    	} 
    	else {
    		timerText.text = textTime;
    		timerSlider.value = time;

    	}
        
    }
}
