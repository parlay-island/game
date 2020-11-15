using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

/**
* This file contains the mode retriever and its abstract class.
* The mode retriever queries the database for a list of all the different
* levels the player can choose from thus providing access to a list of levels.
* 
* @author: Holly Ansel
*/


public abstract class AbstractModeRetriever : MonoBehaviour
{
  public abstract List<LevelModel> GetLevels();
  public abstract bool IsLoading();
}

public class ModeRetriever : AbstractModeRetriever
{
    [SerializeField] private string apiUrl;
    private List<LevelModel> levels;
    private bool isLoading = false;


    void Awake()
    {
      string auth_token = GameObject.Find("PlayerInfo")?.GetComponent<PlayerAuth>()?.GetAuthToken() ?? "";
      StartCoroutine(GetLevelsRequest(apiUrl, auth_token));
    }

    public override bool IsLoading()
    {
        return isLoading;
    }

    public override List<LevelModel> GetLevels()
    {
        return levels;
    }

    IEnumerator GetLevelsRequest(string uri, string auth_token)
    {
      isLoading = true;
      UnityWebRequest webRequest = UnityWebRequest.Get(uri);
      webRequest.timeout = 5000;
      webRequest.SetRequestHeader("Content-Type", "application/json");
      webRequest.SetRequestHeader("Authorization", "Token " + auth_token);
      yield return webRequest.SendWebRequest();
      if (webRequest.isNetworkError || webRequest.isHttpError)
      {
          levels = null;
          Debug.LogWarningFormat("There was an error when loading levels [{0}]", webRequest.error);
      }
      else
      {
        string getString = webRequest.downloadHandler.text;
        LevelList levelsList = JsonConvert.DeserializeObject<LevelList>(getString);
        levels = levelsList.levels;
      }
      isLoading = false;
    }
}
