using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests
{
    public class AwardText
    {
        private QuestionManager _questionManager;
        private GameObject questionManagerGameObject;
        private GameObject awardUI;
        private Text awardText;
        private Answered10QuestionsAward award;

        [SetUp]
        public void SetUp()
        {
            questionManagerGameObject = 
                MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/QuestionManager"));
            _questionManager = questionManagerGameObject.GetComponent<QuestionManager>();
            award = questionManagerGameObject.AddComponent<Answered10QuestionsAward>();
            awardText = questionManagerGameObject.AddComponent<Text>();
            awardUI = new GameObject();
            award.awardUI = awardUI;
            award.text = awardText;
            award.questionManager = _questionManager;
            _questionManager.webRetriever = questionManagerGameObject.AddComponent<QuestionManagerTest.MockWebRetriever>();
            _questionManager.timer = null;
            _questionManager.questionUI = awardUI;
            _questionManager.SetTimeReward(10);
            _questionManager.SetQuestionText(awardText);
            _questionManager.errorDisplaySource = questionManagerGameObject.AddComponent<ErrorDisplaySource>();
            _questionManager.errorDisplaySource.errorTitle = awardText;
            _questionManager.errorDisplaySource.errorMessage = awardText;
            _questionManager.errorDisplaySource.errorMessageObject = new GameObject();
        }

        [TearDown]
        public void TearDown()
        {
            GameObject.Destroy(questionManagerGameObject);
        }
        
        [UnityTest]
        public IEnumerator Answered10QuestionsAwardWinsAwardWhen10QuestionsAnswered()
        {
            for (int i = 0; i < 10; i++)
            {
                _questionManager.UserSelect(1);
            }

            yield return null;
            Assert.True(award.enabled);
            Assert.AreEqual(award.text.text, awardText.text);
        }
    }
}