using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * This file manages the business logic for the in game timer.
 * Timer UI and helper functions can be modfied here.
 * 
 * @author: Jessica Su, Holly Ansel
 */

public class Timer : MonoBehaviour
{
	private const float timeForColorChanges = 1.5f;
	public Image timer_image = null;
	public Sprite neutral_image = null;
	public Sprite positive_image = null;
	public Sprite negative_image = null;
	public Slider timerSlider = null;
	public Image time_icon = null;
    public Text timerText = null;
    public Text timeLabel = null;

    private float totalTime;
    private float currTime;
    private float timeElapsed;
	private float timeSinceColorChange;
	private bool colorChanged;
	private bool reflectChangesVisually;

	public void Start()
	{
		timerSlider = timerSlider ?? gameObject.AddComponent<Slider>();
		timerText = timerText ?? gameObject.AddComponent<Text>();
		timeLabel = timeLabel ?? timerText;
	}

	void Update()
	{
		if(reflectChangesVisually && colorChanged)
		{
			timeSinceColorChange += Time.deltaTime;
			if(timeSinceColorChange >= timeForColorChanges)
			{
				colorChanged = false;
				timer_image.sprite = neutral_image;
			}
		}
	}

	private void SetUpImage()
	{
		timer_image.sprite = neutral_image;
		colorChanged = false;
		timeSinceColorChange = 0f;
	}

    public void initTimer(float time) {
		SetUpImage();
        SetUp(time);
        activateTimer();
		reflectChangesVisually = true;
    }

	public void SetUp(float time)
	{
		reflectChangesVisually = false;
		totalTime = time;
		currTime = time;
		timeElapsed = 0f;
	}

    private void activateTimer() {
		timerSlider.gameObject.SetActive(true);
        timeLabel.gameObject.SetActive(true);
	    time_icon.gameObject.SetActive(true);
        timerSlider.maxValue = totalTime;
        timerSlider.value = totalTime;
  		changeTimerLabel(currTime);
    }

    public void hideTimer() {
        timerSlider.gameObject.SetActive(false);
        timeLabel.gameObject.SetActive(false);
        time_icon.gameObject.SetActive(false);
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
		if(reflectChangesVisually)
		{
			colorChanged = true;
			timeSinceColorChange = 0f;
			if(timeBonus > 0)
			{
				timer_image.sprite = positive_image;
			}
			else
			{
				timer_image.sprite = negative_image;
			}
		}
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
