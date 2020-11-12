using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] public AbstractWebRetriever webRetriever;
    [SerializeField] public ErrorDisplaySource errorDisplaySource;
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> resultEntryTransformList;
    private List<ResultModel> results = new List<ResultModel>();
    public GameObject medal;

    public void Start()
    {
        try
        {
            RetrieveResultsIfNotAlreadySet();
            CreateLeaderboard();
        }
        catch (Exception exception)
        {
            errorDisplaySource.DisplayNewError("Cannot load leaderboard", "An error occurred while loading "
                            + "results. Please try again later.");
            gameObject.SetActive(false);
            Debug.Log(exception);
        }
    }

    public void ClearResults()
    {
      results = new List<ResultModel>();
    }

    private void RetrieveResultsIfNotAlreadySet()
    {
        if (results == null || results.Count == 0)
        {
            results = webRetriever.GetMostRecentResults();
        }
    }

    private void CreateLeaderboard()
    {
      entryContainer = transform;
      entryTemplate = entryContainer.Find("LeaderboardEntry").transform;

      entryTemplate.gameObject.SetActive(false);

      resultEntryTransformList = new List<Transform>();
      foreach(ResultModel resultEntry in results)
      {
          CreateHighScoreEntryTransform(resultEntry, entryContainer, resultEntryTransformList);
      }
    }

    private void CreateHighScoreEntryTransform(ResultModel resultEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 1.3f;
        float initialPos = -1.6f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(-0f, initialPos + (-templateHeight * transformList.Count));
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString = GetRankNumber(rank);
        entryTransform.Find("Rank").GetComponent<TextMeshProUGUI>().SetText(rankString);

        float score = Mathf.Round(resultEntry.distance);
        entryTransform.Find("Distance").GetComponent<TextMeshProUGUI>().SetText(score.ToString() + "m");

        string user = resultEntry.player_name;
        entryTransform.Find("Name").GetComponent<TextMeshProUGUI>().SetText(user);
        
        List<string> awards = resultEntry.award_list;
        if (awards != null && awards.Count == 1)
        {
            medal.SetActive(true);
            Sprite temp = Resources.Load<Sprite>("Resources/Prefabs/Award Prefabs/" + awards[0]);
            //If sprite is null then sprite shows empty which is desired functionality
            medal.GetComponent<SpriteRenderer>().sprite = temp;
        }
        else
        {
            medal.SetActive(false);
        }


        transformList.Add(entryTransform);
    }

    private string GetRankNumber(int rank)
    {
      switch (rank)
      {
          case 1:
              return "1st";
          case 2:
              return "2nd";
          case 3:
              return "3rd";
          default:
              return rank + "th";
      }
    }

}
