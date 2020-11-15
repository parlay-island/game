using System.Linq;
using UnityEngine;

/**
* This file holds the abstracted award object for answering 30 questions
* 
* @author: Andres Montoya
*/

public class Answered30QuestionsAward : Award
{
    [SerializeField] public QuestionManager questionManager;
    [SerializeField] public GameObject medal;
    private const string MEDAL_NAME = "gold medal";

    private void Start()
    {
        awardName = MEDAL_NAME;
    }

    public override bool WinsAward()
    {
        return questionManager.GetAnsweredQuestions().Count == 30;
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
