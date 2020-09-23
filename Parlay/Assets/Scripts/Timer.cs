using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
	public Slider timerSlider;
    public Text timerText;
    public Text timeLabel;
    public float gameTime = 10f;

    public void initTimer() {
        timerSlider.maxValue = gameTime;
        timerSlider.value = gameTime;
    }

    public void hideTimer() {
    	timerSlider.gameObject.SetActive(false);
        timeLabel.gameObject.SetActive(false);
    }

    public float updateTime() {
    	float time = gameTime - Time.time;
    	int minutes = Mathf.FloorToInt(time / 60);
    	int seconds = Mathf.FloorToInt (time - minutes * 60) + 1;

    	string textTime = string.Format("{0:0}:{1:00}", minutes, seconds);
    	if (time > 0) {
            timerText.text = textTime;
    		timerSlider.value = time;
    	} 
    	
    	return time;
    }
}
