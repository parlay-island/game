using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class ModeRetriever : MonoBehaviour
{
    [SerializeField] public string apiUrl;
    private List<LevelModel> levels;


    void Start()
    {
      StartCoroutine(GetLevelsRequest(apiUrl));
    }

    public List<LevelModel> GetLevels()
    {
        return levels;
    }

    IEnumerator GetLevelsRequest(string uri)
    {
      UnityWebRequest webRequest = UnityWebRequest.Get(uri);
      webRequest.timeout = 5000;
      webRequest.SetRequestHeader("Content-Type", "application/json");
      yield return webRequest.SendWebRequest();
      if (webRequest.isNetworkError || webRequest.isHttpError)
      {
          Debug.LogWarningFormat("There was an error when loading levels [{0}]", webRequest.error);
      }
      else
      {
        string getString = webRequest.downloadHandler.text;
        LevelList levelsList = JsonConvert.DeserializeObject<LevelList>(getString);
        levels = levelsList.levels;
      }
    }
}

[JsonObject]
public class LevelList
{
  [JsonProperty("levels")] public List<LevelModel> levels;
}

[JsonObject]
public class LevelModel
{
    [JsonProperty("id")] public int id;
    [JsonProperty("name")] public string name;

    public LevelModel(int id, string name)
    {
        this.id = id;
        this.name = name;
    }
}
