using System.Collections.Generic;
using Newtonsoft.Json;

[JsonObject]
public class ResultList
{
    [JsonProperty("results")] public List<ResultModel> results;
}

[JsonObject]
public class ResultModel
{
	[JsonProperty("id")] public int id;
	[JsonProperty("level")] public int level;
	[JsonProperty("distance")] public float distance;
	[JsonProperty("player_id")] public int player_id;
	[JsonProperty("player_name")] public string player_name;

	public ResultModel(int level, float distance, int player_id, string player_name = "") {
    this.level = level;
		this.distance = distance;
		this.player_name = player_name;
    this.player_id = player_id;
	}
}
