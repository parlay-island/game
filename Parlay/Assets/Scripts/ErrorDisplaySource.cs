using System;
using UnityEngine;
using UnityEngine.UI;

public class ErrorDisplaySource : MonoBehaviour
{
      [SerializeField] private Text errorTitle;
      [SerializeField] private Text errorMessage;
      [SerializeField] private GameObject errorMessageObject;
      
      public void DisplayNewError(string title, string message)
      {
            errorTitle.text = title;
            errorMessage.text = message;
            errorMessageObject.SetActive(true);
      }
}
