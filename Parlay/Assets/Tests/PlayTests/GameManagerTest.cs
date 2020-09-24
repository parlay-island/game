using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class GameManagerTest
    {
        private GameObject testObject;
        private GameManager gameManager;

        [SetUp]
        public void Setup()
        {
            testObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/GameManager"));
            gameManager = testObject.GetComponent<GameManager>();
            gameManager.setGameTime(2f);
        }

        [TearDown]
        public void Teardown()
        {
            GameObject.Destroy(testObject);
        }

        
        // [UnityTest]
        // public IEnumerator GameManagerInitSetup()
        // {
        //     // check that game over image and text are NOT showing
        //     Assert.IsFalse(gameManager.gameOverImage.activeSelf);
        //     Assert.IsFalse(gameManager.gameOverText.gameObject.activeSelf);

        //     // check that timer is showing
        //     Assert.IsTrue(gameManager.timerManager.timerSlider.enabled);

        //     yield return null;
        // }

        [UnityTest]
        public IEnumerator TimerCountdownIntegration() {
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
    }
}
