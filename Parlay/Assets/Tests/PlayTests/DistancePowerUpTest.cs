using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;

/**
* This file tests the distance power up
* 
* @author: Andres Montoya
*/

namespace Tests
{

    public class DistancePowerUpTest
    {
        private GameObject player;
        private CharacterController2D characterController;
        private GameObject powerUp;
        private GameManager gameManager;
        private GameObject gameManagerObj;
        private float bonusDistance;
        private Text powerupText;

        [SetUp]
        public void Setup()
        {
            gameManagerObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/GameManager"));
            gameManager = gameManagerObj.GetComponent<GameManager>();
            player = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
            characterController = player.GetComponent<CharacterController2D>();
            player.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
            gameManager.setGameTime(30f);
            gameManager.questionUI.gameObject.SetActive(false);

            powerupText = MonoBehaviour.Instantiate(Resources.Load<Text>("Prefabs/PowerUpLabel"));
            gameManager.powerUpText = powerupText;
            GameManager.instance.powerUpText = powerupText;

            characterController.Move(10f, false);
            foreach (GameObject question in GameObject.FindGameObjectsWithTag("Question"))
            {
                GameObject.Destroy(question);
            }
            bonusDistance = gameManager.bonusDistance;
        }

        [TearDown]
        public void Teardown()
        {
            gameManager.questionUI.gameObject.SetActive(false);
            foreach(GameObject obj in GameObject.FindObjectsOfType<GameObject>()) {
              GameObject.Destroy(obj);
            }
        }

        private Vector2 GetPowerUpColliderPos()
        {
            return powerUp.transform.localPosition - (powerUp.GetComponent<SpriteRenderer>().bounds.extents / 2);
        }

        private void CollideWithPowerUp()
        {
            Vector2 powerUpPos = GetPowerUpColliderPos();
            characterController.Move(powerUpPos.x, false);
        }


        [Retry(4)]
        [UnityTest, Order(1)]
        public IEnumerator TestDistancePowerUpActivation()
        {
            powerUp = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Terrain Prefabs/Interactible Tiles/ChestTile1"));
            powerUp.GetComponent<PowerUp>().type = 2;
            CollideWithPowerUp();
            yield return new WaitForSeconds(2);
            //Test if time was increased
            Assert.True(gameManager.bonusDistance > 0);
            Assert.True(gameManager.playerDistance > player.GetComponent<PlayerMovement>().getDistanceTravelled());
        }

    }
}
