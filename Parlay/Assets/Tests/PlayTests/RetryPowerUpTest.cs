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

    public class RetryPowerUpTest
    {
        private GameObject player;
        private CharacterController2D characterController;
        private GameObject powerUp;
        private GameManager gameManager;
        private GameObject gameManagerObj;

        private const int TimeReward = 1;
        private const string QuestionText = "question";
        private const string RightChoice = "choice0";
        private const string WrongChoice = "choice1";
        private const int RightChoiceIndex = 0;
        private const int WrongChoiceIndex = 1;

        private GameObject _uiGameObject;
        private List<GameObject> _questionManagerGameObjectList;
        private QuestionManager _questionManager;
        private Timer _timer;

        public class MockWebRetriever : AbstractWebRetriever
        {
            public override List<QuestionModel> GetQuestions()
            {
                return new List<QuestionModel>
                {
                    new QuestionModel(QuestionText, new List<ChoiceModel>
                        {
                            new ChoiceModel(RightChoice),
                            new ChoiceModel(WrongChoice)
                        },
                        new List<int> { RightChoiceIndex })
                };
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

        public class TimeoutWebRetriever : AbstractWebRetriever
        {
            public override List<QuestionModel> GetQuestions()
            {
                throw new TimeoutException();
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
            foreach (GameObject question in GameObject.FindGameObjectsWithTag("Question"))
            {
                GameObject.Destroy(question);
            }

            _questionManagerGameObjectList = new List<GameObject>();
            _uiGameObject = new GameObject();
            GameObject questionManagerObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/QuestionManager"));
            _questionManagerGameObjectList.Add(questionManagerObj);
            _questionManager = questionManagerObj.GetComponent<QuestionManager>();
            GameObject timeManagerObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/TimeManager"));
            _questionManagerGameObjectList.Add(timeManagerObj);
            _timer = timeManagerObj.GetComponent<Timer>();
            _questionManager.webRetriever = AddComponent<MockWebRetriever>();
            _questionManager.timer = _timer;
            _questionManager.questionUI = _uiGameObject;
            _questionManager.SetTimeReward(TimeReward);
            _questionManager.SetQuestionText(AddComponent<Text>());
            _questionManager.errorDisplaySource = AddComponent<ErrorDisplaySource>();
            _questionManager.errorDisplaySource.errorTitle = AddComponent<Text>();
            _questionManager.errorDisplaySource.errorMessage = AddComponent<Text>();
            _questionManager.errorDisplaySource.errorMessageObject = new GameObject();

            _questionManager.SetChoiceTexts(new List<Text>
            {
                AddComponent<Text>(),
                AddComponent<Text>()
            });
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

            foreach (GameObject chunk in GameObject.FindGameObjectsWithTag("Chunck"))
            {
                GameObject.Destroy(chunk);
            }

            GameObject.Destroy(_uiGameObject);
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
            powerUp = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Terrain Prefabs/Interactible Tiles/GemTile2"));
            CollideWithPowerUp();
            yield return new WaitForSeconds(2);
            //Test if time was increased
            Assert.True(gameManager.retries.Count > 0);
            _questionManager.UserSelect(1);
            Assert.True(gameManager.retries.Count == 0);
        }

    }
}
