using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

// used to pass the level selected across scenes so it can be accessed in the game
public class Level : MonoBehaviour
{
  private LevelModel level;

  public void Awake()
  {
    DontDestroyOnLoad(gameObject);
  }

  public void SetLevel(LevelModel level_selected)
  {
    level = level_selected;
  }

  public int GetId(){
    return level.id;
  }

  public string GetName(){
    return level.name;
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
