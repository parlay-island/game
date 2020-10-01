using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

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

        private GameObject uiGameObject;
        private QuestionManager questionManager;
        private Timer timer;
        
        public class MockWebRetriever : AbstractWebRetriever
        {
            public override Task<List<QuestionModel>> GetQuestions()
            {
                return new Task<List<QuestionModel>>(() => new List<QuestionModel>
                {
                    new QuestionModel(QuestionText, new List<ChoiceModel>
                        {
                            new ChoiceModel(RightChoice), 
                            new ChoiceModel(WrongChoice)
                        },
                        new List<int> { RightChoiceIndex })
                });
            }
        }

        [SetUp]
        public void SetUp()
        {
            uiGameObject = new GameObject();
            questionManager = Object.Instantiate(Resources.Load<GameObject>("Prefabs/QuestionManager"))
                .GetComponent<QuestionManager>();
            timer = Object.Instantiate(Resources.Load<GameObject>("Prefabs/TimeManager"))
                .GetComponent<Timer>();
            questionManager.webRetriever = AddComponent<MockWebRetriever>();
            questionManager.timer = timer;
            questionManager.questionUI = uiGameObject;
            questionManager.SetTimeReward(TimeReward);
            questionManager.SetQuestionText(AddComponent<Text>());
            questionManager.SetChoiceTexts(new List<Text>
            {
                AddComponent<Text>(),
                AddComponent<Text>()
            });
            questionManager.Start()
                .Wait();
        }

        private static T AddComponent<T>() where T : Component
        {
            return new GameObject().AddComponent<T>();
        }

        [Test]
        public void UserSelectAddsToTimerWhenQuestionIsCorrect()
        {
            var timeBefore = timer.getCurrTime();
            questionManager.UserSelect(RightChoiceIndex);
            Assert.Less(timeBefore, timer.getCurrTime());
        }

        [Test]
        public void UserSelectDoesntAddToTimerWhenQuestionIsIncorrect()
        {
            var timeBefore = timer.getCurrTime();
            questionManager.UserSelect(WrongChoiceIndex);
            Assert.AreEqual(timeBefore,timer.getCurrTime(), 0.001);
        }

        [Test]
        public void UserSelectTurnsUIInactive()
        {
            questionManager.UserSelect(RightChoiceIndex);
            Assert.That(!uiGameObject.activeSelf);
        }
    }
}
