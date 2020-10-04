using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Tests
{
    public class GameManagerTest
    {
        private GameObject testObject;
        private GameManager gameManager;

        public class MockWebRetriever : AbstractWebRetriever {
            public override Task<List<QuestionModel>> GetQuestions() {
                return Task.Run(() => new List<QuestionModel>());
            }

            public override Task<HttpResponseMessage> PostEndResult(ResultModel result, int playerID) {
                HttpContent content = new StringContent("distance:0.00,level:1");

                return Task.Run(() => 
                    new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = content });
            }
        }

        public class TimeoutWebRetriever : AbstractWebRetriever
        {
            public override Task<List<QuestionModel>> GetQuestions()
            {
                throw new TimeoutException();
            }

            public override Task<HttpResponseMessage> PostEndResult(ResultModel result, int playerID) {
                throw new TimeoutException();
            }
        }

        [SetUp]
        public void Setup()
        {
           initializeGameManager(); 
        }

        private void initializeGameManager() {
            testObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/GameManager"));
            gameManager = testObject.GetComponent<GameManager>();
            gameManager.setGameTime(2f);
            
            gameManager.webRetriever = new GameObject().AddComponent<MockWebRetriever>();
        }

        [TearDown]
        public void Teardown()
        {
            GameObject.Destroy(testObject);
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

            Assert.That(() => gameManager.gameOver(), Throws.Nothing);

            yield return new WaitForSeconds(0.1f);
            // player did not move at all for this test --> distance should be 0.00
            Assert.That(gameManager.getPostEndResultContent().Contains("0.00"));
            Assert.That(gameManager.getPostEndResultContent().Contains(gameManager.getLevel().ToString()));
        }

        [UnityTest]
        public IEnumerator TestPostRequestForEndResultsWithTimeout() {
            gameManager.webRetriever = new GameObject().AddComponent<TimeoutWebRetriever>();

            // making sure that exception is thrown
            try {
                gameManager.gameOver();

                // should not get here because exception should be thrown first
                Assert.Fail();
            } catch (Exception e) {
                Debug.Log(e);
            }
            yield return null;
        }
        
    }
}
