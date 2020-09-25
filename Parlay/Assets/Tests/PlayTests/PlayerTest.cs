using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
*/

namespace Tests
{
    public class PlayerTest
    {
        private GameObject testPlayer;
        private CharacterController2D characterController;
        private Animator animator;

        private const int directionReversal = -1;
        private float distanceTraveledLeft;
        private float runSpeed;

        [SetUp]
        public void Setup()
        {
            testPlayer = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
            characterController = testPlayer.GetComponent<CharacterController2D>();
            PlayerMovement playerMovement = testPlayer.GetComponent<PlayerMovement>();
            runSpeed = playerMovement.runSpeed;
            animator = testPlayer.GetComponent<Animator>();
            distanceTraveledLeft = Time.fixedDeltaTime * -1 * runSpeed;
        }

        [TearDown]
        public void Teardown()
        {
            GameObject.Destroy(testPlayer);
        }

        [UnityTest]
        public IEnumerator TestPlayerMovesLeftPosition()
        {
            float initialXPos = testPlayer.transform.position.x;
            characterController.Move(distanceTraveledLeft, false);
            yield return new WaitForSeconds(0.1f);
            Assert.Less(testPlayer.transform.position.x, initialXPos);
        }

        [UnityTest]
        public IEnumerator TestPlayerMovesLeftRotation()
        {
            float initalRotation = testPlayer.transform.localScale.x;
            characterController.Move(distanceTraveledLeft, false);
            Assert.AreEqual(testPlayer.transform.localScale.x, directionReversal * initalRotation);
            yield return null;
        }

        [UnityTest]
        public IEnumerator TestPlayerMovesRightPosition()
        {
            float initialXPos = testPlayer.transform.position.x;
            characterController.Move(distanceTraveledLeft * directionReversal, false);
            yield return new WaitForSeconds(0.1f);
            Assert.Greater(testPlayer.transform.position.x, initialXPos);
        }

        [UnityTest]
        public IEnumerator TestPlayerMovesRightRotation()
        {
            float initalRotation = testPlayer.transform.localScale.x;
            characterController.Move(distanceTraveledLeft * directionReversal, false);
            Assert.AreEqual(testPlayer.transform.localScale.x, initalRotation);
            yield return null;
        }

        [UnityTest]
        public IEnumerator TestPlayerRunsAnimation()
        {
            animator.SetFloat("Speed", Mathf.Abs(runSpeed));
            animator.Update(1);
            Assert.AreEqual("PlayerRun", animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
            yield return null;
        }

    }
}
