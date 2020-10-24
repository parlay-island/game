using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColor : MonoBehaviour
{
    public Button button;
    public Image image;

    public void changeColor()
    {
        Debug.Log("Changing highlighed color");
        ColorBlock colorVar = button.colors;
        if (colorVar.normalColor != Color.white)
        {
            Debug.Log("Change to white");
            colorVar.normalColor = Color.white;
            colorVar.highlightedColor = Color.white;
            colorVar.pressedColor = Color.white;
            image.color = Color.white;
        } else
        {
            Debug.Log("Change to green");
            colorVar.normalColor = Color.green;
            colorVar.highlightedColor = Color.green;
            colorVar.pressedColor = Color.green;
            image.color = Color.green;
        }
        button.colors = colorVar;

    }
}
