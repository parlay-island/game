using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class Award : MonoBehaviour
{
    [SerializeField] public GameObject awardUI;
    [SerializeField] public TextMeshProUGUI text;
    [SerializeField] public GameObject bronzeMedal;
    [SerializeField] public GameObject silverMedal;
    [SerializeField] public GameObject goldMedal;
    public string awardName;

    public void DisplayChosenAward(GameObject award)
    {
        text.text = GetAwardMessage();
        awardUI.SetActive(true);
        award.SetActive(true);
        Invoke("hideAward", 5);
    }

    //private void hideAward()
    //{
    //    bronzeMedal.SetActive(false);
    //    silverMedal.SetActive(false);
    //    goldMedal.SetActive(false);
    //    awardUI.SetActive(false);
    //    text.text = "";
    //}

    public abstract void hideAward();
    public abstract void DisplayAward();
    public abstract bool WinsAward();
    public abstract string GetAwardMessage();
}
