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
    private float timeElapsed;

    public void initTimer(float time) {
        totalTime = time;
        currTime = time;
        timeElapsed = 0f;
        
        activateTimer();
    }

    private void activateTimer() {
        timerSlider.gameObject.SetActive(true);
        timeLabel.gameObject.SetActive(true);
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

    public float getCurrTime() {
        return currTime;
    }

    public void updateTime() {
        timeElapsed += Time.deltaTime;
    	currTime = Mathf.Max(0f, totalTime - timeElapsed);

    	int minutes = Mathf.FloorToInt(currTime / 60);
    	int seconds = currTime == 0 ? 0 : Mathf.FloorToInt (currTime - minutes * 60) + 1;
    	string textTime = string.Format("{0:0}:{1:00}", minutes, seconds);
        
        timerText.text = textTime;
        timerSlider.value = currTime;
    }
}
