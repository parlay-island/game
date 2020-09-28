using System.Collections.Generic;
using UnityEngine;

public class WebRetriever : MonoBehaviour
{
    private List<Question> _questions;
    
    void Start()
    {
        _questions = new List<Question>
        {
            new Question("What is opportunity cost?", new List<string> {"0", "1", "2", "3"}, 0)
        };
    }

    public List<Question> GetQuestions()
    {
        return _questions;
    }
}
