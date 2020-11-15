using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/**
* This file manages the transitions between the start screen to the login screen
* and sign up screen.
* 
* @author: Holly Ansel
*/


public class StartScreenManager : MonoBehaviour
{
    public const string LOGIN_SCREEN_NAME = "LoginScreen";
    public const string SIGNUP_SCREEN_NAME = "SignUpScreen";

    public void LoadLogin()
    {
      StartCoroutine(LoadLoginScene());
    }

    IEnumerator LoadLoginScene()
    {
      AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(LOGIN_SCREEN_NAME, LoadSceneMode.Single);
      while (!asyncLoad.isDone)
      {
          yield return null;
      }
    }


    public void LoadSignUp()
    {
      StartCoroutine(LoadSignUpScene());
    }

    IEnumerator LoadSignUpScene()
    {
      AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SIGNUP_SCREEN_NAME, LoadSceneMode.Single);
      while (!asyncLoad.isDone)
      {
          yield return null;
      }
    }
}
