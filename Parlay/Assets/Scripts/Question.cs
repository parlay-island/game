
using System.Collections.Generic;
using Newtonsoft.Json;

/**
 * This file is used to hold Question and Choice objects.
 * These are used to store question data as well as choice data.
 * 
 * @author: Jake Derry
 */

[JsonObject]
public class QuestionList
{
    [JsonProperty("questions")] public List<QuestionModel> questions;
}

[JsonObject]
public class QuestionModel
{
    [JsonProperty("id")] public int id;
    [JsonProperty("body")] public string body;
    [JsonProperty("tags")] public List<string> tags;
    [JsonProperty("timesAnswered")] public int timesAnswered;
    [JsonProperty("timesCorrect")] public int timesCorrect;
    [JsonProperty("choices")] public List<ChoiceModel> choices;
    [JsonProperty("answer")] public List<int> answers;

    public QuestionModel(string body, List<ChoiceModel> choices, List<int> answers)
    {
        this.body = body;
        this.choices = choices;
        this.answers = answers;
    }
}

[JsonObject]
public class ChoiceModel
{
    [JsonProperty("id")] public int id;
    [JsonProperty("body")] public string body;
    [JsonProperty("timesChosen")] public int timesChosen;

    public ChoiceModel(string body)
    {
        this.body = body;
    }
}