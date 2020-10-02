using UnityEngine;
using UnityEngine.UI;

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
