using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class DistanceIntegrationTest
    {
        private GameObject testObject;
        private GameObject testPlayer;
        private GameObject testGround;

        private GameManager gameManager;
        private GameObject player;
        private CharacterController2D characterController; 
        private PlayerMovement playerMovement;
        private Rigidbody2D rigidbody;

        private const int directionReversal = -1;
        private float runSpeed;
        private float distanceTraveled;

        [SetUp]
        public void Setup()
        {
            testObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/GameManager"));
            testPlayer = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));

            gameManager = testObject.GetComponent<GameManager>();
            gameManager.setGameTime(2f);
            gameManager.player = testPlayer;

            playerMovement = testPlayer.GetComponent<PlayerMovement>();
            characterController = testPlayer.GetComponent<CharacterController2D>();
            runSpeed = playerMovement.runSpeed;
            distanceTraveled = Time.fixedDeltaTime * -1 * runSpeed;
            rigidbody = testPlayer.GetComponent<Rigidbody2D>();
            rigidbody.gravityScale = 0;
        }

        [TearDown]
        public void Teardown()
        {
            GameObject.Destroy(testObject);
            GameObject.Destroy(testPlayer);
        }

        [UnityTest]
        public IEnumerator TestDistanceTrackingWhileGameIsNotOver() {
            // test that distance tracking label is showing 
            Assert.IsFalse(gameManager.finalDistanceText.gameObject.activeSelf);

            // test that distance tracking label displays the correct distance when player moves left
            characterController.Move(distanceTraveled * directionReversal, false);
            yield return new WaitForSeconds(0.1f);
            Assert.AreEqual(gameManager.distanceText.text, "Distance: " + playerMovement.getDistanceTravelled().ToString("F2"));

            // test that distance tracking label displays the correct distance when player moves right
            characterController.Move(distanceTraveled, false);
            yield return new WaitForSeconds(0.1f);
            Assert.AreEqual(gameManager.distanceText.text, "Distance: " + playerMovement.getDistanceTravelled().ToString("F2"));

            // test that distance tracking label displays the correct distance when player jumps
            rigidbody.gravityScale = 3;
            characterController.Move(0f, true);
            yield return new WaitForSeconds(0.1f);
            Assert.AreEqual(gameManager.distanceText.text, "Distance: " + playerMovement.getDistanceTravelled().ToString("F2"));
        }

        [UnityTest]
        public IEnumerator TestFinalDistanceDisplayWhenGameIsOver() {
            // test that the final distance is reported
            yield return new WaitForSeconds(3f);
            Assert.IsTrue(gameManager.finalDistanceText.gameObject.activeSelf);
            Assert.AreEqual(gameManager.finalDistanceText.text, "You have travelled " + playerMovement.getDistanceTravelled().ToString("F2") + "m");
        }
    }
}
