using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This file sets up the level generator
 * The level generator randomly loads all the chunks into the game
 * Chunks are premade level components which contain tilessets, this is the terrain for the game
 * This file also has the functionality to reset the level
 * 
 * @author: Andres Montoya
 */

public class LevelGenerator : MonoBehaviour
{
    private const float PLAYER_DISTANCE_CHUNCK_SPAWN = 20f;

    [SerializeField] private Transform levelStart;
    [SerializeField] private List<Transform> chunckList;
    [SerializeField] public Rigidbody2D player;

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

        //Move Player
        player.MovePosition(new Vector2(-6, 1));
    }

    public void Reset()
    {
        //Delete chunks
        foreach (GameObject chunk in GameObject.FindGameObjectsWithTag("Chunck"))
        {
            GameObject.Destroy(chunk);
        }

        //Generate Terrain
        Transform firstChunck = SpawnChunck(levelStart, startPosition);
        lastEndPosition = firstChunck.Find("EndPosition").position;
        for (int i = 0; i < startingChuncks; i++)
        {
            SpawnChunck();
        }

        //Move Player
        player.position = new Vector2(-6, 14);
        player.MovePosition(new Vector2(-6, 14));
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
