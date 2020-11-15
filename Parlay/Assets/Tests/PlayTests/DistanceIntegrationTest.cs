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

        private GameManager gameManager;
        private GameObject player;
        private CharacterController2D characterController;
        private PlayerMovement playerMovement;
        private Rigidbody2D rigidbody;
        private GameObject mockWebRetrieverObj;
        private GameObject awardObject;
        private AwardManager awardManager;

        private const int directionReversal = -1;
        private const float deltaDistance = 0.2f;
        private float runSpeed;
        private float distanceTraveled;

        public class MockWebRetriever : AbstractWebRetriever
        {
            public override void SetUp(string auth_token, int level){
            }
            public override List<QuestionModel> GetQuestions() {
                return new List<QuestionModel>();
            }

            public override void PostEndResult(EndResult result, int playerID) {
            }

            public override string GetMostRecentPostRequestResult() {
              return "";
            }
            public override void FetchResults() {
            }
            public override List<ResultModel> GetMostRecentResults() {
              return new List<ResultModel>();
            }
            public override bool IsLoading()
            {
              return false;
            }
        }

        [SetUp]
        public void Setup()
        {
            initializeGameManagerAndPlayer();
        }

        private void initializeGameManagerAndPlayer() {
            testObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/GameManager"));
            testPlayer = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));


            gameManager = testObject.GetComponent<GameManager>();
            mockWebRetrieverObj = new GameObject();
            MockWebRetriever mockWebRetriever = mockWebRetrieverObj.AddComponent<MockWebRetriever>();
            gameManager.webRetriever = mockWebRetriever;
            gameManager.setGameTime(10f);
            gameManager.player = testPlayer;

            awardObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Award Prefabs/AwardManager"));
            awardManager = awardObject.GetComponent<AwardManager>();
            awardManager.award_list = new List<Award>();
            gameManager.awardManager = awardManager;


            playerMovement = testPlayer.GetComponent<PlayerMovement>();
            characterController = testPlayer.GetComponent<CharacterController2D>();
            runSpeed = playerMovement.runSpeed;
            distanceTraveled = Time.fixedDeltaTime * -1 * runSpeed;
            rigidbody = testPlayer.GetComponent<Rigidbody2D>();
            rigidbody.gravityScale = 0;
            playerMovement.questionUI.gameObject.SetActive(false);
        }

        [TearDown]
        public void Teardown()
        {
            playerMovement.questionUI.gameObject.SetActive(false);
            GameObject.Destroy(awardObject);
            foreach(GameObject obj in GameObject.FindObjectsOfType<GameObject>()) {
              GameObject.Destroy(obj);
            }
        }

        [Retry(4)]
        [UnityTest]
        public IEnumerator TestDistanceTrackingWhileGameIsNotOver() {
            // test that distance tracking label is showing
            Assert.IsFalse(gameManager.finalDistanceText.gameObject.activeSelf);

            // test that distance tracking label displays the correct distance when player moves right
            characterController.Move(distanceTraveled * directionReversal, false);
            yield return new WaitForSeconds(1f);
            Assert.That(findDifferenceBetweenActualAndDisplayedDifference(false) < deltaDistance);

            // test that distance tracking label displays the correct distance when player moves left
            characterController.Move(distanceTraveled, false);
            yield return new WaitForSeconds(1f);
            Assert.That(findDifferenceBetweenActualAndDisplayedDifference(false) < deltaDistance);

            // test that distance tracking label displays the correct distance when player jumps
            rigidbody.gravityScale = 3;
            characterController.Move(0f, true);
            yield return new WaitForSeconds(1f);
            Assert.That(findDifferenceBetweenActualAndDisplayedDifference(false) < deltaDistance);

            // test that the final distance is reported
            gameManager.gameOver();
            Assert.IsTrue(gameManager.finalDistanceText.gameObject.activeSelf);
            Assert.That(findDifferenceBetweenActualAndDisplayedDifference(true) < deltaDistance);
        }

        private float findDifferenceBetweenActualAndDisplayedDifference(bool isFinalDistance) {
            float actualPlayerDistance = playerMovement.getDistanceTravelled();
            float displayedPlayerDistance = getFloatDistanceFromDistanceText(gameManager.distanceText.text);
            if (isFinalDistance) {
                displayedPlayerDistance = getFloatDistanceFromFinalDistanceText(gameManager.finalDistanceText.text);
            }

            return Mathf.Abs(displayedPlayerDistance - actualPlayerDistance);
        }

        private float getFloatDistanceFromDistanceText(string text) {
            string[] parts = text.Split(':');
            float distance = float.Parse(parts[1].Trim());
            return distance;
        }

        private float getFloatDistanceFromFinalDistanceText(string finalDistanceText) {
            string[] parts = finalDistanceText.Split(' ');
            float distance = float.Parse(parts[3]);
            return distance;
        }
    }
}
