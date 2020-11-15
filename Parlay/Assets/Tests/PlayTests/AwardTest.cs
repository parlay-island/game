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
* @author: Jake Derry
*/
namespace Tests
{
    public class AwardText
    {
        private QuestionManager _questionManager;
        private GameObject questionManagerGameObject;
        private GameObject awardUI;
        private TextMeshProUGUI awardText;
        private Answered10QuestionsAward award;
        private GameManager gameManager;
        private GameObject gameManagerObj;
        private List<GameObject> _questionManagerGameObjectList;

        [SetUp]
        public void SetUp()
        {
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

            gameManagerObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/GameManager"));
            gameManager = gameManagerObj.GetComponent<GameManager>();
            QuestionManagerTest.MockWebRetriever webRetriever = AddComponent<QuestionManagerTest.MockWebRetriever>();
            gameManager.webRetriever = webRetriever;
            gameManager.gameEndRequestHelper = new GameEndRequestHelper(webRetriever, 1);
            gameManager.setGameTime(30f);
            _questionManager.gameManager = gameManager;
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
          foreach(GameObject obj in GameObject.FindObjectsOfType<GameObject>()) {
            GameObject.Destroy(obj);
          }
        }

        [Retry (2)]
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
