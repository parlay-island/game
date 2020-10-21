using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] public AbstractWebRetriever webRetriever;
    [SerializeField] public ErrorDisplaySource errorDisplaySource;
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> resultEntryTransformList;
    private List<ResultModel> results = new List<ResultModel>();

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
      entryContainer = transform.Find("highscoreEntryContainer");
      entryTemplate = entryContainer.Find("highscoreEntryTemplate");

      entryTemplate.gameObject.SetActive(false);

      resultEntryTransformList = new List<Transform>();
      foreach(ResultModel resultEntry in results)
      {
          CreateHighScoreEntryTransform(resultEntry, entryContainer, resultEntryTransformList);
      }
    }

    private void CreateHighScoreEntryTransform(ResultModel resultEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 75f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            case 1:
                rankString = "1ST";
                break;
            case 2:
                rankString = "2ND";
                break;
            case 3:
                rankString = "3RD";
                break;
            default:
                rankString = rank + "TH";
                break;
        }

        entryTransform.Find("PositionText").GetComponent<Text>().text = rankString;

        float score = resultEntry.distance;

        entryTransform.Find("ScoreText").GetComponent<Text>().text = score.ToString();

        string user = resultEntry.player_name;
        entryTransform.Find("NameText").GetComponent<Text>().text = user;

        //Set background visible odds and evens, makes it easier to read
        entryTransform.Find("templateBackground").gameObject.SetActive(rank % 2 == 1);

        if (rank == 1)
        {
            entryTransform.Find("NameText").GetComponent<Text>().color = Color.green;
            entryTransform.Find("ScoreText").GetComponent<Text>().color = Color.green;
            entryTransform.Find("PositionText").GetComponent<Text>().color = Color.green;
        }

        transformList.Add(entryTransform);
    }

}
