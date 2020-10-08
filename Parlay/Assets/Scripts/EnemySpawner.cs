using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [Range(0.1f, 5f)] [SerializeField] private float spawnTime = 1.25f;
    [Range(0.1f, 4f)] [SerializeField] private float spawnTimeRandomnessUpperBound = 3f;

    private float spawnTimer;

    void Start()
    {
      ResetSpawnTimer();
    }

    void Update()
    {
      if(!GameManager.instance.IsQuestionShown())
      {
        spawnTimer -= Time.deltaTime;
         if (spawnTimer <= 0.0f)
         {
             Vector3 pos = new Vector3(transform.position.x, transform.position.y, 0);
             Instantiate(enemyPrefab, pos, Quaternion.identity);
             ResetSpawnTimer();
         }
      }
    }

    private void ResetSpawnTimer()
     {
         spawnTimer = (float)(spawnTime + Random.Range(0, spawnTimeRandomnessUpperBound*100)/100.0);
     }
}
