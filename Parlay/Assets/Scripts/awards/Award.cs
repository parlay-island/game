using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class Award : MonoBehaviour
{
    [SerializeField] public GameObject awardUI;
    [SerializeField] public Text text;

    void Start()
    {
        awardUI.SetActive(false);
        StartCoroutine(DisplayAward());
    }

    IEnumerator DisplayAward()
    {
        while (true)
        {
            if (WinsAward())
            {
                text.text = GetAwardMessage();
                awardUI.SetActive(true);
                yield return new WaitForSeconds(5);
                awardUI.SetActive(false);
            }
            yield return null;
        }
    }

    public abstract bool WinsAward();
    public abstract string GetAwardMessage();
}