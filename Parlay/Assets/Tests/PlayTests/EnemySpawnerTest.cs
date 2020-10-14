using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
  public class EnemySpawnerTest
  {
    private GameManager gameManager;
    private GameObject gameManagerObj;
    private GameObject enemySpawner;

    [SetUp]
    public void Setup()
    {
      gameManagerObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/GameManager"));
      gameManager = gameManagerObj.GetComponent<GameManager>();
      enemySpawner = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/EnemySpawner"));
      gameManager.setGameTime(10f);
      enemySpawner.gameObject.SetActive(true);
    }

    [TearDown]
    public void Teardown()
    {
      enemySpawner.gameObject.SetActive(true);
      foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
      {
        GameObject.Destroy(enemy);
      }
      GameObject.Destroy(enemySpawner);
      GameObject.Destroy(gameManagerObj);
    }

    //tests for 2 time cycles to show that this is a repeating process
    [UnityTest]
    public IEnumerator TestEnemySpawning()
    {
      int initialNumberOfEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
      yield return new WaitForSeconds(4.25f);
      int numberEnemiesAfterSpawnTime = GameObject.FindGameObjectsWithTag("Enemy").Length;
      Assert.Greater(numberEnemiesAfterSpawnTime, initialNumberOfEnemies);
      yield return new WaitForSeconds(4.25f);
      Assert.Greater(GameObject.FindGameObjectsWithTag("Enemy").Length, numberEnemiesAfterSpawnTime);
      yield return new WaitForSeconds(0.5f);
    }


  }
}
