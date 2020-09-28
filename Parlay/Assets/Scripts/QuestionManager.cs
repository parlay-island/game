using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    public List<Question> questions;
    public GameObject questionUI;
    public Timer timer;
    private static List<Question> _unansweredQuestions;

    private Question _currentQuestion;

    [SerializeField] private WebRetriever webRetriever;
    [SerializeField] private int timeReward;
    [SerializeField] private Text questionText;
    [SerializeField] private List<Text> choiceTexts;

    public async void Start()
    {
        if (_unansweredQuestions == null || _unansweredQuestions.Count == 0)
        {
            _unansweredQuestions = await webRetriever.GetQuestions();
        }

        SetCurrentQuestion();
    }

    private void SetCurrentQuestion()
    {
        int randomQuestionIndex = Random.Range(0, _unansweredQuestions.Count - 1);
        _currentQuestion = _unansweredQuestions[randomQuestionIndex];

        questionText.text = _currentQuestion.questionText;

        for (int i = 0; i < choiceTexts.Count; i++)
        {
            choiceTexts[i].text = _currentQuestion.choices[i];
        }

        _unansweredQuestions.RemoveAt(randomQuestionIndex);
    }

    public void UserSelect(int userChoice)
    {
        if (userChoice == _currentQuestion.correctChoice)
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
