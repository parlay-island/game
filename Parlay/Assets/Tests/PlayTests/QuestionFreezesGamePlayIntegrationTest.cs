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


    [SetUp]
    public void Setup()
    {
        gameManagerObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/GameManager"));
        testPlayer = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
        characterController = testPlayer.GetComponent<CharacterController2D>();
        playerMovement = testPlayer.GetComponent<PlayerMovement>();
        level = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/LevelGenerator"));
        testEnemy = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Enemy"));
        gameManagerObj.GetComponent<GameManager>().setGameTime(10f);
    }

    [TearDown]
    public void Teardown()
    {
        playerMovement.questionUI.SetActive(false);
        testEnemy.GetComponent<Enemy>().questionUI.SetActive(false);
        GameObject.Destroy(gameManagerObj);
        GameObject.Destroy(testPlayer);
        GameObject.Destroy(testEnemy);
        foreach(GameObject chunk in GameObject.FindGameObjectsWithTag("Chunck"))
        {
          GameObject.Destroy(chunk);
        }
        GameObject.Destroy(level);
    }

    [UnityTest]
    public IEnumerator TestMovementAllowedWhenQuestionNotShown()
    {
      playerMovement.questionUI.SetActive(false);
      testEnemy.GetComponent<Enemy>().questionUI.SetActive(false);
      float initialEnemyXPos = testEnemy.transform.position.x;
      float initialDistance = playerMovement.getDistanceTravelled();
      characterController.Move(0.5f, false);
      yield return new WaitForSeconds(1f);
      Assert.Greater(testEnemy.transform.position.x, initialEnemyXPos);
      Assert.Greater(playerMovement.getDistanceTravelled(), initialDistance);
    }

    [UnityTest]
    public IEnumerator TestPlayerMovementFreezesWhenQuestionShown()
    {
      playerMovement.questionUI.SetActive(true);
      float initialDistance = playerMovement.getDistanceTravelled();
      characterController.Move(0.5f, false);
      yield return new WaitForSeconds(1f);
      Assert.AreEqual(playerMovement.getDistanceTravelled(), initialDistance);
    }

    [UnityTest]
    public IEnumerator TestEnemyMovementFreezesWhenQuestionShown()
    {
      testEnemy.GetComponent<Enemy>().questionUI.SetActive(true);
      float initialEnemyXPos = testEnemy.transform.position.x;
      yield return new WaitForSeconds(0.5f);
      Assert.AreEqual(testEnemy.transform.position.x, initialEnemyXPos);
    }


  }
}
