using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        if (GameManager.instance.retries.Count == 0)
        {
            text.text = "";
            ColorBlock colorvar = button.colors;
            colorvar.normalColor = Color.clear;
            colorvar.highlightedColor = Color.clear;
            colorvar.pressedColor = Color.clear;
            button.colors = colorvar;
            image.color = Color.clear;
        } else if (GameManager.instance.canRetry)
        {
            text.text = "Retry";
            ColorBlock colorvar = button.colors;
            colorvar.normalColor = Color.green;
            colorvar.highlightedColor = Color.green;
            colorvar.pressedColor = Color.green;
            image.color = Color.green;
            button.colors = colorvar;
        } else
        {
            text.text = "Retry";
            ColorBlock colorvar = button.colors;
            colorvar.normalColor = Color.white;
            colorvar.highlightedColor = Color.white;
            colorvar.pressedColor = Color.white;
            image.color = Color.white;
            button.colors = colorvar;
        }
    }
    
    public void changeColor()
    {

    }
}
