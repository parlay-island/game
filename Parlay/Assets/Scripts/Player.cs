using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Player : MonoBehaviour
{
  private PlayerModel player;
  private string auth_token;

  public void Awake()
  {
    auth_token = "";
  }

  public void SetAuthToken(string auth_token)
  {
      this.auth_token = auth_token;
  }

  public string GetAuthToken()
  {
      return auth_token;
  }

  public void SetPlayer(PlayerModel player_fetched)
  {
    this.player = player_fetched;
  }

  public int GetId(){
    return player?.id ?? 1;
  }

  public string GetName(){
    return player?.name ?? "";
  }
}

[JsonObject]
public class PlayerModel
{
    [JsonProperty("id")] public int id;
    [JsonProperty("name")] public string name;
    [JsonProperty("accuracy")] public float accuracy;

    public PlayerModel(int id, string name, float accuracy)
    {
        this.name = name;
        this.id = id;
        this.accuracy = accuracy;
    }
}


[JsonObject]
public class LoginModel
{
    [JsonProperty("username")] public string username;
    [JsonProperty("password")] public string password;

    public LoginModel(string username, string password)
    {
        this.username = username;
        this.password = password;
    }
}

[JsonObject]
public class LoginResponseModel
{
    [JsonProperty("auth_token")] public string auth_token;

    public LoginResponseModel(string auth_token)
    {
        this.auth_token = auth_token;
    }
}
