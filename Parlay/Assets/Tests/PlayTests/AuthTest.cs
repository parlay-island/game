using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using TMPro;

namespace Tests
{
    public class AuthTest
    {
        private const int testPlayerId = 5;
        private const float testPlayerAccuracy = 100f;
        private const string testAssignedClass = "TestClass";

        public class MockPlayerRetriever: AbstractPlayerRetriever
        {
          public override void LoginPlayer(LoginModel loginModel, System.Action successCallback, System.Action<string> errorCallback, PlayerAuth playerAuth)
          {
            PlayerModel player_fetched = new PlayerModel(testPlayerId, loginModel.username, testPlayerAccuracy);
            playerAuth.SetPlayer(player_fetched);
            successCallback();
          }
          public override void CreateAccount(CreateAccountModel createAccountModel, System.Action successCallback, System.Action<string> errorCallback)
          {
            successCallback();
          }
          public override void LogoutPlayer(System.Action successCallback, System.Action<string> errorCallback, PlayerAuth playerAuth)
          {
            GameObject.Destroy(playerAuth);
            successCallback();
          }
        }

        public class ErrorPlayerRetriever: AbstractPlayerRetriever
        {
          public override void LoginPlayer(LoginModel loginModel, System.Action successCallback, System.Action<string> errorCallback, PlayerAuth playerAuth)
          {
            errorCallback("error in test for player retriever");
          }
          public override void CreateAccount(CreateAccountModel createAccountModel, System.Action successCallback, System.Action<string> errorCallback)
          {
            errorCallback("error in test for creating account");
          }
          public override void LogoutPlayer(System.Action successCallback, System.Action<string> errorCallback, PlayerAuth playerAuth)
          {
            errorCallback("error in test for player retriever");
          }
        }

        private AccountCreationManager accountCreationManager;
        private LoginManager loginManager;
        private Logout logoutManager;
        private PlayerAuth _playerAuth;
        private TextMeshProUGUI errorMessage;
        private AbstractPlayerRetriever playerRetriever;
        private Scene testScene;
        private TMP_InputField usernameInput;
        private TMP_InputField passwordInput;
        private TMP_InputField passwordConfirmationInput;

        [OneTimeSetUp]
        public void Init()
        {
          testScene = SceneManager.GetActiveScene();
        }

        [SetUp]
        public void Setup()
        {
          SetUpUIComponents();
          SetUpLoginManager();
          SetUpAccountManager();
        }

        private void SetUpUIComponents()
        {
          GameObject playerObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/PlayerInfo"));
          playerObj.name = "PlayerInfo";
          _playerAuth = playerObj.GetComponent<PlayerAuth>();
          usernameInput = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Login/Username")).GetComponent<TMP_InputField>();
          usernameInput.text = "";
          errorMessage = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Login/ErrorMessage")).GetComponent<TextMeshProUGUI>();
          errorMessage.SetText("");
          errorMessage.faceColor = new Color32(255, 0, 0, 255);
        }

        private void SetUpLoginManager()
        {
          loginManager = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Login/LoginManager")).GetComponent<LoginManager>();
          loginManager.playerAuth = _playerAuth;
          loginManager.errorMessage = errorMessage;
          loginManager.usernameInput = usernameInput;
          loginManager.isTest = true;
          logoutManager = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/LogoutButton")).GetComponent<Logout>();
          logoutManager.playerAuth = _playerAuth;
          logoutManager.isTest = true;
          playerRetriever = new GameObject().AddComponent<MockPlayerRetriever>();
          loginManager.playerRetriever = playerRetriever;
          logoutManager.playerRetriever = playerRetriever;
        }

        private void SetUpAccountManager()
        {
          passwordInput = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/SignUp/Password")).GetComponent<TMP_InputField>();
          passwordInput.text = "";
          passwordConfirmationInput = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/SignUp/PasswordConfirm")).GetComponent<TMP_InputField>();
          passwordConfirmationInput.text = "";
          accountCreationManager = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/SignUp/AccountCreationManager")).GetComponent<AccountCreationManager>();
          accountCreationManager.usernameInput = usernameInput;
          accountCreationManager.playerRetriever = playerRetriever;
          accountCreationManager.loginManager = loginManager;
          accountCreationManager.passwordInput = passwordInput;
          accountCreationManager.passwordConfirmationInput = passwordConfirmationInput;
        }

        [TearDown]
        public void Teardown()
        {
            foreach(GameObject obj in GameObject.FindObjectsOfType<GameObject>()) {
              GameObject.Destroy(obj);
            }
        }

        [UnityTest, Order(1)]
        public IEnumerator TestSuccessfulLoginGoesToModeSelection()
        {
            Scene originalScene = SceneManager.GetActiveScene();
            loginManager.Login();
            yield return new WaitForSeconds(1f);
            Scene newScene = SceneManager.GetActiveScene();
            Assert.AreNotEqual(originalScene.name, newScene.name);
            Assert.AreEqual("ModeSelection", newScene.name);
            SceneManager.SetActiveScene(testScene);
            SceneManager.UnloadSceneAsync(newScene);
            yield return new WaitForSeconds(1f);
        }

        [UnityTest, Order(2)]
        public IEnumerator TestLoginWithError()
        {
          Scene originalScene = SceneManager.GetActiveScene();
          string errorMessageBefore = errorMessage.text;
          AbstractPlayerRetriever errorPlayerRetriever = new GameObject().AddComponent<ErrorPlayerRetriever>();
          loginManager.playerRetriever = errorPlayerRetriever;
          loginManager.Login();
          yield return new WaitForSeconds(1f);
          Scene newScene = SceneManager.GetActiveScene();
          Assert.AreEqual(originalScene.name, newScene.name);
          Assert.AreNotEqual(errorMessageBefore, errorMessage.text);
        }

        [UnityTest, Order(3)]
        public IEnumerator TestBackToStartScreen()
        {
            Scene originalScene = SceneManager.GetActiveScene();
            loginManager.BackToStartScreen();
            yield return new WaitForSeconds(1f);
            Scene newScene = SceneManager.GetActiveScene();
            Assert.AreNotEqual(originalScene.name, newScene.name);
            Assert.AreEqual("StartScreen", newScene.name);
            SceneManager.SetActiveScene(testScene);
            SceneManager.UnloadSceneAsync(newScene);
            yield return new WaitForSeconds(1f);
        }

        [UnityTest, Order(4)]
        public IEnumerator TestPlayerPropagatesToNextSceneWithCorrectID()
        {
          Scene originalScene = SceneManager.GetActiveScene();
          loginManager.Login();
          yield return new WaitForSeconds(1f);
          Scene newScene = SceneManager.GetActiveScene();
          Assert.True(GameObject.Find("PlayerInfo") != null);
          Assert.AreEqual(_playerAuth.GetId(), testPlayerId);
          SceneManager.SetActiveScene(testScene);
          SceneManager.UnloadSceneAsync(newScene);
          yield return new WaitForSeconds(1f);
        }

        [UnityTest, Order(5)]
        public IEnumerator TestPlayerNameIsSetOnLogin()
        {
          usernameInput.text = "Test";
          Scene originalScene = SceneManager.GetActiveScene();
          loginManager.Login();
          yield return new WaitForSeconds(1f);
          Scene newScene = SceneManager.GetActiveScene();
          Assert.AreEqual(_playerAuth.GetName(), usernameInput.text);
          SceneManager.SetActiveScene(testScene);
          SceneManager.UnloadSceneAsync(newScene);
          yield return new WaitForSeconds(1f);
        }

        [UnityTest, Order(6)]
        public IEnumerator TestSuccessfulLogoutGoesToStart()
        {
            Scene originalScene = SceneManager.GetActiveScene();
            loginManager.Login();
            yield return new WaitForSeconds(1f);
            logoutManager.LogoutOfGame();
            yield return new WaitForSeconds(1f);
            Scene newScene = SceneManager.GetActiveScene();
            Assert.AreEqual("StartScreen", newScene.name);
            SceneManager.SetActiveScene(testScene);
            SceneManager.UnloadSceneAsync(newScene);
            yield return new WaitForSeconds(1f);
        }

        [UnityTest, Order(7)]
        public IEnumerator TestLogoutWithError()
        {
            Scene originalScene = SceneManager.GetActiveScene();
            AbstractPlayerRetriever errorPlayerRetriever = new GameObject().AddComponent<ErrorPlayerRetriever>();
            logoutManager.playerRetriever = errorPlayerRetriever;
            logoutManager.LogoutOfGame();
            yield return new WaitForSeconds(1f);
            Scene newScene = SceneManager.GetActiveScene();
            Assert.AreEqual(originalScene.name, newScene.name);
            Assert.AreNotEqual("error in test for player retriever", errorMessage.text);
        }
        [UnityTest, Order(8)]
        public IEnumerator TestSuccessfulCreateAccount()
        {
          usernameInput.text = "Test";
          passwordInput.text = "Test1234";
          passwordConfirmationInput.text = "Test1234";
          Scene originalScene = SceneManager.GetActiveScene();
          accountCreationManager.CreateAccount();
          yield return new WaitForSeconds(1f);
          Scene newScene = SceneManager.GetActiveScene();
          Assert.AreEqual(_playerAuth.GetName(), usernameInput.text);
          Assert.AreNotEqual(originalScene.name, newScene.name);
          Assert.AreEqual("ModeSelection", newScene.name);
          SceneManager.SetActiveScene(testScene);
          SceneManager.UnloadSceneAsync(newScene);
          yield return new WaitForSeconds(1f);
        }

        [UnityTest, Order(9)]
        public IEnumerator TestCreateAccountWithError()
        {
          usernameInput.text = "Test";
          passwordInput.text = "Test1234";
          passwordConfirmationInput.text = "Test1234";
          Scene originalScene = SceneManager.GetActiveScene();
          string errorMessageBefore = errorMessage.text;
          AbstractPlayerRetriever errorPlayerRetriever = new GameObject().AddComponent<ErrorPlayerRetriever>();
          loginManager.playerRetriever = errorPlayerRetriever;
          accountCreationManager.CreateAccount();
          yield return new WaitForSeconds(1f);
          Scene newScene = SceneManager.GetActiveScene();
          Assert.AreEqual(originalScene.name, newScene.name);
          Assert.AreNotEqual(errorMessageBefore, errorMessage.text);
        }

        [UnityTest, Order(10)]
        public IEnumerator TestCreateAccountWithErrorPasswordDontMatch()
        {
          usernameInput.text = "Test";
          passwordInput.text = "Test1234";
          passwordConfirmationInput.text = "test1234";
          Scene originalScene = SceneManager.GetActiveScene();
          string errorMessageBefore = errorMessage.text;
          AbstractPlayerRetriever errorPlayerRetriever = new GameObject().AddComponent<ErrorPlayerRetriever>();
          loginManager.playerRetriever = errorPlayerRetriever;
          accountCreationManager.CreateAccount();
          yield return new WaitForSeconds(1f);
          Scene newScene = SceneManager.GetActiveScene();
          Assert.AreEqual(originalScene.name, newScene.name);
          Assert.AreNotEqual(errorMessageBefore, errorMessage.text);
          Assert.AreEqual("Your passwords don't match", errorMessage.text);
        }

        [UnityTest, Order(10)]
        public IEnumerator TestCreateAccountWithErrorPasswordNotLongEnough()
        {
          usernameInput.text = "Test";
          passwordInput.text = "Test";
          passwordConfirmationInput.text = "Test";
          Scene originalScene = SceneManager.GetActiveScene();
          string errorMessageBefore = errorMessage.text;
          AbstractPlayerRetriever errorPlayerRetriever = new GameObject().AddComponent<ErrorPlayerRetriever>();
          loginManager.playerRetriever = errorPlayerRetriever;
          accountCreationManager.CreateAccount();
          yield return new WaitForSeconds(1f);
          Scene newScene = SceneManager.GetActiveScene();
          Assert.AreEqual(originalScene.name, newScene.name);
          Assert.AreNotEqual(errorMessageBefore, errorMessage.text);
          Assert.AreEqual("Your password needs to be at least 8 characters", errorMessage.text);
      }

    }
}
