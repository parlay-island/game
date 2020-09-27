using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private const float PLAYER_DISTANCE_CHUNCK_SPAWN = 20f;

    [SerializeField] private Transform levelStart;
    [SerializeField] private List<Transform> chunckList;
    [SerializeField] private Rigidbody2D player;

    //Legnth of chunck is 14
    private Vector3 lastEndPosition;
    private Vector3 startPosition = new Vector3(-7, -1);
    private int startingChuncks = 5;

    private void Awake()
    {
        //Generate Terrain
        Transform firstChunck = SpawnChunck(levelStart, startPosition);
        lastEndPosition = firstChunck.Find("EndPosition").position;
        for (int i = 0; i < startingChuncks; i++)
        {
            SpawnChunck();
        }

        //Generate Player
        Instantiate(player, new Vector3(-6, 1), Quaternion.identity);
    }

    private void Update()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        //Check to see whether player is near enough edge of chunck to spawn another chunck
        if (Vector3.Distance(playerObj.transform.position, lastEndPosition) < PLAYER_DISTANCE_CHUNCK_SPAWN)
        {
            //Spawn another chunck
            SpawnChunck();
        }
    }

    private void SpawnChunck()
    {
        Transform chosenChunckPart = chunckList[Random.Range(0, chunckList.Count)];
        Transform lastChunckPartTransform = SpawnChunck(chosenChunckPart, lastEndPosition);
        lastEndPosition = lastChunckPartTransform.Find("EndPosition").position;
    }



    private Transform SpawnChunck(Transform chunckPart, Vector3 spawnPosition)
    {
        Transform chunckPartTransform = Instantiate(chunckPart, spawnPosition, Quaternion.identity);
        return chunckPartTransform;
    }
}
