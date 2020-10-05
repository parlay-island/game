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
        gameManager.setGameTime(10f);
      }

      [TearDown]
      public void Teardown()
      {
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
        CollidePlayerWithEnemy();
        float initialTime = gameManager.timerManager.getCurrTime();
        float waitTime = 2f;
        yield return new WaitForSeconds(waitTime);
        Assert.Less(Mathf.Ceil(gameManager.timerManager.getCurrTime()), Mathf.Ceil(initialTime - waitTime));
        Assert.True(testEnemy != null);
      }

      [UnityTest]
      public IEnumerator TestEnemyKilled()
      {
        float initialTime = gameManager.timerManager.getCurrTime();
        characterController.Move(testEnemy.transform.position.x, true);
        float waitTime = 5f;
        yield return new WaitForSeconds(waitTime);
        Assert.True(testEnemy == null);
        Assert.GreaterOrEqual(Mathf.Ceil(gameManager.timerManager.getCurrTime()), Mathf.Ceil(initialTime - waitTime));
        yield return null;
      }

      [UnityTest]
      public IEnumerator TestEnemyCollisionWithEnemyHasNoEffect()
      {
        characterController.Move(-50f, false);
        float waitTime = 0.2f;
        yield return new WaitForSeconds(waitTime);
        float initialTime = gameManager.timerManager.getCurrTime();
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
        Assert.GreaterOrEqual(Mathf.Ceil(gameManager.timerManager.getCurrTime()), Mathf.Ceil(initialTime - waitTime));
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
        CollidePlayerWithEnemy();
        float initialXVelocity = testEnemy.GetComponent<Rigidbody2D>().velocity.x;
        yield return new WaitForSeconds(1f);
        Assert.True(testEnemy.GetComponent<Rigidbody2D>().velocity.x == -initialXVelocity);
      }

      [UnityTest]
      public IEnumerator TestEnemyDirectionChangeWhenReachEnd()
      {
        characterController.Move(-2000f, false);
        yield return new WaitForSeconds(0.1f);
        Rigidbody2D rigidbody = testEnemy.GetComponent<Rigidbody2D>();
        float initialXVelocity = rigidbody.velocity.x;
        testEnemy.transform.position = new Vector3(-1500f, testEnemy.transform.position.y, 0);
        yield return new WaitForSeconds(0.7f);
        Assert.True(rigidbody.velocity.x == -initialXVelocity);
      }




      /**
      *   Feature: as the level map continues to generate, new enemies will spawn
      *   Purpose: So that as the player continues throughout the game they will continue to encounter enemies to maintain and increase difficulty. Ensuring that a specific number of enemies are present in each chunk.
      *   Preconditions: The player is in the scene with one chunk of the map generated
      *   Steps: The player moves far enough to create a new chunk of map
      *   Postconditions: In the new chunk of map there are at least a constant number of enemies determined in the code (maybe 2 enemies per chunk)
      */
      [UnityTest]
      public IEnumerator TestEnemySpawning()
      {
        yield return null;
      }

  }
}
