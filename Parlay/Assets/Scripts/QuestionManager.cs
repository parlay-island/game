using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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

    public async void Start()
    {
        try
        {
            await RetrieveQuestionsIfNotAlreadySet();
        }
        catch (AggregateException aggregateException)
        {
            Debug.LogWarningFormat("An aggregate exception was caught that may include multiple exceptions");
            foreach (var exception in aggregateException.InnerExceptions)
            {
                LogSingleError(exception);
            }
        }
        
        catch (Exception exception)
        {
            errorDisplaySource.DisplayNewError("Cannot load questions", "An error occurred while loading " 
                            + "questions. Please try again later.");
            LogSingleError(exception);
        }
    }

    private async Task RetrieveQuestionsIfNotAlreadySet()
    {
        if (_unansweredQuestions == null || _unansweredQuestions.Count == 0)
        {
            var getQuestionsOrTimeout = GetQuestionsOrTimeoutBy(5000);
            _unansweredQuestions = await getQuestionsOrTimeout;
        }

        SetCurrentQuestion();
    }

    private static void LogSingleError(Exception exception)
    {
        Debug.LogWarningFormat("There was an error when loading questions [{0}]", exception);
    }

    private async Task<List<QuestionModel>> GetQuestionsOrTimeoutBy(int timeout)
    {
        Debug.LogFormat("Getting questions or timing out in {0} milliseconds", timeout);
        var apiRequestedQuestionTask = webRetriever.GetQuestions();
        using (var timeoutCancellationTokenSource = new CancellationTokenSource())
        {
            if (await Task.WhenAny(apiRequestedQuestionTask, 
                Task.Delay(timeout, timeoutCancellationTokenSource.Token)) == apiRequestedQuestionTask)
            {
                timeoutCancellationTokenSource.Cancel();
                if (apiRequestedQuestionTask.Exception != null) throw apiRequestedQuestionTask.Exception;
                return await apiRequestedQuestionTask;
            }

            throw new TimeoutException($"Questions were not retrieved within {timeout} seconds");
        }
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
