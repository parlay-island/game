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

        public class MockPlayerRetriever: AbstractPlayerRetriever
        {
          public override void LoginPlayer(LoginModel loginModel, System.Action successCallback, System.Action<string> errorCallback, Player player)
          {
            PlayerModel player_fetched = new PlayerModel(testPlayerId, loginModel.username, testPlayerAccuracy);
            player.SetPlayer(player_fetched);
            successCallback();
          }
          public override void LogoutPlayer(System.Action successCallback, System.Action<string> errorCallback, Player player)
          {
            GameObject.Destroy(player);
            successCallback();
          }
        }

        public class ErrorPlayerRetriever: AbstractPlayerRetriever
        {
          public override void LoginPlayer(LoginModel loginModel, System.Action successCallback, System.Action<string> errorCallback, Player player)
          {
            errorCallback("error in test for player retriever");
          }
          public override void LogoutPlayer(System.Action successCallback, System.Action<string> errorCallback, Player player)
          {
            errorCallback("error in test for player retriever");
          }
        }

        private LoginManager loginManager;
        private Logout logoutManager;
        private Player player;
        private TextMeshProUGUI errorMessage;
        private AbstractPlayerRetriever playerRetriever;
        private Scene testScene;
        private TMP_InputField usernameInput;

        [OneTimeSetUp]
        public void Init()
        {
          testScene = SceneManager.GetActiveScene();
        }

        [SetUp]
        public void Setup()
        {
          GameObject playerObj = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/PlayerInfo"));
          playerObj.name = "PlayerInfo";
          player = playerObj.GetComponent<Player>();
          usernameInput = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Login/Username")).GetComponent<TMP_InputField>();
          usernameInput.text = "";
          errorMessage = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Login/ErrorMessage")).GetComponent<TextMeshProUGUI>();
          errorMessage.SetText("");
          errorMessage.faceColor = new Color32(255, 0, 0, 255);
          loginManager = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Login/LoginManager")).GetComponent<LoginManager>();
          loginManager.player = player;
          loginManager.errorMessage = errorMessage;
          loginManager.usernameInput = usernameInput;
          loginManager.isTest = true;
          logoutManager = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/LogoutButton")).GetComponent<Logout>();
          logoutManager.player = player;
          logoutManager.isTest = true;
          playerRetriever = new GameObject().AddComponent<MockPlayerRetriever>();
          loginManager.playerRetriever = playerRetriever;
          logoutManager.playerRetriever = playerRetriever;
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
          Assert.AreEqual(player.GetId(), testPlayerId);
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
          Assert.AreEqual(player.GetName(), usernameInput.text);
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

    }
}
