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
	[JsonProperty("answered_questions")] public List<AnsweredQuestion> answered_questions;

	public ResultModel(int level, float distance, int player_id, List<AnsweredQuestion> answered_questions, string player_name = "") {
		this.level = level;
		this.distance = distance;
		this.player_name = player_name;
		this.player_id = player_id;
		this.answered_questions = answered_questions;
	}
}

[JsonObject]
public class AnsweredQuestion
{
	[JsonProperty("question_id")] public int question_id;
	[JsonProperty("choice_selected")] public int choice_selected;

	public AnsweredQuestion(int question_id, int choice_selected)
	{
		this.question_id = question_id;
		this.choice_selected = choice_selected;
	}
}
