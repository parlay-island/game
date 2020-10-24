using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScreenManager : MonoBehaviour
{
    [SerializeField] private Button play_game;
    [SerializeField] private GameObject mode_selector;

    public void LoadModeSelection()
    {
      StartCoroutine(LoadModeScene());
    }

    IEnumerator LoadModeScene()
    {
      Scene currentScene = SceneManager.GetActiveScene();
      string mode_scene_name = "ModeSelection";

      AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(mode_scene_name, LoadSceneMode.Additive);
      while (!asyncLoad.isDone)
      {
          yield return null;
      }
      SceneManager.MoveGameObjectToScene(mode_selector, SceneManager.GetSceneByName(mode_scene_name));
      yield return SceneManager.UnloadSceneAsync(currentScene);
    }
}
