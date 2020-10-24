using System.Linq;
using UnityEngine;

public class Answered10QuestionsAward : Award
{
    [SerializeField] public QuestionManager questionManager;
    
    public override bool WinsAward()
    {
        return questionManager.GetAnsweredQuestions().Count >= 10;
    }

    public override string GetAwardMessage()
    {
        return "Answered 10 Questions in 1 Game";
    }
}
