using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

/**
* This file contains the business logic to log out of the game. It references
* the player retriever to log out the current user and destroys all redundent 
* objects (playerObj and levelInfo). It then manages navigation back to the start
* screen.
* 
* @author: Andres
*/

public class Logout : MonoBehaviour
{
    [SerializeField] public AbstractPlayerRetriever playerRetriever;
    private GameObject playerObj;
    [FormerlySerializedAs("player")] public PlayerAuth playerAuth;
    public bool isTest = false;

    public void Awake()
    {
        playerObj = GameObject.FindGameObjectWithTag("PlayerInfo");
        if (playerObj != null)
            playerAuth = playerObj.GetComponent<PlayerAuth>();
    }

    public void LogoutOfGame()
    {
        playerRetriever.LogoutPlayer(LoadStart, DisplayError, playerAuth);
    }

    public void DisplayError(string message)
    {
        print(message);
    }

    private void LoadStart()
    {
        GameObject.Destroy(playerObj);
        GameObject.Destroy(GameObject.FindGameObjectWithTag("LevelInfo"));
        StartCoroutine(LoadStartScreen());
    }

    IEnumerator LoadStartScreen()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string start_scene_name = "StartScreen";

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(start_scene_name, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        if (!isTest)
            yield return SceneManager.UnloadSceneAsync(currentScene);
        else
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(start_scene_name));
    }
}
