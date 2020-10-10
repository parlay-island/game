using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class QuestionManager : MonoBehaviour
{
    public GameObject questionUI;
    public Timer timer;
    private static List<QuestionModel> _unansweredQuestions;

    private QuestionModel _currentQuestion;

    [SerializeField] public AbstractWebRetriever webRetriever;
    [SerializeField] public int timeReward;
    [SerializeField] public Text questionText;
    [SerializeField] public List<Text> choiceTexts;
    [SerializeField] public ErrorDisplaySource errorDisplaySource;

    public void Start()
    {
        try
        {
            RetrieveQuestionsIfNotAlreadySet();
        }
        catch (Exception exception)
        {
            errorDisplaySource.DisplayNewError("Cannot load questions", "An error occurred while loading "
                            + "questions. Please try again later.");
        }
    }

    private void RetrieveQuestionsIfNotAlreadySet()
    {
        if (_unansweredQuestions == null || _unansweredQuestions.Count == 0)
        {
            _unansweredQuestions = webRetriever.GetQuestions();
            if(_unansweredQuestions.Count == 0)
            {
              throw new Exception();
            }
        }

        SetCurrentQuestion();
    }

    private void SetCurrentQuestion()
    {
        int randomQuestionIndex = Random.Range(0, _unansweredQuestions.Count - 1);
        _currentQuestion = _unansweredQuestions[randomQuestionIndex];

        questionText.text = _currentQuestion.body;

        for (int i = 0; i < choiceTexts.Count; i++)
        {
            choiceTexts[i].text = _currentQuestion.choices[i].body;
        }

        _unansweredQuestions.RemoveAt(randomQuestionIndex);
    }

    public void UserSelect(int userChoice)
    {
        if (_currentQuestion.answers.Contains(userChoice))
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
