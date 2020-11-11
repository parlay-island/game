using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class RulesTest
    {
        private GameObject testObject;
        private Rules rules;
		private Scene testScene;

		[OneTimeSetUp]
        public void Init()
        {
          testScene = SceneManager.GetActiveScene();
        }

        [SetUp]
        public void Setup() {
            testObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Rules/RulesManager"));
            rules = testObject.GetComponent<Rules>();
			rules.isTest = true;
        }

        [TearDown]
        public void Teardown()
        {
          foreach(GameObject obj in GameObject.FindObjectsOfType<GameObject>()) {
            GameObject.Destroy(obj);
          }
        }

        [UnityTest, Order(1)]
        public IEnumerator GoesToNextRulesScreen()
        {
            rules.goToRulesOrModeScreen("RulesScreen2");
            yield return new WaitForSeconds(1f);
            Scene ruleScene2 = SceneManager.GetActiveScene();
            Assert.AreEqual("RulesScreen2", ruleScene2.name);
			SceneManager.SetActiveScene(testScene);
            SceneManager.UnloadSceneAsync(ruleScene2);
            yield return new WaitForSeconds(1f);
        }

        [UnityTest, Order(2)]
        public IEnumerator GoesToFirstRulesScreen()
        {
            rules.goToRulesOrModeScreen("RulesScreen");
            yield return new WaitForSeconds(1f);
            Scene ruleScene = SceneManager.GetActiveScene();
            Assert.AreEqual("RulesScreen", ruleScene.name);
			SceneManager.SetActiveScene(testScene);
            SceneManager.UnloadSceneAsync(ruleScene);
            yield return new WaitForSeconds(1f);
        }

        [UnityTest, Order(3)]
        public IEnumerator GoesBackToModeScreen()
        {
            rules.goToRulesOrModeScreen("ModeSelection");
            yield return new WaitForSeconds(1f);
            Scene modeScene = SceneManager.GetActiveScene();
            Assert.AreEqual("ModeSelection", modeScene.name);
			SceneManager.SetActiveScene(testScene);
            SceneManager.UnloadSceneAsync(modeScene);
            yield return new WaitForSeconds(1f);
        }


    }
}
