using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This file is used to detect whether the question mark tile has collided
 * with an object. This allows the questionUI to activiate when it collides with
 * the player.
 * 
 * @author: Jake Derry
 */

public class QuestionActivator : MonoBehaviour
{
    public GameObject questionUI;
    
    void OnCollisionEnter()
    {
        questionUI.SetActive(true);
    }
}
