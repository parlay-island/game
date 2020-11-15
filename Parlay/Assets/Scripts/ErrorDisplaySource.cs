using UnityEngine;
using UnityEngine.UI;

/**
 * This file contains the object ErrorDisplaySource
 * This object is used to display error data across the app
 * 
 * @author: Jake Derry
 */

public class ErrorDisplaySource : MonoBehaviour
{
      [SerializeField] public Text errorTitle;
      [SerializeField] public Text errorMessage;
      [SerializeField] public GameObject errorMessageObject;
      
      public void DisplayNewError(string title, string message)
      {
            errorTitle.text = title;
            errorMessage.text = message;
            errorMessageObject.SetActive(true);
      }
}
