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


        [SetUp]
        public void Setup() {
            testObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Rules/RulesManager"));
            rules = testObject.GetComponent<Rules>();
        }
        
        [TearDown]
        public void Teardown()
        {
            GameObject.Destroy(testObject);
        }
        
        [UnityTest, Order(1)]
        public IEnumerator GoesToNextRulesScreen()
        {
            rules.goToRulesScreen("RulesScreen2");
            yield return new WaitForSeconds(1f);
            Scene ruleScene2 = SceneManager.GetActiveScene();
            Assert.AreEqual("RulesScreen2", ruleScene2.name);
        }
        
        [UnityTest, Order(2)]
        public IEnumerator GoesToFirstRulesScreen()
        {
            rules.goToRulesScreen("RulesScreen");
            yield return new WaitForSeconds(1f);
            Scene ruleScene2 = SceneManager.GetActiveScene();
            Assert.AreEqual("RulesScreen", ruleScene2.name);
        }
        
        [UnityTest, Order(3)]
        public IEnumerator GoesBackToModeScreen()
        {
            rules.backToModeSelection();
            yield return new WaitForSeconds(1f);
            Scene modeScene = SceneManager.GetActiveScene();
            Assert.AreEqual("ModeSelection", modeScene.name);
        }

        
    }
}
