using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This file loads the game manager at the start of the game
 * It also contains a helper function to reset the game 
 * 
 * @author: Holly Ansel, Jessica Su, Andres Montoya
 */

public class Loader : MonoBehaviour
{
	public GameObject gameManager;
	public float gameTime = 30f;

    // Start is called before the first frame update
    void Awake()
    {
        if (GameManager.instance == null) {
    		Instantiate (gameManager);
    	} 
    	GameManager.instance.setGameTime(gameTime);
    }

    public void Reset()
    {
        GameManager.instance.Reset(gameTime);
        print("Resetting game");
    }
}
