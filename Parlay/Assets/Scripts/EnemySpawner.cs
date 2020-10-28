using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private const float MULTIPLIER = 100f;

    [SerializeField] private GameObject enemyPrefab;
    [Range(0.1f, 5f)] [SerializeField] private float minSpawnTime = 1.25f;
    [Range(0.1f, 4f)] [SerializeField] private float spawnTimeRandomnessUpperBound = 3f;

    private Timer timer;

    void Start()
    {
      timer = gameObject.AddComponent<Timer>();
      timer.Start();
      timer.SetUp(GenerateRandomSpawnTime());
    }

    void Update()
    {
      if(!GameManager.instance.IsQuestionShown())
      {
         timer.updateTime();
         if (timer.isTimeUp())
         {
           Vector3 pos = new Vector3(transform.position.x, transform.position.y, 0);
           Instantiate(enemyPrefab, pos, Quaternion.identity);
           timer.AddTime(GenerateRandomSpawnTime());
         }
      }
    }

    private float GenerateRandomSpawnTime()
    {
      return (float)(minSpawnTime + Random.Range(0, spawnTimeRandomnessUpperBound * MULTIPLIER)/MULTIPLIER);
    }

}
