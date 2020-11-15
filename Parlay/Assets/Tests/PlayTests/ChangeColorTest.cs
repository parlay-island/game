using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.TestTools;

/**
* This file tests color changes for a button
* 
* @author: Andres Montoya
*/

namespace Tests
{
    public class ChangeColorTest
    {
        private ChangeColor changeColor;
        private GameManager gameManager;
        private GameObject gameManagerObj;

        [SetUp]
        public void Setup()
        {
            gameManagerObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/GameManager"));
            gameManager = gameManagerObj.GetComponent<GameManager>();
            gameManager.setGameTime(30f);
            changeColor = AddComponent<ChangeColor>();
            changeColor.button = AddComponent<Button>();
            changeColor.image = AddComponent<Image>();
            changeColor.text = AddComponent<TextMeshProUGUI>();
        }

        [TearDown]
        public void Teardown()
        {
            foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
            {
                GameObject.Destroy(obj);
            }
        }

        private T AddComponent<T>() where T : Component
        {
            var gameObject = new GameObject();
            return gameObject.AddComponent<T>();
        }

        [UnityTest, Order(1)]
        public IEnumerator testClear()
        {
            GameManager.instance.retries.Clear();
            yield return new WaitForSeconds(2);
            Assert.True(changeColor.image.color == Color.clear);
        }

        [UnityTest, Order(2)]
        public IEnumerator testGreen()
        {
            GameManager.instance.retries.Clear();
            GameManager.instance.retries.Add(1);
            GameManager.instance.canRetry = true;
            yield return new WaitForSeconds(2);
            Assert.True(changeColor.image.color == Color.green);
        }

        [UnityTest, Order(3)]
        public IEnumerator testWhite()
        {
            GameManager.instance.retries.Clear();
            GameManager.instance.retries.Add(1);
            GameManager.instance.canRetry = false;
            yield return new WaitForSeconds(2);
            Assert.True(changeColor.image.color == Color.white);
        }
    }
}
