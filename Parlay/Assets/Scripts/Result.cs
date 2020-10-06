using System.Collections.Generic;
using Newtonsoft.Json;

[JsonObject]
public class ResultModel
{
	[JsonProperty("distance")] public float distance;
	[JsonProperty("level")] public int level;

	public ResultModel(float distance, int level) {
		this.distance = distance;
		this.level = level;
	}
}
