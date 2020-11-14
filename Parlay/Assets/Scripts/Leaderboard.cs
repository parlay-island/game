using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/**
 * This file holds the business logic for the leaderboard. Each leaderboard
 * entry is populated with data from the backend to display each run's ranking
 * within the class. All UI modifications to the leaderboard can be done here.
 * 
 * @author: Holly Ansel, Andres Montoya
 */

public class Leaderboard : MonoBehaviour
{
    [SerializeField] public AbstractWebRetriever webRetriever;
    [SerializeField] public ErrorDisplaySource errorDisplaySource;
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> resultEntryTransformList;
    private List<ResultModel> results = new List<ResultModel>();
    private GameEndRequestHelper _gameEndRequestHelper;
    private PlayerAuth _playerAuth;

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

    public void SetPlayer(PlayerAuth playerAuth)
    {
        _playerAuth = playerAuth;
    }

    public void SetGameEndRequestHelper(GameEndRequestHelper gameEndRequestHelper)
    {
        _gameEndRequestHelper = gameEndRequestHelper;
    }

    private void RetrieveResultsIfNotAlreadySet()
    {
        if (results == null || results.Count == 0)
        {
            results = webRetriever.GetMostRecentResults();
            EndResult endResult = _gameEndRequestHelper.getMostRecentEndResult();
            results.Add(new ResultModel(endResult.level, endResult.distance, _playerAuth.GetId(), _playerAuth.GetName()));
            results.Sort((model1, model2) => model2.distance.CompareTo(model1.distance));

        }
    }

    private void CreateLeaderboard()
    {
      entryContainer = transform;
      entryTemplate = entryContainer.Find("LeaderboardEntry").transform;

      entryTemplate.gameObject.SetActive(false);

      resultEntryTransformList = new List<Transform>();
      for(int i = 0; i < Mathf.Min(10, results.Count); i++)
      {
          ResultModel resultEntry = results[i];
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
