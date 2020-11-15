using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/**
* This file holds the abstract class for an award object.
* Holds business logic to display specified award.
* 
* @author: Andres Montoya, Jake Derry
*/

public abstract class Award : MonoBehaviour
{
    [SerializeField] public GameObject awardUI;
    [SerializeField] public TextMeshProUGUI text;
    public string awardName;

    public void DisplayChosenAward(GameObject award)
    {
        text.text = GetAwardMessage();
        awardUI.SetActive(true);
        award.SetActive(true);
        Invoke("hideAward", 5);
    }

    public abstract void hideAward();
    public abstract void DisplayAward();
    public abstract bool WinsAward();
    public abstract string GetAwardMessage();
}
