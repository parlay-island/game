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
        gameManager.setGameTime(30f);
        player = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
        characterController = player.GetComponent<CharacterController2D>();
        testEnemy = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Enemy"));
        level = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/LevelGenerator"));
        player.GetComponent<PlayerMovement>().questionUI.gameObject.SetActive(false);
        gameManager.questionUI.gameObject.SetActive(false);
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
        GameObject.Destroy(gameManagerObj);
        foreach(GameObject chunk in GameObject.FindGameObjectsWithTag("Chunck"))
        {
          GameObject.Destroy(chunk);
        }
        GameObject.Destroy(level);
        if(testEnemy != null)
          GameObject.Destroy(testEnemy);
        if(enemy2 != null)
          GameObject.Destroy(enemy2);
      }

      private Vector2 GetEnemyColliderPos()
      {
        return testEnemy.transform.localPosition - (testEnemy.GetComponent<SpriteRenderer>().bounds.extents / 2);
      }

      private void CollidePlayerWithEnemy()
      {
        Vector2 enemyPos = GetEnemyColliderPos();
        characterController.Move(enemyPos.x - 0.1f, false);
      }

      [UnityTest, Order(1)]
      public IEnumerator TestEnemyPlayerCollision()
      {
        float initialTime = gameManager.timerManager.getCurrTime();
        float timeReduction = testEnemy.GetComponent<Enemy>().GetTimeReduction();
        CollidePlayerWithEnemy();
        yield return new WaitForSeconds(1f);
        Assert.True(testEnemy != null);
        Assert.True(initialTime - gameManager.timerManager.getCurrTime() >= Mathf.Abs(timeReduction));
      }

      [Retry(3)]
      [UnityTest, Order(2)]
      public IEnumerator TestEnemyKilled()
      {
        float timeReduction = testEnemy.GetComponent<Enemy>().GetTimeReduction();
        float initialTime = gameManager.timerManager.getCurrTime();
        characterController.Move(testEnemy.transform.position.x + (testEnemy.transform.right.x * testEnemy.GetComponent<SpriteRenderer>().bounds.extents.x), true);
        float waitTime = 5f;
        yield return new WaitForSeconds(waitTime);
        Assert.True(initialTime - waitTime - gameManager.timerManager.getCurrTime() < Mathf.Abs(timeReduction));
        Assert.True(testEnemy == null);
      }

      [Retry(3)]
      [UnityTest, Order(3)]
      public IEnumerator TestEnemyCollisionWithEnemyHasNoEffect()
      {
        characterController.Move(-100f, false);
        float waitTime = 1f;
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
        Assert.True(body1.velocity.x * initialVelocityEnemy1.x > 0);
        Assert.True(body2.velocity.x * initialVelocityEnemy2.x > 0);
      }

      [UnityTest, Order(4)]
      public IEnumerator TestEnemyInitialFreeMovement()
      {
        float initialEnemyXPos = testEnemy.transform.position.x;
        yield return new WaitForSeconds(1f);
        Assert.AreNotEqual(testEnemy.transform.position.x, initialEnemyXPos);
      }

      [Retry(3)]
      [UnityTest, Order(5)]
      public IEnumerator TestEnemyDirectionChangeWhenEncounterObstacle()
      {
        Rigidbody2D rigidbody = testEnemy.GetComponent<Rigidbody2D>();
        float initialXVelocity = rigidbody.velocity.x;
        CollidePlayerWithEnemy();
        yield return new WaitForSeconds(3f);
        Assert.True(initialXVelocity * rigidbody.velocity.x < 0);
      }

      [Retry(3)]
      [UnityTest, Order(6)]
      public IEnumerator TestEnemyDirectionChangeWhenReachEnd()
      {
        characterController.Move(-2000f, false);
        Rigidbody2D rigidbody = testEnemy.GetComponent<Rigidbody2D>();
        float initialXVelocity = rigidbody.velocity.x;
        testEnemy.transform.position = new Vector3(-1500f, testEnemy.transform.position.y, 0);
        yield return new WaitForFixedUpdate();
        Assert.True(initialXVelocity * rigidbody.velocity.x < 0);
      }

  }
}
