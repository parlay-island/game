using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class AbstractWebRetriever : MonoBehaviour
{
    public abstract Task<List<QuestionModel>> GetQuestions();
}
