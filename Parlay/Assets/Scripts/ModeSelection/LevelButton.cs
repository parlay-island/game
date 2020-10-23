using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    private List<LevelModel> levels = new List<LevelModel>();
    private GameObject levelObj;

    void Start()
    {
      ModeRetriever mode_retriever = GameObject.Find("ModeRetriever").GetComponent<ModeRetriever>();
      levels = mode_retriever.GetLevels();
      levelObj = GameObject.Find("LevelObj");
    }

    public void LoadGame()
    {
      string level_name = gameObject.GetComponentInChildren<TextMeshProUGUI>().text;
      LevelModel level_selected = new LevelModel(1, "");
      foreach(LevelModel level in levels){
        if(level.name == level_name)
        {
          level_selected = level;
        }
      }
      levelObj.GetComponent<Level>().SetLevel(level_selected);
      StartCoroutine(Load());
    }

    IEnumerator Load()
    {
      Scene currentScene = SceneManager.GetActiveScene();
      string game_scene_name = "Development";

      AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(game_scene_name, LoadSceneMode.Single);
      while (!asyncLoad.isDone)
      {
          yield return null;
      }
      SceneManager.UnloadSceneAsync(currentScene);
    }
}
