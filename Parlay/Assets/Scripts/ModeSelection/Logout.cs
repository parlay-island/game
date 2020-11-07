using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logout : MonoBehaviour
{
    [SerializeField] public AbstractPlayerRetriever playerRetriever;
    public Player player;
    public bool isTest = false;

    public void Awake()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("PlayerInfo");
        if (playerObj != null)
            player = playerObj.GetComponent<Player>();
    }

    public void LogoutOfGame()
    {
        playerRetriever.LogoutPlayer(LoadStart, DisplayError, player);
    }

    public void DisplayError(string message)
    {
        print(message);
    }

    private void LoadStart()
    {
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
