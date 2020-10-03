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
        GameObject.Destroy(characterController);
        GameObject.Destroy(player);
        foreach(GameObject chunk in GameObject.FindGameObjectsWithTag("Chunck"))
        {
          GameObject.Destroy(chunk);
        }
        GameObject.Destroy(level);
        if(testEnemy != null)
          GameObject.Destroy(testEnemy);
        GameObject.Destroy(gameManagerObj);
        GameObject.Destroy(gameManager);
      }

      [UnityTest]
      public IEnumerator TestEnemyPlayerCollision()
      {
        Vector2 enemyPos = testEnemy.transform.position - testEnemy.transform.right * testEnemy.GetComponent<SpriteRenderer>().bounds.extents.x;
        float initialTime = gameManager.timerManager.getCurrTime();
        characterController.Move(enemyPos.x, false);
        float waitTime = 1f;
        yield return new WaitForSeconds(waitTime);
        Assert.Less(gameManager.timerManager.getCurrTime(), initialTime - waitTime);
        Assert.True(testEnemy != null);
      }

      [UnityTest]
      public IEnumerator TestEnemyKilled()
      {
        float initialTime = gameManager.timerManager.getCurrTime();
        characterController.Move(testEnemy.transform.position.x, true);
        float waitTime = 2f;
        yield return new WaitForSeconds(waitTime);
        Assert.True(testEnemy == null);
        Assert.GreaterOrEqual(gameManager.timerManager.getCurrTime(), initialTime - waitTime);
        yield return null;
      }

      /**
      *   Feature: when an enemy collides with another enemy nothing happens
      *   Purpose: To ensure that if 2 enemies happen to hit each other it will not affect the player’s gameplay or cause unintended consequences
      *   Preconditions: the scene has multiple enemies
      *   Steps: move an enemy into another enemy
      *   Postconditions: the enemies continue moving and the time does not change
      */
      [UnityTest]
      public IEnumerator TestEnemyCollision()
      {
        yield return null;
      }

      /**
      *   Feature: enemies freely move without any input from the user
      *   Purpose: So that the enemies are not in a static position and can create more difficulty for the player (ex: if an enemy is where a question is located). Ensuring that the enemies are moving as time goes on
      *   Preconditions: An enemy is on the scene
      *   Steps: Wait for certain amount of time to pass
      *   Postconditions: the enemy’s x position has changed
      */
      [UnityTest]
      public IEnumerator TestEnemyMovement()
      {
        yield return null;
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
