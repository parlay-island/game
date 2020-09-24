using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

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
        public IEnumerator testPlayerMovesLeftPosition()
        {
            float initialXPos = testPlayer.transform.position.x;
            characterController.Move(distanceTraveledLeft, false);
            yield return new WaitForSeconds(0.1f);
            Assert.Less(testPlayer.transform.position.x, initialXPos);
        }

        [UnityTest]
        public IEnumerator testPlayerMovesLeftRotation()
        {
            float initalRotation = testPlayer.transform.localScale.x;
            characterController.Move(distanceTraveledLeft, false);
            Assert.AreEqual(testPlayer.transform.localScale.x, directionReversal * initalRotation);
            yield return null;
        }

        [UnityTest]
        public IEnumerator testPlayerMovesRightPosition()
        {
            float initialXPos = testPlayer.transform.position.x;
            characterController.Move(distanceTraveledLeft * directionReversal, false);
            yield return new WaitForSeconds(0.1f);
            Assert.Greater(testPlayer.transform.position.x, initialXPos);
        }

        [UnityTest]
        public IEnumerator testPlayerMovesRightRotation()
        {
            float initalRotation = testPlayer.transform.localScale.x;
            characterController.Move(distanceTraveledLeft * directionReversal, false);
            Assert.AreEqual(testPlayer.transform.localScale.x, initalRotation);
            yield return null;
        }

        [UnityTest]
        public IEnumerator testPlayerRunsAnimation()
        {
            animator.SetFloat("Speed", Mathf.Abs(runSpeed));
            animator.Update(1);
            Assert.AreEqual("PlayerRun", animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
            yield return null;
        }

    }
}
