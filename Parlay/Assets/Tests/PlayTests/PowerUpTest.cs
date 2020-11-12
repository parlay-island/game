using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests
{

    public class PowerUpTest
    {
        private GameObject player;
        private CharacterController2D characterController;
        private GameObject powerUp;
        private GameManager gameManager;
        private GameObject gameManagerObj;
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
            characterController.Move(10f, false);
            foreach (GameObject question in GameObject.FindGameObjectsWithTag("Question"))
            {
                GameObject.Destroy(question);
            }
            powerupText = MonoBehaviour.Instantiate(Resources.Load<Text>("Prefabs/PowerUpLabel"));
            gameManager.powerUpText = powerupText;
            GameManager.instance.powerUpText = powerupText;
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
         public IEnumerator TestTimePowerUpActivation()
         {
            float initialTime = gameManager.timerManager.getCurrTime();
            powerUp = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Terrain Prefabs/Interactible Tiles/ChestTile1"));
            powerUp.GetComponent<PowerUp>().type = 1;
            CollideWithPowerUp();
            float waitTime = 3f;
             yield return new WaitForSeconds(waitTime);
             //Test if time was increased
             Assert.True(gameManager.timerManager.getCurrTime() - initialTime - waitTime >= 0);
         }

    }
}
