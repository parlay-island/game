using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
* This file contains the business logic to change the color of the retry button
* when it is clicked and then hide it when it is not active.
* When button is active and selected: Green
* When button is active and not selected: White
* When button is not active: Clear
* 
* @author: Andres Montoya
*/

public class ChangeColor : MonoBehaviour
{
    public Button button;
    public Image image;
    public TextMeshProUGUI text;
    public GameObject self;
    private ColorBlock currentColor;

    public void Awake()
    {
        currentColor.normalColor = Color.clear;
        currentColor.highlightedColor = Color.clear;
        currentColor.pressedColor = Color.clear;
    }

    public void Update()
    {
        Color selectedColor;
        if (GameManager.instance.retries.Count == 0)
        {
            text.text = "";
            selectedColor = Color.clear;
        } else if (GameManager.instance.canRetry)
        {
            text.text = "Retry";
            selectedColor = Color.green;
        } else
        {
            text.text = "Retry";
            selectedColor = Color.white;
        }
        ColorBlock colorvar = button.colors;
        colorvar.normalColor = selectedColor;
        colorvar.highlightedColor = selectedColor;
        colorvar.pressedColor = selectedColor;
        button.colors = colorvar;
        image.color = selectedColor;
    }
}
