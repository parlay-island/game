using System.Linq;
using UnityEngine;

public class Answered30QuestionsAward : Award
{
    [SerializeField] public QuestionManager questionManager;
    [SerializeField] public GameObject medal;

    private void Start()
    {
        awardName = "gold medal";
    }

    public override bool WinsAward()
    {
        return questionManager.GetAnsweredQuestions().Count == 30;
    }

    public override void DisplayAward()
    {
        DisplayChosenAward(medal);
    }

    public override string GetAwardMessage()
    {
        return "Answered " + questionManager.GetAnsweredQuestions().Count + " Questions in 1 Game";
    }
}
