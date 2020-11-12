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
    public GameManager gameManager;
    private Color32 originalColor;
    public Color32 green;
    public Color32 red;

    [SerializeField] public AbstractWebRetriever webRetriever;
    [SerializeField] public int timeReward;
    [SerializeField] public TextMeshProUGUI questionText;
    [SerializeField] public List<TextMeshProUGUI> choiceTexts;
    [SerializeField] public List<Image> choicePanels;
    [SerializeField] public ErrorDisplaySource errorDisplaySource;

    public void Start()
    {
        try
        {
            RetrieveQuestionsIfNotAlreadySet();
            gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            originalColor = new Color32(255, 255, 255, 100);
            ResetPanelColors();
            green = new Color32(152,251,152,150);
            red = new Color32(220,20,60, 150);
        }
        catch (Exception exception)
        {
            errorDisplaySource.DisplayNewError("Cannot load questions", "An error occurred while loading "
                            + "questions. Please try again later.");
        }
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
            UserSelect(0);
        if (Input.GetKeyUp(KeyCode.Alpha2))
            UserSelect(1);
        if (Input.GetKeyUp(KeyCode.Alpha3))
            UserSelect(2);
        if (Input.GetKeyUp(KeyCode.Alpha4))
            UserSelect(3);
    }

    public void ClearQuestions()
    {
      _unansweredQuestions = null;
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
      if(userChoice != 4)
      {
        Image choicePanel = choicePanels[userChoice];
        if (_currentQuestion.answers.Contains(userChoice))
        {
            choicePanel.color = green;
            timer.AddTime(timeReward);
            _answeredQuestions.Add(_currentQuestion);
            addQuestionToAnsweredQuestions(userChoice);
            Invoke("HideQuestion", 0.25f);
        } else if (GameManager.instance.retries.Count > 0 && GameManager.instance.canRetry)
        {
            choicePanel.color = red;
            GameManager.instance.retries.RemoveAt(0);
            addQuestionToAnsweredQuestions(userChoice);
            Invoke("ResetPanelColors", 0.25f);
        } else
        {
            choicePanel.color = red;
            _answeredQuestions.Add(_currentQuestion);
            addQuestionToAnsweredQuestions(userChoice);
            Invoke("HideQuestion", 0.25f);
        }
      } else {
            GameManager.instance.canRetry = !GameManager.instance.canRetry;
      }
    }

    private void ResetPanelColors()
    {
      SetCurrentQuestion();
      foreach(Image choicePanel in choicePanels)
      {
        choicePanel.color = originalColor;
      }
    }

    private void HideQuestion()
    {
      questionUI.SetActive(false);
      ResetPanelColors();
    }

    private void addQuestionToAnsweredQuestions(int userChoice)
    {
        AnsweredQuestion answeredQuestion = new AnsweredQuestion(_currentQuestion.id, userChoice);
        if (GameManager.instance)
        {
            GameManager.instance.addAnsweredQuestion(answeredQuestion);
        }
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
