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
        }

        [TearDown]
        public void Teardown()
        {
            GameObject.Destroy(testObject);
        }

        
        [UnityTest]
        public IEnumerator GameManagerInitSetup()
        {
            // check that game over image and text are NOT showing
            Assert.IsFalse(gameManager.gameOverImage.activeSelf);
            Assert.IsFalse(gameManager.gameOverText.gameObject.activeSelf);

            // check that timer is showing
            Assert.IsTrue(gameManager.timerManager.timerSlider.enabled);

            yield return null;
        }
    }
}
