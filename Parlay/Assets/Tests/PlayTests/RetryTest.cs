using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class RetryTest
    {
        private GameObject testObject;
        private LevelGenerator levelGenerator;
        private GameObject playerObj;
        private GameObject chunckObj;
        private GameManager gameManager;
        private GameObject gameManagerObj;

        [SetUp]
        public void Setup()
        {
            testObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/LevelGenerator"));
            playerObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
            levelGenerator = testObject.GetComponent<LevelGenerator>();
            gameManagerObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/GameManager"));
            gameManager = gameManagerObj.GetComponent<GameManager>();
            gameManager.setGameTime(30f);
            gameManager.questionUI.gameObject.SetActive(false);
            levelGenerator.player = playerObj.GetComponent<Rigidbody2D>();
            foreach (GameObject question in GameObject.FindGameObjectsWithTag("Question"))
            {
                GameObject.Destroy(question);
            }
            foreach (GameObject chunk in GameObject.FindGameObjectsWithTag("Chunck"))
            {
                GameObject.Destroy(chunk);
            }
        }

        [TearDown]
        public void Teardown()
        {
            gameManager.questionUI.gameObject.SetActive(false);
            GameObject.Destroy(testObject);
            GameObject.Destroy(playerObj);
            GameObject.Destroy(gameManagerObj);
            foreach (GameObject chunk in GameObject.FindGameObjectsWithTag("Chunck"))
            {
                GameObject.Destroy(chunk);
            }
        }

        [Retry(3)]
        [UnityTest , Order(1)]
        public IEnumerator resetLevel()
        {
            levelGenerator.player.transform.position = new Vector3(67, 1);
            yield return new WaitForSeconds(0.1f);
            levelGenerator.Reset();
            yield return new WaitForSeconds(0.1f);
            Assert.IsTrue(levelGenerator.player.transform.position.x < 66);
            Debug.Log(GameObject.FindGameObjectsWithTag("Chunck").Length);
            Assert.IsTrue(GameObject.FindGameObjectsWithTag("Chunck").Length <= 6);
            yield return null;
        }

        [UnityTest, Order(2)]
        public IEnumerator resetTime()
        {
            GameManager.instance.setGameTime(20f);
            GameManager.instance.Reset(30f);
            Assert.IsTrue(GameManager.instance.timerManager.getCurrTime() > 20f);
            yield return null;
        }
    }
}
