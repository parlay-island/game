using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class Award : MonoBehaviour
{
    [SerializeField] public GameObject awardUI;
    [SerializeField] public TextMeshProUGUI text;
    [SerializeField] public int awardMultiplier;
    [SerializeField] public GameObject bronzeMedal;
    [SerializeField] public GameObject silverMedal;
    [SerializeField] public GameObject goldMedal;

    void Start()
    {
        awardUI.SetActive(false);
        bronzeMedal.SetActive(false);
        silverMedal.SetActive(false);
        goldMedal.SetActive(false);
        awardMultiplier = 1;
    }

    public void DisplayAward()
    {
        if (WinsAward(awardMultiplier * 30))
        {
            text.text = GetAwardMessage();
            awardUI.SetActive(true);
            bronzeMedal.SetActive(false);
            silverMedal.SetActive(false);
            goldMedal.SetActive(true);
            Invoke("hideAward", 5);
        } else if (WinsAward(awardMultiplier * 20))
        {
            text.text = GetAwardMessage();
            awardUI.SetActive(true);
            bronzeMedal.SetActive(false);
            silverMedal.SetActive(true);
            goldMedal.SetActive(false);
            Invoke("hideAward", 5);
        } else if (WinsAward(awardMultiplier * 10))
        {
            text.text = GetAwardMessage();
            awardUI.SetActive(true);
            bronzeMedal.SetActive(true);
            silverMedal.SetActive(false);
            goldMedal.SetActive(false);
            Invoke("hideAward", 5);
        }
    }

    private void hideAward()
    {
        bronzeMedal.SetActive(false);
        silverMedal.SetActive(false);
        goldMedal.SetActive(false);
        awardUI.SetActive(false);
    }

    public abstract bool WinsAward(int count);
    public abstract string GetAwardMessage();
}
