using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;

public class QuestionManager : MonoBehaviour
{
    public GameObject questionUI;
    public Timer timer;
    private static List<QuestionModel> _unansweredQuestions;
    private static List<QuestionModel> _allQuestions;
    private static readonly List<QuestionModel> _answeredQuestions = new List<QuestionModel>();

    private QuestionModel _currentQuestion;

    [SerializeField] public AbstractWebRetriever webRetriever;
    [SerializeField] public int timeReward;
    [SerializeField] public TextMeshProUGUI questionText;
    [SerializeField] public List<TextMeshProUGUI> choiceTexts;
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
            _allQuestions = webRetriever.GetQuestions();
            _unansweredQuestions = new List<QuestionModel>(_allQuestions);
            if(_unansweredQuestions.Count == 0)
            {
              throw new Exception();
            }
        }

        SetCurrentQuestion();
    }

    private void SetCurrentQuestion()
    {
        if (!_unansweredQuestions.Any()) _unansweredQuestions = new List<QuestionModel>(_allQuestions);
        int randomQuestionIndex = Random.Range(0, _unansweredQuestions.Count - 1);
        _currentQuestion = _unansweredQuestions[randomQuestionIndex];

        questionText.SetText(_currentQuestion.body);

        for (int i = 0; i < _currentQuestion.choices.Count; i++)
        {
            choiceTexts[i].SetText(_currentQuestion.choices[i].body);
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
        _answeredQuestions.Add(_currentQuestion);
        SetCurrentQuestion();
    }

    public void SetTimeReward(int timeReward)
    {
        this.timeReward = timeReward;
    }

    public void SetQuestionText(TextMeshProUGUI questionText)
    {
        this.questionText = questionText;
    }

    public void SetChoiceTexts(List<TextMeshProUGUI> choiceTexts)
    {
        this.choiceTexts = choiceTexts;
    }

    public List<QuestionModel> GetAnsweredQuestions()
    {
        return _answeredQuestions;
    }
}
