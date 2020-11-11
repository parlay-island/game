using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using TMPro;

namespace Tests
{
    public class LeaderboardTest
    {
        private const float distance1 = 100.0f;
        private const string player_name1 = "test";
        private const float distance2 = 200.0f;
        private const string player_name2 = "test2";
        private const float distance3 = 300.0f;
        private const string player_name3 = "test3";
        private const int level = 1;

        private GameObject leaderboardObj;
        private Leaderboard leaderboard;
        private GameObject gameManager;
        private GameObject mockWebRetrieverObj;
        private GameEndRequestHelper _gameEndRequestHelper;

        public class MockWebRetriever : AbstractWebRetriever
        {
            private List<ResultModel> results;

            public override List<QuestionModel> GetQuestions()
            {
                return new List<QuestionModel>();
            }

            public override void PostEndResult(EndResult result, int playerID) {
            }

            public override string GetMostRecentPostRequestResult() {
              return "";
            }

            public override void FetchResults(int level, string auth_token) {
              results = new List<ResultModel>  {
                    new ResultModel(level, distance1,0, player_name1),
                    new ResultModel(level, distance2,1, player_name2)
                };
            }
            public override List<ResultModel> GetMostRecentResults() {
              return results;
            }
            public override bool IsLoading()
            {
              return false;
            }
        }

        public class TimeoutWebRetriever : AbstractWebRetriever
        {
            private List<ResultModel> results;

            public override List<QuestionModel> GetQuestions()
            {
              return new List<QuestionModel>();
            }

            public override void PostEndResult(EndResult result, int playerID) {
            }

            public override string GetMostRecentPostRequestResult() {
              return "";
            }
            public override void FetchResults(int level, string auth_token) {
            }
            public override List<ResultModel> GetMostRecentResults() {
              return results;
            }
            public override bool IsLoading()
            {
              return false;
            }
        }

        [SetUp]
        public void SetUp()
        {
          gameManager = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/GameManager"));
          mockWebRetrieverObj = new GameObject();
          MockWebRetriever mockWebRetriever = mockWebRetrieverObj.AddComponent<MockWebRetriever>();
          leaderboardObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Leaderboard/Leaderboard"));
          leaderboard = leaderboardObj.GetComponent<Leaderboard>();
          leaderboard.webRetriever = mockWebRetriever;
          var playerAuth = leaderboardObj.AddComponent<PlayerAuth>();
          playerAuth.SetPlayer(new PlayerModel(1, player_name3, 30.0f));
          leaderboard.SetPlayer(playerAuth);
          _gameEndRequestHelper = new GameEndRequestHelper(mockWebRetriever);
          _gameEndRequestHelper.postGameEndResults(distance3, level, 1, 
              new List<AnsweredQuestion>());
          leaderboard.SetGameEndRequestHelper(_gameEndRequestHelper);
          mockWebRetriever.FetchResults(level, "");
          leaderboardObj.gameObject.SetActive(true);
          gameManager.GetComponent<GameManager>().setGameTime(20f);
        }

        [TearDown]
        public void TearDown()
        {
          leaderboardObj.gameObject.SetActive(false);
          foreach(GameObject obj in GameObject.FindObjectsOfType<GameObject>()) {
            GameObject.Destroy(obj);
          }

        }

        [UnityTest, Order(1)]
        public IEnumerator LeaderboardShowsResultsCorrectly()
        {
            GameObject[] entries = GameObject.FindGameObjectsWithTag("LeaderboardEntry");
            GameObject entry0 = entries[0];
            GameObject entry1 = entries[1];
            GameObject entry2 = entries[2];
            Assert.AreEqual(entry0.transform.Find("Name").GetComponent<TextMeshProUGUI>().text, player_name1);
            Assert.AreEqual(entry1.transform.Find("Name").GetComponent<TextMeshProUGUI>().text, player_name2);
            Assert.AreEqual(entry2.transform.Find("Name").GetComponent<TextMeshProUGUI>().text, player_name3);
            Assert.AreEqual(entry0.transform.Find("Distance").GetComponent<TextMeshProUGUI>().text, distance1 + "m");
            Assert.AreEqual(entry1.transform.Find("Distance").GetComponent<TextMeshProUGUI>().text, distance2 + "m");
            Assert.AreEqual(entry2.transform.Find("Distance").GetComponent<TextMeshProUGUI>().text, distance3 + "m");
            yield return null;
        }

        [UnityTest, Order(2)]
        public IEnumerator ApiTimesOutResponseSoTheErrorAppears()
        {
            leaderboard.ClearResults();
            leaderboard.errorDisplaySource.errorMessage.text = "";
            var errorMessageBefore = leaderboard.errorDisplaySource.errorMessage.text;
            TimeoutWebRetriever mockWebRetriever = new GameObject().AddComponent<TimeoutWebRetriever>();
            leaderboard.webRetriever = mockWebRetriever;
            mockWebRetriever.FetchResults(level, "");
            leaderboard.Start();
            Assert.AreNotEqual(errorMessageBefore,
                leaderboard.errorDisplaySource.errorMessage.text);
            yield return null;
        }


    }
}
