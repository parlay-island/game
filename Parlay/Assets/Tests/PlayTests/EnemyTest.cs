using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
  public class EnemyTest
  {
      private GameObject testEnemy;
      private GameObject enemy2;
      private GameObject player;
      private GameManager gameManager;
      private GameObject gameManagerObj;
      private GameObject level;
      private CharacterController2D characterController;

      [SetUp]
      public void Setup()
      {
        gameManagerObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/GameManager"));
        gameManager = gameManagerObj.GetComponent<GameManager>();
        player = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
        characterController = player.GetComponent<CharacterController2D>();
        testEnemy = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Enemy"));
        level = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/LevelGenerator"));
        player.GetComponent<PlayerMovement>().questionUI.gameObject.SetActive(false);
        gameManager.questionUI.gameObject.SetActive(false);
        gameManager.setGameTime(10f);
        foreach(GameObject question in GameObject.FindGameObjectsWithTag("Question"))
        {
          GameObject.Destroy(question);
        }
      }

      [TearDown]
      public void Teardown()
      {
        player.GetComponent<PlayerMovement>().questionUI.gameObject.SetActive(false);
        gameManager.questionUI.gameObject.SetActive(false);
        GameObject.Destroy(player);
        foreach(GameObject chunk in GameObject.FindGameObjectsWithTag("Chunck"))
        {
          GameObject.Destroy(chunk);
        }
        GameObject.Destroy(level);
        if(testEnemy != null)
          GameObject.Destroy(testEnemy);
        if(enemy2 != null)
          GameObject.Destroy(enemy2);
        GameObject.Destroy(gameManagerObj);
      }

      private Vector2 GetEnemyColliderPos()
      {
        return testEnemy.transform.position - testEnemy.transform.right * testEnemy.GetComponent<SpriteRenderer>().bounds.extents.x;
      }

      private void CollidePlayerWithEnemy()
      {
        Vector2 enemyPos = GetEnemyColliderPos();
        characterController.Move(enemyPos.x, false);
      }

      [UnityTest]
      public IEnumerator TestEnemyPlayerCollision()
      {
        // float timeReduction = testEnemy.GetComponent<Enemy>().GetTimeReduction();
        // CollidePlayerWithEnemy();
        // float initialTime = gameManager.timerManager.getCurrTime();
        float waitTime = 0.5f;
        yield return new WaitForSeconds(waitTime);
        // Assert.True(initialTime - waitTime - gameManager.timerManager.getCurrTime() >= Mathf.Abs(timeReduction));
        // Assert.True(testEnemy != null);
      }

      [UnityTest]
      public IEnumerator TestEnemyKilled()
      {
        // float timeReduction = testEnemy.GetComponent<Enemy>().GetTimeReduction();
        // float initialTime = gameManager.timerManager.getCurrTime();
        // characterController.Move(testEnemy.transform.position.x, true);
        float waitTime = 5f;
        yield return new WaitForSeconds(waitTime);
        // Assert.True(initialTime - waitTime - gameManager.timerManager.getCurrTime() < Mathf.Abs(timeReduction));
        // Assert.True(testEnemy == null);
      }

      [UnityTest]
      public IEnumerator TestEnemyCollisionWithEnemyHasNoEffect()
      {
        characterController.Move(-50f, false);
        float waitTime = 0.2f;
        Rigidbody2D body1 = testEnemy.GetComponent<Rigidbody2D>();
        enemy2 = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Enemy"));
        Vector3 currRotation = testEnemy.transform.eulerAngles;
        currRotation.y += 180;
        enemy2.transform.eulerAngles = currRotation;
        Rigidbody2D body2 = enemy2.GetComponent<Rigidbody2D>();
        body2.velocity = -body1.velocity;
        body2.MovePosition(GetEnemyColliderPos());
        Vector2 initialVelocityEnemy1 = body1.velocity;
        Vector2 initialVelocityEnemy2 = body2.velocity;
        yield return new WaitForSeconds(waitTime);
        Assert.AreEqual(body1.velocity, initialVelocityEnemy1);
        Assert.AreEqual(body2.velocity, initialVelocityEnemy2);
      }

      [UnityTest]
      public IEnumerator TestEnemyInitialFreeMovement()
      {
        float initialEnemyXPos = testEnemy.transform.position.x;
        yield return new WaitForSeconds(1f);
        Assert.Greater(testEnemy.transform.position.x, initialEnemyXPos);
      }

      [UnityTest]
      public IEnumerator TestEnemyDirectionChangeWhenEncounterObstacle()
      {
        Rigidbody2D rigidbody = testEnemy.GetComponent<Rigidbody2D>();
        float initialXVelocity = rigidbody.velocity.x;
        CollidePlayerWithEnemy();
        yield return new WaitForSeconds(1f);
        Assert.True(initialXVelocity < 0);
        Assert.True(rigidbody.velocity.x > 0);
      }

      [UnityTest]
      public IEnumerator TestEnemyDirectionChangeWhenReachEnd()
      {
        characterController.Move(-2000f, false);
        Rigidbody2D rigidbody = testEnemy.GetComponent<Rigidbody2D>();
        float initialXVelocity = rigidbody.velocity.x;
        testEnemy.transform.position = new Vector3(-1500f, testEnemy.transform.position.y, 0);
        yield return new WaitForFixedUpdate();
        Assert.True(initialXVelocity < 0);
        Assert.True(rigidbody.velocity.x > 0);
      }

  }
}
