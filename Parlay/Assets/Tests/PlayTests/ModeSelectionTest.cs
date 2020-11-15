using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
using TMPro;

/**
* This file tests mode selection
* 
* @author: Holly Ansel
*/

namespace Tests
{
    public class ModeSelectionTest
    {
        private const string level1Name = "test1";
        private const string level2Name = "test2";
        private const string level3Name = "test3";

        public class MockModeRetriever : AbstractModeRetriever {
            public override List<LevelModel> GetLevels()
            {
              return new List<LevelModel>{
                new LevelModel(1, level1Name),
                new LevelModel(2, level2Name),
                new LevelModel(3, level3Name)
              };
            }
            public override bool IsLoading()
            {
              return false;
            }
        }

        public class ErrorModeRetriever : AbstractModeRetriever {
            public override List<LevelModel> GetLevels()
            {
              return null;
            }
            public override bool IsLoading()
            {
              return false;
            }
        }

        private GameObject mockModeRetrieverObj;
        private LevelSelector levelSelector;
        private GameObject canvas;
        private GameObject panel;
        private GameObject levelObj;

        [SetUp]
        public void Setup()
        {
          levelObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/LevelObj"));
          levelObj.name = "LevelObj";
          canvas = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/ModeSelection/Canvas"));
          panel = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/ModeSelection/Panel"));

          mockModeRetrieverObj = new GameObject();
          mockModeRetrieverObj.AddComponent<MockModeRetriever>();
          mockModeRetrieverObj.name = "ModeRetriever";

          levelSelector = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/ModeSelection/MainCamera")).GetComponent<LevelSelector>();
          levelSelector.thisCanvas = canvas;
          levelSelector.levelHolder = panel;
        }

        [TearDown]
        public void Teardown()
        {
            foreach(GameObject obj in GameObject.FindObjectsOfType<GameObject>()) {
              GameObject.Destroy(obj);
            }
        }

        [UnityTest, Order(1)]
        public IEnumerator TestModesDisplay()
        {
          GameObject[] levelButtons = GameObject.FindGameObjectsWithTag("LevelButton");
          GameObject level1 = levelButtons[0];
          GameObject level2 = levelButtons[1];
          GameObject level3 = levelButtons[2];
          Assert.AreEqual(level1.GetComponentInChildren<TextMeshProUGUI>().text, level3Name);
          Assert.AreEqual(level2.GetComponentInChildren<TextMeshProUGUI>().text, level2Name);
          Assert.AreEqual(level3.GetComponentInChildren<TextMeshProUGUI>().text, level1Name);

          yield return null;
        }

        [UnityTest, Order(2)]
        public IEnumerator TestModeRetrieverError()
        {
          mockModeRetrieverObj.name = "ModeRetriever2";
          levelSelector.errorDisplaySource.errorMessage.text = "";
          var errorMessageBefore = levelSelector.errorDisplaySource.errorMessage.text;
          GameObject retriever = new GameObject();
          ErrorModeRetriever mockErrorRetriever = retriever.AddComponent<ErrorModeRetriever>();
          mockErrorRetriever.name = "ModeRetriever";
          levelSelector.Start();
          Assert.AreNotEqual(errorMessageBefore,
              levelSelector.errorDisplaySource.errorMessage.text);
          GameObject.Destroy(retriever);
          yield return null;

        }

        [UnityTest, Order(3)]
        public IEnumerator TestModeSelectedPropagatesToGame()
        {
            Level lev = levelObj.GetComponent<Level>();
            Assert.Throws<NullReferenceException>(() => lev.GetName());
            LevelButton button = GameObject.Find(level2Name).GetComponent<LevelButton>();
            button.isTest = true;
            button.LoadGame();
            yield return new WaitForSeconds(1);
            Assert.DoesNotThrow(() => lev.GetName());
            Assert.AreEqual(lev.GetName(), level2Name);
            Assert.AreEqual(lev.GetId(), 2);
        }

    }
}
