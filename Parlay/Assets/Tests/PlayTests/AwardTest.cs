using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using TMPro;

/**
* This file tests awards
* 
* @author: Jake Derry, Andres Montoya
*/
namespace Tests
{
    public class AwardText
    {
        private QuestionManager _questionManager;
        private GameObject questionManagerGameObject;
        private GameObject awardUI;
        private GameObject medal;
        private TextMeshProUGUI awardText;
        private Answered10QuestionsAward award;
        private Answered20QuestionsAward award20;
        private Answered30QuestionsAward award30;
        private GameManager gameManager;
        private GameObject gameManagerObj;
        private GameObject awardObject;
        private AwardManager awardManager;
        private List<GameObject> _questionManagerGameObjectList;

        [SetUp]
        public void SetUp()
        {
            _questionManagerGameObjectList = new List<GameObject>();
            questionManagerGameObject =
                MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Questions/QuestionManager"));
            _questionManager = questionManagerGameObject.GetComponent<QuestionManager>();
            awardText = questionManagerGameObject.AddComponent<TextMeshProUGUI>();
            awardUI = new GameObject();
            medal = new GameObject();

            award = questionManagerGameObject.AddComponent<Answered10QuestionsAward>();
            award.awardUI = awardUI;
            award.medal = medal;
            award.text = awardText;
            award.questionManager = _questionManager;

            award20 = questionManagerGameObject.AddComponent<Answered20QuestionsAward>();
            award20.awardUI = awardUI;
            award20.medal = medal;
            award20.text = awardText;
            award20.questionManager = _questionManager;

            award30 = questionManagerGameObject.AddComponent<Answered30QuestionsAward>();
            award30.awardUI = awardUI;
            award30.medal = medal;
            award30.text = awardText;
            award30.questionManager = _questionManager; 

            _questionManager.webRetriever = questionManagerGameObject.AddComponent<QuestionManagerTest.MockWebRetriever>();
            _questionManager.timer = null;
            _questionManager.questionUI = awardUI;
            _questionManager.SetTimeReward(10);
            _questionManager.SetQuestionText(AddComponent<TextMeshProUGUI>());
            _questionManager.errorDisplaySource = questionManagerGameObject.AddComponent<ErrorDisplaySource>();
            _questionManager.errorDisplaySource.errorTitle = AddComponent<Text>();
            _questionManager.errorDisplaySource.errorMessage = AddComponent<Text>();
            _questionManager.errorDisplaySource.errorMessageObject = new GameObject();

            gameManagerObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/GameManager"));
            gameManager = gameManagerObj.GetComponent<GameManager>();
            QuestionManagerTest.MockWebRetriever webRetriever = AddComponent<QuestionManagerTest.MockWebRetriever>();
            gameManager.webRetriever = webRetriever;
            gameManager.gameEndRequestHelper = new GameEndRequestHelper(webRetriever, 1);
            gameManager.setGameTime(30f);
            _questionManager.gameManager = gameManager;
            awardObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Award Prefabs/AwardManager"));
            awardManager = awardObject.GetComponent<AwardManager>();
            awardManager.award_list = new List<Award>();
            _questionManager.awardManager = awardManager;

        }

        private T AddComponent<T>() where T : Component
        {
            var gameObject = new GameObject();
            _questionManagerGameObjectList.Add(gameObject);
            return gameObject.AddComponent<T>();
        }

        [TearDown]
        public void TearDown()
        {
          GameObject.Destroy(awardObject);
          foreach(GameObject obj in GameObject.FindObjectsOfType<GameObject>()) {
            GameObject.Destroy(obj);
          }
        }

        [Retry (2)]
        [UnityTest, Order(1)]
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

        [Retry(2)]
        [UnityTest, Order(2)]
        public IEnumerator Answered20QuestionsAwardWinsAwardWhen20QuestionsAnswered()
        {
            for (int i = 0; i < 20; i++)
            {
                _questionManager.UserSelect(1);
            }

            yield return null;
            Assert.True(award20.enabled);
            Assert.AreEqual(award20.text.text, awardText.text);
        }

        [Retry(2)]
        [UnityTest, Order(3)]
        public IEnumerator Answered30QuestionsAwardWinsAwardWhen30QuestionsAnswered()
        {
            for (int i = 0; i < 30; i++)
            {
                _questionManager.UserSelect(1);
            }

            yield return null;
            Assert.True(award30.enabled);
            Assert.AreEqual(award30.text.text, awardText.text);
        }

        [Retry(2)]
        [UnityTest, Order(4)]
        public IEnumerator TopAwardCalculation()
        {
            awardManager.award_list.Clear();
            awardManager.award_list.Add(award);
            awardManager.award_list.Add(award20);
            awardManager.award_list.Add(award30);
            for (int i = 0; i < 30; i++)
            {
                _questionManager.UserSelect(1);
            }

            yield return null;
            Assert.True(awardManager.top_award.Count >= 1);
        }
    }
}
