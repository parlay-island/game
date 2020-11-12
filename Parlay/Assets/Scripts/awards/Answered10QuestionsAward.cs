using System.Linq;
using UnityEngine;

public class Answered10QuestionsAward : Award
{
    [SerializeField] public QuestionManager questionManager;
    [SerializeField] public GameObject medal;

    private void Start()
    {
        awardName = "bronze medal";
    }

    public override bool WinsAward()
    {
        return questionManager.GetAnsweredQuestions().Count == 10;
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
