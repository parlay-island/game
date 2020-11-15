﻿using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System;
using TMPro;
using UnityEngine.UI;

/**
* This file tests the Game Manager
* 
* @author: Holly Ansel, Jessica Su
*/

namespace Tests
{
    public class GameManagerTest
    {
        private GameObject testObject;
        private GameObject testPlayer;
        private GameManager gameManager;
        private CharacterController2D characterController;
        private PlayerMovement playerMovement;
        private float distanceTraveled;
        private GameObject mockWebRetrieverObj;
        private GameObject _uiGameObject;
        private GameObject questionManagerGameObject;
        private QuestionManager _questionManager;
        private static int level = 1;

        private static bool mockingIncreasedDistance;
        private static bool mockingAnsweredQuestion;

        private static int user_choice = 1;

        private static QuestionModel firstQuestion = new QuestionModel("question", new List<ChoiceModel>
            {
                new ChoiceModel("choice0"),
                new ChoiceModel("choice1")
            },
            new List<int> {0});

        public class GameMockWebRetriever : AbstractWebRetriever {

            public override void SetUp(string auth_token, int level){
            }
            public override List<QuestionModel> GetQuestions() {
                return new List<QuestionModel>
                {
                    firstQuestion
                };
            }

            public override void PostEndResult(EndResult result, int playerID) {
            }

            public override string GetMostRecentPostRequestResult() {
              string requestParamContent = "distance:0.00,level:" + level;
              if (mockingIncreasedDistance) {
                  requestParamContent = "distance:1.00,level:" + level;
              }

              if (mockingAnsweredQuestion)
              {
                  requestParamContent = "{'results': [{'question':0, 'player':2, 'choice':1, 'count':1}]}";
              }
              return requestParamContent;
            }
            public override void FetchResults() {
            }
            public override List<ResultModel> GetMostRecentResults() {
              return new List<ResultModel>();
            }
            public override bool IsLoading()
            {
              return false;
            }
        }

        public class TimeoutWebRetriever : AbstractWebRetriever
        {
            public override void SetUp(string auth_token, int level){
            }
            public override List<QuestionModel> GetQuestions()
            {
                throw new TimeoutException();
            }

            public override void PostEndResult(EndResult result, int playerID) {
                throw new TimeoutException();
            }

            public override string GetMostRecentPostRequestResult() {
              return "";
            }
            public override void FetchResults() {
            }
            public override List<ResultModel> GetMostRecentResults() {
              return new List<ResultModel>();
            }
            public override bool IsLoading()
            {
              return false;
            }
        }

        [SetUp]
        public void Setup()
        {
            initializeGameManager();
            initializeWebRetriever();
            initializeQuestionManager();
            initializePlayer();
            gameManager.setGameTime(2f);
        }

        private void initializeGameManagerAndPlayer() {
            initializeGameManager();
            initializePlayer();
        }

        private void initializeQuestionManager()
        {
            questionManagerGameObject =
                MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Questions/QuestionManager"));
            _questionManager = questionManagerGameObject.GetComponent<QuestionManager>();
            _uiGameObject = new GameObject();
            _questionManager.webRetriever = questionManagerGameObject.AddComponent<QuestionManagerTest.MockWebRetriever>();
            _questionManager.timer = null;
            _questionManager.questionUI = _uiGameObject;
            _questionManager.SetTimeReward(10);
        }

        private void initializeGameManager() {
            testObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/GameManager"));
            gameManager = testObject.GetComponent<GameManager>();
            mockingIncreasedDistance = false;
            mockingAnsweredQuestion = false;
        }

        private void initializeWebRetriever() {
            mockWebRetrieverObj = new GameObject();
            GameMockWebRetriever mockWebRetriever = mockWebRetrieverObj.AddComponent<GameMockWebRetriever>();
            gameManager.webRetriever = mockWebRetriever;
            gameManager.gameEndRequestHelper = new GameEndRequestHelper(mockWebRetriever, level);
        }

        private void initializePlayer() {
            testPlayer = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
            gameManager.player = testPlayer;

            playerMovement = testPlayer.GetComponent<PlayerMovement>();
            characterController = testPlayer.GetComponent<CharacterController2D>();
            distanceTraveled = 10f;
        }

        [TearDown]
        public void Teardown()
        {
          foreach(GameObject obj in GameObject.FindObjectsOfType<GameObject>()) {
            GameObject.Destroy(obj);
          }
        }

        [UnityTest, Order(2)]
        public IEnumerator TestTimerCountdownIntegration() {
            // check that game over image and text are NOT showing
            Assert.IsFalse(gameManager.gameOverImage.activeSelf);

            // check that timer is showing
            Assert.IsTrue(gameManager.timerManager.timerSlider.enabled);
            Assert.AreEqual(gameManager.timerManager.timerText.text, "0:02");

            // check that game time decreases properly
            gameManager.timerManager.mockDecreaseTime(1f);
            Assert.AreEqual(gameManager.timerManager.timerText.text, "0:01");
            Assert.IsFalse(gameManager.timerManager.isTimeUp());

            // check that game over screen shows at end
            gameManager.timerManager.mockDecreaseTime(1f);
            gameManager.gameOver();
            Assert.IsTrue(gameManager.gameOverImage.activeSelf);
            Assert.AreEqual(gameManager.timerManager.timerText.text, "0:00");
            Assert.IsTrue(gameManager.timerManager.isTimeUp());

            // check that time doesn't go below 0
            yield return new WaitForSeconds(1f);
            Assert.AreEqual(gameManager.timerManager.timerText.text, "0:00");
        }

        [UnityTest, Order(3)]
        public IEnumerator TestPostRequestForEndResultsWithoutTimeout() {
            // exception should not be thrown because there is no timeout
            Assert.That(() => gameManager.gameOver(), Throws.Nothing);
            yield return new WaitForSeconds(0.1f);

            // player did not move at all for this test --> distance should be 0.00
            Assert.That(gameManager.gameEndRequestHelper.getPostEndResultContent().Contains(gameManager.playerDistance.ToString("F2")));
            Assert.That(gameManager.gameEndRequestHelper.getPostEndResultContent().Contains(level.ToString()));
        }

        [UnityTest, Order(4)]
        public IEnumerator TestPostRequestForEndResultsAfterPlayerMovement() {
            // mock the web retriever with distance greater than 0
            mockingIncreasedDistance = true;
            initializeWebRetriever();

            characterController.Move(distanceTraveled, false);
            yield return new WaitForSeconds(0.3f);

            Assert.That(() => gameManager.gameOver(), Throws.Nothing);
            yield return new WaitForSeconds(0.1f);

            // making sure the mocked post request was sent with the right values
            string postEndResultContent = gameManager.gameEndRequestHelper.getPostEndResultContent();
            string[] parts = postEndResultContent.Split(',');
            float distance = float.Parse(parts[0].Split(':')[1]);

            Assert.Greater(distance, 0f);
            Assert.That(gameManager.gameEndRequestHelper.getPostEndResultContent().Contains(level.ToString()));
        }

        [UnityTest, Order(1)]
        public IEnumerator TestPostRequestForEndResultAfterAnsweringQuestion()
        {
            mockingAnsweredQuestion = true;
            initializeWebRetriever();

            // answer question
            _questionManager.UserSelect(user_choice);
            yield return new WaitForSeconds(0.1f);

            // make sure that question is added to questions answered by player
            List<AnsweredQuestion> playerAnsweredQuestions = gameManager.getPlayerAnsweredQuestions();
            yield return new WaitForSeconds(0.1f);
            Assert.AreEqual(playerAnsweredQuestions[0].question_id, firstQuestion.id);

            Assert.That(() => gameManager.gameOver(), Throws.Nothing);
            yield return new WaitForSeconds(0.1f);

            string postEndResultContent = gameManager.gameEndRequestHelper.getPostEndResultContent();
            Assert.That(postEndResultContent.Contains(firstQuestion.id.ToString()));
            Assert.That(postEndResultContent.Contains(user_choice.ToString()));
        }

        [UnityTest, Order(5)]
        public IEnumerator TestPostRequestForEndResultsWithTimeout() {
            GameObject timeoutWebRetrieverObj = new GameObject();
            TimeoutWebRetriever timeoutWebRetriever = timeoutWebRetrieverObj.AddComponent<TimeoutWebRetriever>();
            gameManager.webRetriever = timeoutWebRetriever;
            gameManager.gameEndRequestHelper = new GameEndRequestHelper(timeoutWebRetriever, level);

            // making sure that exception is thrown
            try {
                gameManager.gameOver();
                // should not get here because exception should be thrown first
                Assert.Fail();
            } catch (Exception e) {
                // exception caught
            }
            GameObject.Destroy(timeoutWebRetrieverObj);
            yield return null;
        }
    }
}
