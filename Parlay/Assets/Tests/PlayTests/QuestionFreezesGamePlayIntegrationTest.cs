using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
  public class QuestionFreezesGamePlayIntegrationTest
  {
    private GameObject testEnemy;
    private GameObject testPlayer;
    private CharacterController2D characterController;
    private PlayerMovement playerMovement;
    private GameObject level;
    private GameObject gameManagerObj;
    private GameManager gameManager;
    private GameObject enemySpawner;

    [SetUp]
    public void Setup()
    {
        gameManagerObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/GameManager"));
        testPlayer = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
        characterController = testPlayer.GetComponent<CharacterController2D>();
        playerMovement = testPlayer.GetComponent<PlayerMovement>();
        level = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/LevelGenerator"));
        testEnemy = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Enemy"));
        gameManager = gameManagerObj.GetComponent<GameManager>();
        enemySpawner = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/EnemySpawner"));
        gameManager.setGameTime(20f);
        enemySpawner.gameObject.SetActive(true);
        gameManager.questionUI.gameObject.SetActive(true);
    }

    [TearDown]
    public void Teardown()
    {
        playerMovement.questionUI.gameObject.SetActive(false);
        gameManager.questionUI.gameObject.SetActive(false);
        enemySpawner.gameObject.SetActive(true);
        foreach(GameObject obj in GameObject.FindObjectsOfType<GameObject>()) {
          GameObject.Destroy(obj);
        }
    }

    [Retry(3)]
    [UnityTest, Order(1)]
    public IEnumerator TestMovementAllowedWhenQuestionNotShown()
    {
      playerMovement.questionUI.SetActive(false);
      gameManager.questionUI.SetActive(false);
      float initialEnemyXPos = testEnemy.transform.position.x;
      float initialDistance = playerMovement.getDistanceTravelled();
      characterController.Move(2f, false);
      yield return new WaitForSeconds(3f);
      Assert.AreNotEqual(testEnemy.transform.position.x, initialEnemyXPos);
      Assert.AreNotEqual(playerMovement.getDistanceTravelled(), initialDistance);
    }

    [Retry(3)]
    [UnityTest, Order(2)]
    public IEnumerator TestEnemiesSpawningWhenQuestionNotShown()
    {
      gameManager.questionUI.SetActive(false);
      int initialNumberOfEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
      yield return new WaitForSeconds(5f);
      Assert.Greater(GameObject.FindGameObjectsWithTag("Enemy").Length, initialNumberOfEnemies);
    }


    [UnityTest, Order(3)]
    public IEnumerator TestPlayerMovementFreezesWhenQuestionShown()
    {
      playerMovement.questionUI.SetActive(true);
      float initialDistance = playerMovement.getDistanceTravelled();
      characterController.Move(0.5f, false);
      yield return new WaitForSeconds(1f);
      Assert.AreEqual(playerMovement.getDistanceTravelled(), initialDistance);
    }

    [UnityTest, Order(4)]
    public IEnumerator TestEnemyMovementFreezesWhenQuestionShown()
    {
      gameManager.questionUI.SetActive(true);
      float initialEnemyXPos = testEnemy.transform.position.x;
      yield return new WaitForSeconds(0.5f);
      Assert.AreEqual(testEnemy.transform.position.x, initialEnemyXPos);
    }

    [UnityTest, Order(5)]
    public IEnumerator TestEnemySpawningFreezesWhenQuestionShown()
    {
      gameManager.questionUI.SetActive(true);
      int initialNumberOfEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
      yield return new WaitForSeconds(4.25f);
      Assert.AreEqual(GameObject.FindGameObjectsWithTag("Enemy").Length, initialNumberOfEnemies);
    }


  }
}
