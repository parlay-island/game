using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using TMPro;

public class AccountCreationManager : MonoBehaviour
{
    [SerializeField] public TMP_InputField usernameInput;
    [SerializeField] public TMP_InputField passwordInput;
    [SerializeField] public TMP_InputField passwordConfirmationInput;
    [SerializeField] private TMP_InputField classCodeInput;
    [SerializeField] public LoginManager loginManager;
    [SerializeField] public AbstractPlayerRetriever playerRetriever;

    public void CreateAccount()
    {
      string username = usernameInput.text;
      string password = passwordInput.text;
      string passwordConfirmation = passwordConfirmationInput.text;
      string classCode = classCodeInput.text;
      if(password != passwordConfirmation)
      {
        DisplayError("Your passwords don't match");
      } else if (password.Length < 8)
      {
        DisplayError("Your password needs to be at least 8 characters");
      } else
      {
        CreateAccountModel createAccountModel = new CreateAccountModel(username, password, classCode);
        playerRetriever.CreateAccount(createAccountModel, LoginInNewlyCreatedPlayer, DisplayError);
      }
    }

    public void LoginInNewlyCreatedPlayer()
    {
      loginManager.Login();
    }

    public void DisplayError(string message)
    {
      loginManager.DisplayError(message);
    }
}
