using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
	public Slider timerSlider;
    public Text timerText;
    public Text timeLabel;

    private float totalTime;
    private float currTime;

    public void initTimer(float time) {
        totalTime = time;
        currTime = time;
        
        timerSlider.maxValue = totalTime;
        timerSlider.value = totalTime;
    }

    public void hideTimer() {
    	timerSlider.gameObject.SetActive(false);
        timeLabel.gameObject.SetActive(false);
    }

    public bool isTimeUp() {
        return currTime == 0;
    }

    public void updateTime() {
    	currTime = Mathf.Max(0f, totalTime - Time.time);
    	int minutes = Mathf.FloorToInt(currTime / 60);
    	int seconds = currTime == 0 ? 0 : Mathf.FloorToInt (currTime - minutes * 60) + 1;
    	string textTime = string.Format("{0:0}:{1:00}", minutes, seconds);
    	
        
        timerText.text = textTime;
        timerSlider.value = currTime;
    }
}
