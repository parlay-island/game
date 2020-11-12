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
    [SerializeField] public AwardManager awardManager;
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> resultEntryTransformList;
    private List<ResultModel> results = new List<ResultModel>();
    public GameObject medal;
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
            Debug.Log(exception);
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
            results.Add(new ResultModel(endResult.level, endResult.distance, _playerAuth.GetId(), awardManager.top_award, _playerAuth.GetName()));
            results.Sort((model1, model2) => model1.distance.CompareTo(model2.distance));
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
            Debug.Log("Adding recorded award");
            Sprite temp = Resources.Load<Sprite>("Prefabs/Award Prefabs/" + awards[0]);
            //If sprite is null then sprite shows empty which is desired functionality
            entryTransform.Find("Award").GetComponent<SpriteRenderer>().sprite = temp;
        }
        else
        {
            Debug.Log("Removing reference to award");
            entryTransform.Find("Award").GetComponent<SpriteRenderer>().sprite = null;
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
