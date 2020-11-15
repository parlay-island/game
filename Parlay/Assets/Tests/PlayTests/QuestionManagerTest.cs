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
        private const string Question2Text = "question2";
        private const string RightChoice = "choice0";
        private const string WrongChoice = "choice1";
        private const int RightChoiceIndex = 0;
        private const int WrongChoiceIndex = 1;
        private const int levelID = 1;

        private GameObject _uiGameObject;
        private QuestionManager _questionManager;
        private Timer _timer;
        private GameManager gameManager;
        private GameObject gameManagerObj;
        private List<Image> choicePanels;
        private GameObject awardObject;
        private AwardManager awardManager;
        private static QuestionModel firstQuestion = new QuestionModel(QuestionText, new List<ChoiceModel>
            {
                new ChoiceModel(RightChoice),
                new ChoiceModel(WrongChoice)
            },
            new List<int> {RightChoiceIndex});
        private static QuestionModel secondQuestion = new QuestionModel(Question2Text, new List<ChoiceModel>
            {
                new ChoiceModel(RightChoice),
                new ChoiceModel(WrongChoice)
            },
            new List<int> {RightChoiceIndex});

        public class MockWebRetriever : AbstractWebRetriever
        {
            private int level;
            public override void SetUp(string auth_token, int level){
              this.level = levelID;
            }
            public override List<QuestionModel> GetQuestions()
            {
                return new List<QuestionModel>
                {
                    firstQuestion,
                    secondQuestion
                };
            }

            public override void PostEndResult(EndResult result, int playerID) {
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

        public class TimeoutWebRetriever : AbstractWebRetriever
        {
            public override void SetUp(string auth_token, int level){
            }
            public override List<QuestionModel> GetQuestions()
            {
                throw new TimeoutException();
            }

            public override void PostEndResult(EndResult result, int playerID) {
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
        public void SetUp()
        {
            _uiGameObject = new GameObject();
            GameObject questionManagerObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Questions/QuestionManager"));
            _questionManager = questionManagerObj.GetComponent<QuestionManager>();
            MockWebRetriever webRetriever = AddComponent<MockWebRetriever>();
            _questionManager.webRetriever = webRetriever;
            _questionManager.questionUI = _uiGameObject;
            _questionManager.SetTimeReward(TimeReward);
            SetUpTimer();
            SetUpErrorMessage();
            SetUpText();
            SetUpPanels();
            SetUpGameManager(webRetriever);
            awardObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Award Prefabs/AwardManager"));
            awardManager = awardObject.GetComponent<AwardManager>();
            awardManager.award_list = new List<Award>();
            _questionManager.awardManager = awardManager;

            gameManager.setGameTime(30f);
            gameManager.awardManager = awardManager;
            _questionManager.Start();
        }

        private void SetUpErrorMessage()
        {
          _questionManager.errorDisplaySource = AddComponent<ErrorDisplaySource>();
          _questionManager.errorDisplaySource.errorTitle = AddComponent<Text>();
          _questionManager.errorDisplaySource.errorMessage = AddComponent<Text>();
          _questionManager.errorDisplaySource.errorMessageObject = new GameObject();
        }

        private void SetUpTimer()
        {
          GameObject timeManagerObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Timer/TimeManager"));
          _timer = timeManagerObj.GetComponent<Timer>();
          _questionManager.timer = _timer;
        }

        private void SetUpText()
        {
          _questionManager.SetQuestionText(AddComponent<TextMeshProUGUI>());
          SetUpErrorMessage();

              _questionManager.SetChoiceTexts(new List<TextMeshProUGUI>
          {
              AddComponent<TextMeshProUGUI>(),
              AddComponent<TextMeshProUGUI>()
          });
        }

        private void SetUpGameManager(AbstractWebRetriever webRetriever)
        {
          gameManagerObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/GameManager"));
          gameManager = gameManagerObj.GetComponent<GameManager>();
          gameManager.webRetriever = webRetriever;
          gameManager.gameEndRequestHelper = new GameEndRequestHelper(webRetriever, levelID);
          _questionManager.gameManager = gameManager;
        }

        private void SetUpPanels()
        {
          GameObject panel1Obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Questions/ChoicePanel1"));
          GameObject panel2Obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Questions/ChoicePanel2"));
          GameObject panel3Obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Questions/ChoicePanel3"));
          GameObject panel4Obj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Questions/ChoicePanel4"));
          choicePanels = new List<Image>{
            panel1Obj.GetComponent<Image>(),
            panel2Obj.GetComponent<Image>(),
            panel3Obj.GetComponent<Image>(),
            panel4Obj.GetComponent<Image>()
          };
          _questionManager.choicePanels = choicePanels;
        }

        [TearDown]
        public void TearDown()
        {
          foreach(GameObject obj in GameObject.FindObjectsOfType<GameObject>()) {
            GameObject.Destroy(obj);
          }
        }

        private T AddComponent<T>() where T : Component
        {
            var gameObject = new GameObject();
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
            yield return new WaitForSeconds(1f);
            Assert.That(!_uiGameObject.activeSelf);
        }

        [UnityTest, Order(4)]
        public IEnumerator ApiTimesOutResponseSoTheErrorAppears()
        {
            _questionManager.ClearQuestions();
            var errorMessageBefore = _questionManager.errorDisplaySource.errorMessage.text;
            _questionManager.webRetriever = AddComponent<TimeoutWebRetriever>();
            _questionManager.Start();
            yield return new WaitForSeconds(1f);
            Assert.AreNotEqual(errorMessageBefore,
                _questionManager.errorDisplaySource.errorMessage.text);
        }

        [UnityTest, Order(5)]
        public IEnumerator QuestionChangesRatherThanStayingTheSame()
        {
            var questionTextBefore = _questionManager.questionText.text;
            _questionManager.UserSelect(1);
            yield return new WaitForSeconds(1f);
            Assert.AreNotEqual(questionTextBefore, _questionManager.questionText.text);
        }

        [Retry(3)]
        [UnityTest, Order(6)]
        public IEnumerator UserSelectIncorrectDisplaysAsRed()
        {
            Color colorBefore = choicePanels[WrongChoiceIndex].color;
            _questionManager.UserSelect(WrongChoiceIndex);
            yield return new WaitForSeconds(0.23f);
            Color resultColor = choicePanels[WrongChoiceIndex].color;
            Color red = _questionManager.red;
            Assert.AreNotEqual(colorBefore, resultColor);
            Assert.AreEqual(colorBefore, choicePanels[RightChoiceIndex].color);
            Assert.AreEqual(red, resultColor);
        }

        [Retry(3)]
        [UnityTest, Order(7)]
        public IEnumerator UserSelectCorrectDisplaysAsGreen()
        {
            Color colorBefore = choicePanels[RightChoiceIndex].color;
            _questionManager.UserSelect(RightChoiceIndex);
            yield return new WaitForSeconds(0.23f);
            Color resultColor = choicePanels[RightChoiceIndex].color;
            Color green = _questionManager.green;
            Assert.AreNotEqual(colorBefore, resultColor);
            Assert.AreEqual(colorBefore, choicePanels[WrongChoiceIndex].color);
            Assert.AreEqual(green, resultColor);
        }
    }
}
