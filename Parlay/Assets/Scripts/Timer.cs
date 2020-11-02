using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
		public Slider timerSlider = null;
    public Text timerText = null;
    public Text timeLabel = null;

    private float totalTime;
    private float currTime;
    private float timeElapsed;

		public void Start()
		{
			timerSlider = timerSlider ?? gameObject.AddComponent<Slider>();
			timerText = timerText ?? gameObject.AddComponent<Text>();
			timeLabel = timeLabel ?? timerText;
		}

    public void initTimer(float time) {
        SetUp(time);

        activateTimer();
    }

		public void SetUp(float time)
		{
			totalTime = time;
			currTime = time;
			timeElapsed = 0f;
		}

    private void activateTimer() {
				timerSlider.gameObject.SetActive(true);
	      timeLabel.gameObject.SetActive(true);
	      timerSlider.maxValue = totalTime;
	      timerSlider.value = totalTime;

	  		changeTimerLabel(currTime);
    }

    public void hideTimer() {
    	timerSlider.gameObject.SetActive(false);
      timeLabel.gameObject.SetActive(false);
    }

    public bool isTimeUp() {
        return currTime <= 0;
    }

    public float getCurrTime() {
        return currTime;
    }

    public void updateTime() {
        ChangeTimeInUI(-Time.deltaTime);
    }

    public void AddTime(float timeBonus)
    {
        ChangeTimeInUI(timeBonus);
    }

    private void changeTimerLabel(float currTime) {
        int minutes = Mathf.FloorToInt(currTime / 60);
        int seconds = currTime == 0 ? 0 : Mathf.CeilToInt (currTime - minutes * 60);
        string textTime = string.Format("{0:0}:{1:00}", minutes, seconds);

        timerText.text = textTime;
    }

    /**
        For testing purposes because Unity is very finicky with time
    */
    public void mockDecreaseTime(float timeDecrease) {
        currTime = Mathf.Max(0f, currTime - timeDecrease);
        Debug.Log("test is decreasing currTime to" + currTime);
        changeTimerLabel(currTime);
    }

    private void ChangeTimeInUI(float timeChange)
    {
        timeElapsed -= timeChange;
        currTime = Mathf.Max(0f, totalTime - timeElapsed);
        changeTimerLabel(currTime);
        timerSlider.value = currTime;
    }
}
