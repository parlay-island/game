using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rules : MonoBehaviour
{
    public bool isTest = false;

    public void goToRulesOrModeScreen(string next_screen_name)
    {
        StartCoroutine(LoadRulesOrModeScreen(next_screen_name));
    }

    IEnumerator LoadRulesOrModeScreen(string next_screen_name)
    {
		Scene currentScene = SceneManager.GetActiveScene();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(next_screen_name, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
		if(!isTest)
        	yield return SceneManager.UnloadSceneAsync(currentScene);
      	else
        	SceneManager.SetActiveScene(SceneManager.GetSceneByName(next_screen_name));
    }
}
