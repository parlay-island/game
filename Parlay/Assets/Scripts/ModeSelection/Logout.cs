using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logout : MonoBehaviour
{
    [SerializeField] public AbstractPlayerRetriever playerRetriever;
    private Player player;

    public void Awake()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("PlayerInfo");
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
        string login_scene_name = "LoginScreen";

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(login_scene_name, LoadSceneMode.Single);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
