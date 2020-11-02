using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{

    public class DistancePowerUpTest
    {
        private GameObject player;
        private CharacterController2D characterController;
        private GameObject powerUp;
        private GameManager gameManager;
        private GameObject gameManagerObj;
        private GameObject mockWebRetrieverObj;

        public class MockWebRetriever : AbstractWebRetriever
        {
            public override List<QuestionModel> GetQuestions()
            {
                return new List<QuestionModel>();
            }

            public override void PostEndResult(ResultModel result, int playerID)
            {
            }

            public override string GetMostRecentPostRequestResult()
            {
                return "";
            }
            public override void FetchResults(int level)
            {
            }
            public override List<ResultModel> GetMostRecentResults()
            {
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

            player = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
            characterController = player.GetComponent<CharacterController2D>();
            player.GetComponent<Rigidbody2D>().gravityScale = 0.0f;

            characterController.Move(10f, false);
            foreach (GameObject question in GameObject.FindGameObjectsWithTag("Question"))
            {
                GameObject.Destroy(question);
            }


        }

        public void initGameManager()
        {
            foreach (GameObject GM in GameObject.FindGameObjectsWithTag("GameManager"))
            {
                GameObject.Destroy(GM);
            }
            gameManagerObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/GameManager"));
            gameManager = gameManagerObj.GetComponent<GameManager>();
            gameManager.setGameTime(30f);
            gameManager.questionUI.gameObject.SetActive(false);
            mockWebRetrieverObj = new GameObject();
            MockWebRetriever mockWebRetriever = mockWebRetrieverObj.AddComponent<MockWebRetriever>();
            gameManager.webRetriever = mockWebRetriever;
        }

        [TearDown]
        public void Teardown()
        {
            gameManager.questionUI.gameObject.SetActive(false);
            GameObject.Destroy(mockWebRetrieverObj);
            GameObject.Destroy(powerUp);
            GameObject.Destroy(player);
            GameObject.Destroy(gameManagerObj);

            foreach (GameObject chunk in GameObject.FindGameObjectsWithTag("Chunck"))
            {
                GameObject.Destroy(chunk);
            }
        }

        private Vector2 GetPowerUpColliderPos()
        {
            return powerUp.transform.localPosition - (powerUp.GetComponent<SpriteRenderer>().bounds.extents / 2);
        }

        private void CollideWithPowerUp()
        {
            Vector2 powerUpPos = GetPowerUpColliderPos();
            characterController.Move(powerUpPos.x - 0.1f, false);
        }


        [Retry(4)]
        [UnityTest, Order(1)]
        public IEnumerator TestDistancePowerUpActivation()
        {
            initGameManager();
            powerUp = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Terrain Prefabs/Interactible Tiles/GemTile1"));
            CollideWithPowerUp();
            yield return new WaitForSeconds(2);
            //Test if time was increased
            Assert.True(GameManager.instance.bonusDistance > 0);
        }

    }
}
