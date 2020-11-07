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

    public class RetryPowerUpTest
    {
        private GameObject player;
        private CharacterController2D characterController;
        private GameObject powerUp;
        private GameManager gameManager;
        private GameObject gameManagerObj;
        private GameObject mockWebRetrieverObj;
        private Text powerupText;

        private QuestionManager _questionManager;
        private GameObject questionManagerGameObject;
        private List<GameObject> _questionManagerGameObjectList;

        private GameObject awardUI;
        private TextMeshProUGUI awardText;
        private Answered10QuestionsAward award;


        [SetUp]
        public void Setup()
        {
            foreach (GameObject question in GameObject.FindGameObjectsWithTag("Question"))
            {
                GameObject.Destroy(question);
            }
            initGameManager();

            _questionManagerGameObjectList = new List<GameObject>();
            questionManagerGameObject =
                MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Questions/QuestionManager"));
            _questionManager = questionManagerGameObject.GetComponent<QuestionManager>();
            award = questionManagerGameObject.AddComponent<Answered10QuestionsAward>();
            awardText = questionManagerGameObject.AddComponent<TextMeshProUGUI>();
            awardUI = new GameObject();
            award.awardUI = awardUI;
            award.text = awardText;
            award.questionManager = _questionManager;
            _questionManager.webRetriever = questionManagerGameObject.AddComponent<QuestionManagerTest.MockWebRetriever>();
            _questionManager.timer = null;
            _questionManager.questionUI = awardUI;
            _questionManager.SetTimeReward(10);
            _questionManager.SetQuestionText(AddComponent<TextMeshProUGUI>());
            _questionManager.errorDisplaySource = questionManagerGameObject.AddComponent<ErrorDisplaySource>();
            _questionManager.errorDisplaySource.errorTitle = AddComponent<Text>();
            _questionManager.errorDisplaySource.errorMessage = AddComponent<Text>();
            _questionManager.errorDisplaySource.errorMessageObject = new GameObject();
        }

        private void initGameManager()
        {
            foreach (GameObject GM in GameObject.FindGameObjectsWithTag("GameManager"))
            {
                GameObject.Destroy(GM);
            }
            gameManagerObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/GameManager"));
            gameManager = gameManagerObj.GetComponent<GameManager>();
            player = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
            characterController = player.GetComponent<CharacterController2D>();
            player.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
            gameManager.setGameTime(30f);
            gameManager.questionUI.gameObject.SetActive(false);
            characterController.Move(10f, false);
            powerupText = MonoBehaviour.Instantiate(Resources.Load<Text>("Prefabs/PowerUpLabel"));
            gameManager.powerUpText = powerupText;
            GameManager.instance.powerUpText = powerupText;
        }

        private T AddComponent<T>() where T : Component
        {
            var gameObject = new GameObject();
            _questionManagerGameObjectList.Add(gameObject);
            return gameObject.AddComponent<T>();
        }

        [TearDown]
        public void Teardown()
        {
            gameManager.questionUI.gameObject.SetActive(false);
            GameObject.Destroy(powerUp);
            GameObject.Destroy(player);
            GameObject.Destroy(gameManagerObj);
            GameObject.Destroy(mockWebRetrieverObj);
            GameObject.Destroy(questionManagerGameObject);
            GameObject.Destroy(powerupText);

            foreach (GameObject chunk in GameObject.FindGameObjectsWithTag("Chunck"))
            {
                GameObject.Destroy(chunk);
            }

            foreach (var gameObject in _questionManagerGameObjectList)
            {
                GameObject.Destroy(gameObject);
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

        [UnityTest, Order(1)]
        public IEnumerator TestRetryPowerUpActivation()
        {
            powerUp = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Terrain Prefabs/Interactible Tiles/ChestTile1"));
            powerUp.GetComponent<PowerUp>().type = 3;
            CollideWithPowerUp();
            yield return new WaitForSeconds(2);
            //Test if time was increased
            Assert.True(GameManager.instance.retries.Count > 0);
            GameManager.instance.canRetry = true;
            _questionManager.UserSelect(1);
            Assert.True(GameManager.instance.retries.Count == 0);
        }

    }
}
