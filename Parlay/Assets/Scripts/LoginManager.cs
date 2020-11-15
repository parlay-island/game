using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.Serialization;

/**
 * Manages the login screen and authentication into the server
 * On succesful login directs user to mode selection screen
 * 
 * @author: Holly Ansel
 */

public class LoginManager : MonoBehaviour
{
    [SerializeField] public TMP_InputField usernameInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] public AbstractPlayerRetriever playerRetriever;
    [SerializeField] public TextMeshProUGUI errorMessage;
    [FormerlySerializedAs("player")] [SerializeField] public PlayerAuth playerAuth;
    private const string START_SCREEN_NAME = "StartScreen";
    private const string MODE_SCREEN_NAME = "ModeSelection";

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

      AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(START_SCREEN_NAME, LoadSceneMode.Additive);
      while (!asyncLoad.isDone)
      {
          yield return null;
      }
      if(!isTest)
        yield return SceneManager.UnloadSceneAsync(currentScene);
      else
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(START_SCREEN_NAME));
    }

    public void Login()
    {
      string username = usernameInput.text;
      string password = passwordInput.text;
      LoginModel loginModel = new LoginModel(username, password);
      playerRetriever.LoginPlayer(loginModel, GoToModeSelection, DisplayError, playerAuth);
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
      DontDestroyOnLoad(playerAuth);
      Scene currentScene = SceneManager.GetActiveScene();

      AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(MODE_SCREEN_NAME, LoadSceneMode.Additive);
      while (!asyncLoad.isDone)
      {
          yield return null;
      }
      if(!isTest)
        yield return SceneManager.UnloadSceneAsync(currentScene);
      else
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(MODE_SCREEN_NAME));
    }
}
