using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/**
* This class tests the player prefab (which contains the characterController
* and playerMovement scripts)
* Unity does not have the functionality to simulate a keypress
* Therefore this class is testing that the animator and the CharacterController2D
* respond appropriately with the input that would be given to them if
* the keys were pressed
* 
* @author: Holly Ansel
*/

namespace Tests
{
    public class PlayerTest
    {
        private GameObject testPlayer;
        private CharacterController2D characterController;
        private Animator animator;
        private GameObject testGround;
        private Rigidbody2D rigidbody;
        private PlayerMovement playerMovement;

        private const int directionReversal = -1;
        private float distanceTraveledLeft;
        private float runSpeed;

        [SetUp]
        public void Setup()
        {
            testPlayer = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
            characterController = testPlayer.GetComponent<CharacterController2D>();
            playerMovement = testPlayer.GetComponent<PlayerMovement>();
            runSpeed = playerMovement.runSpeed;
            animator = testPlayer.GetComponent<Animator>();
            distanceTraveledLeft = Time.fixedDeltaTime * -1 * runSpeed;
            rigidbody = testPlayer.GetComponent<Rigidbody2D>();
            rigidbody.gravityScale = 0;
            playerMovement.questionUI.gameObject.SetActive(false);
        }

        [TearDown]
        public void Teardown()
        {
            playerMovement.questionUI.gameObject.SetActive(false);
            foreach(GameObject obj in GameObject.FindObjectsOfType<GameObject>()) {
              GameObject.Destroy(obj);
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


        [UnityTest, Order(1)]
        public IEnumerator TestPlayerMovesLeftPosition()
        {
            float initialXPos = testPlayer.transform.position.x;
            characterController.Move(distanceTraveledLeft, false);
            yield return new WaitForSeconds(0.1f);
            Assert.Less(testPlayer.transform.position.x, initialXPos);
        }

        [UnityTest, Order(2)]
        public IEnumerator TestPlayerMovesLeftRotation()
        {
            float initalRotation = testPlayer.transform.localScale.x;
            characterController.Move(distanceTraveledLeft, false);
            Assert.AreEqual(testPlayer.transform.localScale.x, directionReversal * initalRotation);
            yield return null;
        }

        [UnityTest, Order(3)]
        public IEnumerator TestPlayerMovesRightPosition()
        {
            float initialXPos = testPlayer.transform.position.x;
            characterController.Move(distanceTraveledLeft * directionReversal, false);
            yield return new WaitForSeconds(0.1f);
            Assert.Greater(testPlayer.transform.position.x, initialXPos);
        }

        [UnityTest, Order(4)]
        public IEnumerator TestPlayerMovesRightRotation()
        {
            float initalRotation = testPlayer.transform.localScale.x;
            characterController.Move(distanceTraveledLeft * directionReversal, false);
            Assert.AreEqual(testPlayer.transform.localScale.x, initalRotation);
            yield return null;
        }

        [UnityTest, Order(5)]
        public IEnumerator TestPlayerRunsAnimation()
        {
            animator.SetFloat("Speed", Mathf.Abs(runSpeed));
            animator.Update(1);
            Assert.AreEqual("PlayerRun", animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
            yield return null;
        }

        [UnityTest, Order(6)]
        public IEnumerator TestPlayerCantJump()
        {
            Assert.False(characterController.CanJump());
            yield return null;
        }

        [UnityTest, Order(7)]
        public IEnumerator TestPlayerCanJump()
        {
            CreateGround();
            yield return new WaitForSeconds(0.2f);
            Assert.True(characterController.CanJump());
        }

        [UnityTest, Order(8)]
        public IEnumerator TestPlayerJump()
        {
          CreateGround();

          yield return new WaitForSeconds(0.2f);
          rigidbody.gravityScale = 3;
          float initialYPos = testPlayer.transform.localPosition.y;
          Assert.True(characterController.CanJump());
          characterController.Move(0f, true);
          yield return new WaitForSeconds(0.1f);
          Assert.False(characterController.CanJump());
          Assert.Greater(testPlayer.transform.localPosition.y, initialYPos);
        }

        [UnityTest, Order(9)]
        public IEnumerator TestPlayerJumpNotAllowed()
        {
          float initialYPos = testPlayer.transform.position.y;
          Assert.False(characterController.CanJump());
          characterController.Move(0f, true);
          yield return new WaitForSeconds(0.1f);
          Assert.AreEqual(testPlayer.transform.position.y, initialYPos);
        }

        [UnityTest, Order(10)]
        public IEnumerator TestPlayerJumpAnimation()
        {
          animator.SetBool("IsJumping", true);
          animator.Update(1);
          Assert.AreEqual("PlayerJump", animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
          yield return null;
        }

        [UnityTest, Order(11)]
        public IEnumerator TestDistanceIncreasesWhenPlayerMovesRight()
        {
            float initialXPos = testPlayer.transform.position.x;
            Assert.AreEqual(playerMovement.getDistanceTravelled(), 0f);

            characterController.Move(distanceTraveledLeft * directionReversal, false);
            yield return new WaitForSeconds(0.1f);

            Assert.Greater(playerMovement.getDistanceTravelled(), initialXPos);
        }

        [UnityTest, Order(12)]
        public IEnumerator TestDistanceDecreasesWhenPlayerMovesLeft()
        {
            float initialXPos = testPlayer.transform.position.x;
            Assert.AreEqual(playerMovement.getDistanceTravelled(), 0f);

            characterController.Move(distanceTraveledLeft, false);
            yield return new WaitForSeconds(0.1f);

            Assert.Less(playerMovement.getDistanceTravelled(), initialXPos);
        }

        [UnityTest, Order(13)]
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
