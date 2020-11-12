using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
/**
* This class tests camera movement
*/

namespace Tests
{
    public class CameraTest
    {
        private GameObject testPlayer;
        private CharacterController2D characterController;
        private GameObject camera;
        private float distanceToTravelRight;
        private float distanceToTravelLeft;

        [SetUp]
        public void Setup()
        {
            testPlayer = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
            characterController = testPlayer.GetComponent<CharacterController2D>();
            camera = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/MainCamera"));
            distanceToTravelRight = 10f;
            distanceToTravelLeft = -10f;
        }

        [TearDown]
        public void Teardown()
        {
          foreach(GameObject obj in GameObject.FindObjectsOfType<GameObject>()) {
            GameObject.Destroy(obj);
          }
        }

        [Retry(3)]
        [UnityTest, Order(1)]
        public IEnumerator TestCameraMovementRight()
        {
            float initialXPos = camera.transform.position.x;
            characterController.Move(distanceToTravelRight, false);
            yield return new WaitForSeconds(1f);
            Assert.Greater(camera.transform.position.x, initialXPos);
        }

        [Retry(3)]
        [UnityTest, Order(2)]
        public IEnumerator TestCameraMovementLeft()
        {
            float initialXPos = camera.transform.position.x;
            characterController.Move(distanceToTravelLeft, false);
            yield return new WaitForSeconds(1f);
            Assert.Less(camera.transform.position.x, initialXPos);
        }
    }
}
