using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using TMPro;

public class LoginManager : MonoBehaviour
{
    [SerializeField] public TMP_InputField usernameInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] public AbstractPlayerRetriever playerRetriever;
    [SerializeField] public TextMeshProUGUI errorMessage;
    [SerializeField] public Player player;

    public bool isTest = false;

    void Awake()
    {
      errorMessage.faceColor = new Color32(250, 0, 0, 250);
    }

    public void BackToStartScreen()
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
      if(!isTest)
        yield return SceneManager.UnloadSceneAsync(currentScene);
      else
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(start_scene_name));
    }

    public void Login()
    {
      string username = usernameInput.text;
      string password = passwordInput.text;
      LoginModel loginModel = new LoginModel(username, password);
      playerRetriever.LoginPlayer(loginModel, GoToModeSelection, DisplayError, player);
    }

    public void DisplayError(string message)
    {
      errorMessage.SetText(message);
      errorMessage.gameObject.SetActive(true);
    }

    public void GoToModeSelection()
    {
      StartCoroutine(LoadModeScene());
    }


    IEnumerator LoadModeScene()
    {
      DontDestroyOnLoad(player);
      Scene currentScene = SceneManager.GetActiveScene();
      string mode_scene_name = "ModeSelection";

      AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(mode_scene_name, LoadSceneMode.Additive);
      while (!asyncLoad.isDone)
      {
          yield return null;
      }
      if(!isTest)
        yield return SceneManager.UnloadSceneAsync(currentScene);
      else
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(mode_scene_name));
    }
}
