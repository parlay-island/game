
using System.Collections.Generic;

[System.Serializable]
public class Question
{
    public string questionText;
    public List<string> choices;
    public int correctChoice;

    public Question(string questionText, List<string> choices, int correctChoice)
    {
        this.questionText = questionText;
        this.choices = choices;
        this.correctChoice = correctChoice;
    }
}
