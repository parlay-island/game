using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/**
* This file tests the parallax effect
* 
* @author: Andres Montoya
*/

namespace Tests
{
    public class ParallaxTests
    {
        private GameObject camera;
        private GameObject cloudsObj;
        private GameObject mountainsObj;
        private GameObject treesObj;
        private GameObject testPlayer;

        [SetUp]
        public void Setup()
        {
            testPlayer = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
            cloudsObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Clouds"));
            mountainsObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Mountains"));
            treesObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Trees"));
            camera = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/MainCamera"));
        }

        [TearDown]
        public void Teardown()
        {
          foreach(GameObject obj in GameObject.FindObjectsOfType<GameObject>()) {
            GameObject.Destroy(obj);
          }
        }

        [UnityTest, Order(1)]
        public IEnumerator RefittingTest()
        {
            float cloud_x = cloudsObj.transform.position.x;
            float mountain_x = mountainsObj.transform.position.x;
            float trees_x = treesObj.transform.position.x;
            testPlayer.transform.position += new Vector3(2000, 0);
            yield return new WaitForSeconds(0.5f);
            Assert.IsTrue(cloudsObj.transform.position.x > cloud_x);
            Assert.IsTrue(mountainsObj.transform.position.x > mountain_x);
            Assert.IsTrue(treesObj.transform.position.x > trees_x);
            yield return null;
        }

        [UnityTest, Order(2)]
        public IEnumerator ParallaxTest()
        {
            testPlayer.transform.position += new Vector3(20, 0);
            yield return new WaitForSeconds(0.5f);
            Assert.IsTrue(cloudsObj.transform.position.x > mountainsObj.transform.position.x);
            Assert.IsTrue(mountainsObj.transform.position.x > treesObj.transform.position.x);
            yield return null;
        }
    }
}
