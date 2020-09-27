using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionActivator : MonoBehaviour
{
    public GameObject questionUI;
    
    void OnCollisionEnter()
    {
        questionUI.SetActive(true);
    }
}
