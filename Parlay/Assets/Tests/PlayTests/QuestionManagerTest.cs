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
    public class QuestionManagerTest
    {
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
        private GameManager gameManager;
        private GameObject gameManagerObj;

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

            public override void PostEndResult(ResultModel result, int playerID) {
            }

            public override string GetMostRecentPostRequestResult() {
              return "";
            }
            public override void FetchResults(int level) {
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
            public override List<QuestionModel> GetQuestions()
            {
                throw new TimeoutException();
            }

            public override void PostEndResult(ResultModel result, int playerID) {
            }

            public override string GetMostRecentPostRequestResult() {
              return "";
            }
            public override void FetchResults(int level) {
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
        public void SetUp()
        {
            _questionManagerGameObjectList = new List<GameObject>();
                _uiGameObject = new GameObject();
            GameObject questionManagerObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Questions/QuestionManager"));
            _questionManagerGameObjectList.Add(questionManagerObj);
            _questionManager = questionManagerObj.GetComponent<QuestionManager>();
            GameObject timeManagerObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/TimeManager"));
            _questionManagerGameObjectList.Add(timeManagerObj);
            _timer = timeManagerObj.GetComponent<Timer>();
            MockWebRetriever webRetriever = AddComponent<MockWebRetriever>();
            _questionManager.webRetriever = webRetriever;
            _questionManager.timer = _timer;
            _questionManager.questionUI = _uiGameObject;
            _questionManager.SetTimeReward(TimeReward);
            _questionManager.SetQuestionText(AddComponent<TextMeshProUGUI>());
            _questionManager.errorDisplaySource = AddComponent<ErrorDisplaySource>();
            _questionManager.errorDisplaySource.errorTitle = AddComponent<Text>();
            _questionManager.errorDisplaySource.errorMessage = AddComponent<Text>();
            _questionManager.errorDisplaySource.errorMessageObject = new GameObject();

                _questionManager.SetChoiceTexts(new List<TextMeshProUGUI>
            {
                AddComponent<TextMeshProUGUI>(),
                AddComponent<TextMeshProUGUI>()
            });

            gameManagerObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/GameManager"));
            gameManager = gameManagerObj.GetComponent<GameManager>();
            gameManager.webRetriever = webRetriever;
            gameManager.gameEndRequestHelper = new GameEndRequestHelper(webRetriever);
            _questionManager.gameManager = gameManager;

        }

        [TearDown]
        public void TearDown()
        {
            GameObject.Destroy(_uiGameObject);
            GameObject.Destroy(gameManagerObj);
            foreach (var gameObject in _questionManagerGameObjectList)
            {
                GameObject.Destroy(gameObject);
            }
        }

        private T AddComponent<T>() where T : Component
        {
            var gameObject = new GameObject();
            _questionManagerGameObjectList.Add(gameObject);
            return gameObject.AddComponent<T>();
        }

        [UnityTest, Order(1)]
        public IEnumerator UserSelectAddsToTimerWhenQuestionIsCorrect()
        {
            var timeBefore = _timer.getCurrTime();
            _questionManager.UserSelect(RightChoiceIndex);
            Assert.Less(timeBefore, _timer.getCurrTime());
            yield return null;
        }

        [UnityTest, Order(2)]
        public IEnumerator UserSelectDoesntAddToTimerWhenQuestionIsIncorrect()
        {
            var timeBefore = _timer.getCurrTime();
            _questionManager.UserSelect(WrongChoiceIndex);
            Assert.AreEqual(timeBefore,_timer.getCurrTime(), 0.001);
            yield return null;
        }

        [UnityTest, Order(3)]
        public IEnumerator UserSelectTurnsUIInactive()
        {
            _questionManager.UserSelect(RightChoiceIndex);
            Assert.That(!_uiGameObject.activeSelf);
            yield return null;
        }

        [UnityTest, Order(4)]
        public IEnumerator ApiTimesOutResponseSoTheErrorAppears()
        {
            var errorMessageBefore = _questionManager.errorDisplaySource.errorMessage.text;
            _questionManager.webRetriever = AddComponent<TimeoutWebRetriever>();
            _questionManager.Start();
            Assert.AreNotEqual(errorMessageBefore,
                _questionManager.errorDisplaySource.errorMessage.text);
            yield return null;
        }
    }
}
