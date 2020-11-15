using System.Linq;
using UnityEngine;

/**
* This file holds the abstracted award object for answering 20 questions
* 
* @author: Andres Montoya
*/

public class Answered20QuestionsAward : Award
{
    [SerializeField] public QuestionManager questionManager;
    [SerializeField] public GameObject medal;
    private const string MEDAL_NAME = "silver medal";

    private void Start()
    {
        awardName = MEDAL_NAME;
    }

    public override bool WinsAward()
    {
        return questionManager.GetAnsweredQuestions().Count == 20;
    }

    public override void DisplayAward()
    {
        DisplayChosenAward(medal);
    }

    public override void hideAward()
    {
        medal.SetActive(false);
        text.text = "";
    }

    public override string GetAwardMessage()
    {
        return "Answered " + questionManager.GetAnsweredQuestions().Count + " Questions in 1 Game";
    }
}
