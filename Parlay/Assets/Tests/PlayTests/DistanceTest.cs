using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class DistanceTest
    {
        private GameObject testPlayer;
        private CharacterController2D characterController;
        private PlayerMovement playerMovement;
        private GameObject testGround;
        private Rigidbody2D rigidbody;

        private const int directionReversal = -1;
        private float distanceTraveled;
        private float runSpeed;

        [SetUp]
        public void Setup() {
            testPlayer = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
            characterController = testPlayer.GetComponent<CharacterController2D>();
            playerMovement = testPlayer.GetComponent<PlayerMovement>();
            runSpeed = playerMovement.runSpeed;
            distanceTraveled = Time.fixedDeltaTime * -1 * runSpeed;
            rigidbody = testPlayer.GetComponent<Rigidbody2D>();
            rigidbody.gravityScale = 0;
        }

        [TearDown]
        public void Teardown()
        {
            GameObject.Destroy(testPlayer);
            if (testGround != null)
            {
              GameObject.Destroy(testGround.GetComponent<BoxCollider2D>());
              GameObject.Destroy(testGround);
            }
        }

        private void CreateGround()
        {
          testGround = new GameObject("ground");
          testGround.layer = 0;
          BoxCollider2D groundCollider = testPlayer.GetComponent<BoxCollider2D>();
          Vector3 groundCheckPosition = groundCollider.bounds.center;
          groundCheckPosition.y = groundCollider.bounds.center.y - Mathf.Abs(groundCollider.bounds.center.y / 2);
          testGround.transform.position = groundCheckPosition;
          BoxCollider2D testGroundCollider = testGround.AddComponent<BoxCollider2D>();
        }

        [UnityTest]
        public IEnumerator TestDistanceIncreasesWhenPlayerMovesRight()
        {
            float initialXPos = testPlayer.transform.position.x;
            Assert.AreEqual(playerMovement.getDistanceTravelled(), 0f);

            characterController.Move(distanceTraveled * directionReversal, false);
            yield return new WaitForSeconds(0.1f);
            
            Assert.Greater(playerMovement.getDistanceTravelled(), initialXPos);
        }

        [UnityTest]
        public IEnumerator TestDistanceDecreasesWhenPlayerMovesLeft()
        {
            float initialXPos = testPlayer.transform.position.x;
            Assert.AreEqual(playerMovement.getDistanceTravelled(), 0f);

            characterController.Move(distanceTraveled, false);
            yield return new WaitForSeconds(0.1f);

            Assert.Less(playerMovement.getDistanceTravelled(), initialXPos);
        }

        [UnityTest]
        public IEnumerator TestDistanceDoesNotChangeWhenPlayerJumps()
        {
            CreateGround();

            yield return new WaitForSeconds(0.2f);
            rigidbody.gravityScale = 3;
            float initialYPos = testPlayer.transform.localPosition.y;
            float distanceBeforeJumping = playerMovement.getDistanceTravelled();
            Assert.AreEqual(distanceBeforeJumping, 0f);
            
            characterController.Move(0f, true);
            yield return new WaitForSeconds(0.1f);
            float distanceAfterJumping = playerMovement.getDistanceTravelled();
            
            // distance should not increase when jumping
            Assert.AreEqual(distanceAfterJumping, 0f);
        }
    }
}
