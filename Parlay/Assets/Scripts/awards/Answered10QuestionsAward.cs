using System.Linq;
using UnityEngine;

public class Answered10QuestionsAward : Award
{
    [SerializeField] public QuestionManager questionManager;
    
    public override bool WinsAward(int count)
    {
        return questionManager.GetAnsweredQuestions().Count == count;
    }

    public override string GetAwardMessage()
    {
        return "Answered " + questionManager.GetAnsweredQuestions().Count + " Questions in 1 Game";
    }
}
