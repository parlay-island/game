﻿using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System;

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

        private static bool mockingIncreasedDistance;

        public class MockWebRetriever : AbstractWebRetriever {
            public override List<QuestionModel> GetQuestions() {
                return new List<QuestionModel>();
            }

            public override void PostEndResult(ResultModel result, int playerID) {
            }

            public override string GetMostRecentPostRequestResult() {
              string requestParamContent = "distance:0.00,level:1";
              if (mockingIncreasedDistance) {
                  requestParamContent = "distance:1.00,level:1";
              }
              return requestParamContent;
            }
        }

        public class TimeoutWebRetriever : AbstractWebRetriever
        {
            public override List<QuestionModel> GetQuestions()
            {
                throw new TimeoutException();
            }

            public override void PostEndResult(ResultModel result, int playerID) {
                throw new TimeoutException();
            }

            public override string GetMostRecentPostRequestResult() {
              return "";
            }
        }

        [SetUp]
        public void Setup()
        {
            initializeGameManager();
            initializeWebRetriever();
        }

        private void initializeGameManagerAndPlayer() {
            initializeGameManager();
            initializePlayer();
        }

        private void initializeGameManager() {
            testObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/GameManager"));
            gameManager = testObject.GetComponent<GameManager>();
            gameManager.setGameTime(2f);
            mockingIncreasedDistance = false;
        }

        private void initializeWebRetriever() {
            MockWebRetriever mockWebRetriever = new GameObject().AddComponent<MockWebRetriever>();
            gameManager.webRetriever = mockWebRetriever;
            gameManager.gameEndRequestHelper = new GameEndRequestHelper(mockWebRetriever);
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
            GameObject.Destroy(testObject);
            GameObject.Destroy(testPlayer);
        }

        [UnityTest]
        public IEnumerator TestTimerCountdownIntegration() {
            initializeGameManager();

            // check that game over image and text are NOT showing
            Assert.IsFalse(gameManager.gameOverImage.activeSelf);
            Assert.IsFalse(gameManager.gameOverText.gameObject.activeSelf);

            // check that timer is showing
            Assert.IsTrue(gameManager.timerManager.timerSlider.enabled);
            Assert.AreEqual(gameManager.timerManager.timerText.text, "0:02");

            // check that game time decreases properly
            yield return new WaitForSeconds(1f);
            Assert.AreEqual(gameManager.timerManager.timerText.text, "0:01");
            Assert.IsFalse(gameManager.timerManager.isTimeUp());

            // check that game over screen shows at end
            yield return new WaitForSeconds(2f);
            Assert.IsTrue(gameManager.gameOverImage.activeSelf);
            Assert.IsTrue(gameManager.gameOverText.gameObject.activeSelf);
            Assert.AreEqual(gameManager.timerManager.timerText.text, "0:00");
            // check that time has run out
            Assert.IsTrue(gameManager.timerManager.isTimeUp());

            // check that time doesn't go below 0
            yield return new WaitForSeconds(1f);
            Assert.AreEqual(gameManager.timerManager.timerText.text, "0:00");
        }

        [UnityTest]
        public IEnumerator TestPostRequestForEndResultsWithoutTimeout() {
            initializeGameManager();
            initializeWebRetriever();

            // exception should not be thrown because there is no timeout
            Assert.That(() => gameManager.gameOver(), Throws.Nothing);
            yield return new WaitForSeconds(0.1f);

            // player did not move at all for this test --> distance should be 0.00
            Assert.That(gameManager.gameEndRequestHelper.getPostEndResultContent().Contains(gameManager.playerDistance.ToString("F2")));
            Assert.That(gameManager.gameEndRequestHelper.getPostEndResultContent().Contains(gameManager.level.ToString()));
        }

        [UnityTest]
        public IEnumerator TestPostRequestForEndResultsAfterPlayerMovement() {
            initializeGameManagerAndPlayer();

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
            Assert.That(gameManager.gameEndRequestHelper.getPostEndResultContent().Contains(gameManager.level.ToString()));
        }

        [UnityTest]
        public IEnumerator TestPostRequestForEndResultsWithTimeout() {
            initializeGameManager();

            TimeoutWebRetriever timeoutWebRetriever = new GameObject().AddComponent<TimeoutWebRetriever>();
            gameManager.webRetriever = timeoutWebRetriever;
            gameManager.gameEndRequestHelper = new GameEndRequestHelper(timeoutWebRetriever);

            // making sure that exception is thrown
            try {
                gameManager.gameOver();
                // should not get here because exception should be thrown first
                Assert.Fail();
            } catch (Exception e) {
                // exception caught
            }
            yield return null;
        }
    }
}
