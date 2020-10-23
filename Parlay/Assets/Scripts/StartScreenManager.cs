using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScreenManager : MonoBehaviour
{
    public Button play_game;
    public GameObject mode_selector;

    public void LoadModeSelection()
    {
      StartCoroutine(Load());
    }

    IEnumerator Load()
    {
      Scene currentScene = SceneManager.GetActiveScene();
      string mode_scene_name = "ModeSelection";
      //Scene mode_selection_scene = SceneManager.GetSceneByName(mode_scene_name);

      AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(mode_scene_name, LoadSceneMode.Additive);
      while (!asyncLoad.isDone)
      {
          yield return null;
      }
      SceneManager.MoveGameObjectToScene(mode_selector, SceneManager.GetSceneByName(mode_scene_name));
      SceneManager.UnloadSceneAsync(currentScene);
    }
}
