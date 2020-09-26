using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private const float PLAYER_DISTANCE_CHUNCK_SPAWN = 200f;

    [SerializeField] private Transform levelStart;
    //[SerializeField] private Player player;

    //Legnth of chunck is 14
    private Vector3 lastEndPosition;
    private Vector3 startPosition = new Vector3(-7, -1);
    private int startingChuncks = 5;

    private void Awake()
    {
        Transform firstChunck = SpawnChunck(startPosition);
        lastEndPosition = firstChunck.Find("EndPosition").position;
        for (int i = 0; i < startingChuncks; i++)
        {
            SpawnChunck();
        }
    }

    private void Update()
    {
        //Check to see whether player is near enough edge of chunck to spawn another chunck
        //if (Vector3.Distance(player.GetPosition(), lastEndPosition) < PLAYER_DISTANCE_CHUNCK_SPAWN)
        //{
        //    //Spawn another chunck
        //    SpawnChunck();
        //}
    }

    private void SpawnChunck()
    {
        Transform lastChunckPartTransform = SpawnChunck(lastEndPosition);
        lastEndPosition = lastChunckPartTransform.Find("EndPosition").position;
    }



    private Transform SpawnChunck(Vector3 spawnPosition)
    {
        Transform chunckPartTransform = Instantiate(levelStart, spawnPosition, Quaternion.identity);
        return chunckPartTransform;
    }
}
