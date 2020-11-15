using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using TMPro;

/**
* This file holds the AccountCreationManager.
* The manager takes in data from the registration input fields and calls
* the player retriever to register the user in the server with the data.
* 
* @author: Holly Ansel
*/

public class AccountCreationManager : MonoBehaviour
{
    [SerializeField] public TMP_InputField usernameInput;
    [SerializeField] public TMP_InputField passwordInput;
    [SerializeField] public TMP_InputField passwordConfirmationInput;
    [SerializeField] private TMP_InputField classCodeInput;
    [SerializeField] public LoginManager loginManager;
    [SerializeField] public AbstractPlayerRetriever playerRetriever;
    private const string PASS_MATCH_ERROR = "Your passwords don't match";
    private const string PASS_LENGTH_ERROR = "Your password needs to be at least 8 characters";

    public void CreateAccount()
    {
      string username = usernameInput.text;
      string password = passwordInput.text;
      string passwordConfirmation = passwordConfirmationInput.text;
      string classCode = classCodeInput.text;
      if(password != passwordConfirmation)
      {
        DisplayError(PASS_MATCH_ERROR);
      } else if (password.Length < 8)
      {
        DisplayError(PASS_LENGTH_ERROR);
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
