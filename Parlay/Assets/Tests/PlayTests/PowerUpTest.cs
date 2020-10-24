using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{

    public class PowerUpTest
    {
        private GameObject player;
        private CharacterController2D characterController;
        private GameObject powerUp;
        private GameManager gameManager;
        private GameObject gameManagerObj;


        [SetUp]
        public void Setup()
        {
            foreach (GameObject GM in GameObject.FindGameObjectsWithTag("GameManager"))
            {
                GameObject.Destroy(GM);
            }
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
        }

        [TearDown]
        public void Teardown()
        {
            gameManager.questionUI.gameObject.SetActive(false);
            GameObject.Destroy(powerUp);
            GameObject.Destroy(player);
            GameObject.Destroy(gameManagerObj);

            foreach (GameObject chunk in GameObject.FindGameObjectsWithTag("Chunck"))
            {
                GameObject.Destroy(chunk);
            }
        }

        private Vector2 GetPowerUpColliderPos()
        {
            return powerUp.transform.localPosition - (powerUp.GetComponent<SpriteRenderer>().bounds.extents / 2);
        }

        private void CollideWithPowerUp()
        {
            Vector2 powerUpPos = GetPowerUpColliderPos();
            characterController.Move(powerUpPos.x - 0.1f, false);
        }

        // [UnityTest, Order(1)]
        // public IEnumerator TestTimePowerUpActivation()
        // {
        //     float initialTime = gameManager.timerManager.getCurrTime();
        //     powerUp = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Terrain Prefabs/Interactible Tiles/PotionTile1"));
        //     CollideWithPowerUp();
        //     yield return new WaitForSeconds(2);
        //     //Test if time was increased
        //     Assert.True(gameManager.timerManager.getCurrTime() - initialTime >= 0);
        // }

    }
}
