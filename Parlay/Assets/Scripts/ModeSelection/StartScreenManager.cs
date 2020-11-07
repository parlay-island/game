using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScreenManager : MonoBehaviour
{
    public void LoadLogin()
    {
      StartCoroutine(LoadLoginScene());
    }

    IEnumerator LoadLoginScene()
    {
      string login_scene_name = "LoginScreen";

      AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(login_scene_name, LoadSceneMode.Single);
      while (!asyncLoad.isDone)
      {
          yield return null;
      }
    }
}
