﻿using System.Collections.Generic;
using Newtonsoft.Json;

/**
 * This file manages all of teh result objects
 * These objects are used to store data that is sent and recieved through
 * api requests
 * 
 * @author: Holly Ansel, Jessica Su
 */

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
    [JsonProperty("award_list")] public List<string> award_list;

    public ResultModel(int level, float distance, int player_id, List<string> award_list, string player_name = "") {
		this.level = level;
		this.distance = distance;
		this.player_name = player_name;
		this.player_id = player_id;
        this.award_list = award_list;
	}
}

[JsonObject]
public class EndResult
{
	[JsonProperty("level")] public int level;
	[JsonProperty("distance")] public float distance;
	[JsonProperty("questions")] public List<AnsweredQuestion> questions;
    [JsonProperty("award_list")] public List<string> award_list;

    public EndResult(int level, float distance, List<AnsweredQuestion> questions, List<string> award_list)
	{
		this.level = level;
		this.distance = distance;
		this.questions = questions;
        this.award_list = award_list;
	}
}

[JsonObject]
public class AnsweredQuestion
{
	[JsonProperty("question_id")] public int question_id;
	[JsonProperty("choice_id")] public int choice_id;

	public AnsweredQuestion(int question_id, int choice_id)
	{
		this.question_id = question_id;
		this.choice_id = choice_id;
	}
}
