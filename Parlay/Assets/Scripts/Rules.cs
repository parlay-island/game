using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rules : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void backToModeSelection()
    {
        StartCoroutine(LoadModeScreen());
    }

    IEnumerator LoadModeScreen()
    {
        string mode_scene_name = "ModeSelection";
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(mode_scene_name, LoadSceneMode.Single);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void goToRulesScreen(string rules_screen_name)
    {
        StartCoroutine(LoadRulesScreen(rules_screen_name));
    }

    IEnumerator LoadRulesScreen(string rules_screen_name)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(rules_screen_name, LoadSceneMode.Single);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
