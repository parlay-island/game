using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    public List<Question> questions;
    public GameObject questionUI;
    public Timer timer;
    private static List<Question> unansweredQuestions;

    private Question currentQuestion;

    [SerializeField] private int timeReward;
    [SerializeField] private Text questionText;
    [SerializeField] private List<Text> choiceTexts;

    public void Start()
    {
        if (unansweredQuestions == null || unansweredQuestions.Count == 0)
        {
            unansweredQuestions = questions.ToList();
        }

        SetCurrentQuestion();
    }

    private void SetCurrentQuestion()
    {
        int randomQuestionIndex = Random.Range(0, unansweredQuestions.Count - 1);
        currentQuestion = unansweredQuestions[randomQuestionIndex];

        questionText.text = currentQuestion.questionText;

        for (int i = 0; i < choiceTexts.Count; i++)
        {
            choiceTexts[i].text = currentQuestion.choices[i];
        }

        unansweredQuestions.RemoveAt(randomQuestionIndex);
    }

    public void UserSelect(int userChoice)
    {
        if (userChoice == currentQuestion.correctChoice)
        {
            timer.AddTime(timeReward);
        }
            
        questionUI.SetActive(false);
    }

    public void SetTimeReward(int timeReward)
    {
        this.timeReward = timeReward;
    }
    
    public void SetQuestionText(Text questionText)
    {
        this.questionText = questionText;
    }
    
    public void SetChoiceTexts(List<Text> choiceTexts)
    {
        this.choiceTexts = choiceTexts;
    }
}
