using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Tests
{
    public class LeaderboardTest
    {
        private const float distance1 = 100.0f;
        private const string player_name1 = "test";
        private const float distance2 = 200.0f;
        private const string player_name2 = "test2";
        private const int level = 1;

        private GameObject leaderboardObj;
        private Leaderboard leaderboard;
        private GameObject gameManager;
        private GameObject mockWebRetrieverObj;

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
          leaderboardObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Leaderboard"));
          leaderboard = leaderboardObj.GetComponent<Leaderboard>();
          leaderboard.webRetriever = mockWebRetriever;
          mockWebRetriever.FetchResults(level, "");
          leaderboardObj.gameObject.SetActive(true);
          gameManager.GetComponent<GameManager>().setGameTime(20f);
        }

        [TearDown]
        public void TearDown()
        {
          leaderboardObj.gameObject.SetActive(false);
          GameObject.Destroy(mockWebRetrieverObj);
          GameObject.Destroy(leaderboardObj);
          GameObject.Destroy(gameManager);

        }

        [UnityTest, Order(1)]
        public IEnumerator LeaderboardShowsResultsCorrectly()
        {
            GameObject[] entries = GameObject.FindGameObjectsWithTag("LeaderboardEntry");
            GameObject entry0 = entries[0];
            GameObject entry1 = entries[1];
            Assert.AreEqual(entry0.transform.Find("NameText").GetComponent<Text>().text, player_name1);
            Assert.AreEqual(entry1.transform.Find("NameText").GetComponent<Text>().text, player_name2);
            Assert.AreEqual(entry0.transform.Find("ScoreText").GetComponent<Text>().text, distance1.ToString());
            Assert.AreEqual(entry1.transform.Find("ScoreText").GetComponent<Text>().text, distance2.ToString());
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
