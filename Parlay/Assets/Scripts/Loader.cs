using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
	public GameObject gameManager;
	public float gameTime = 10f;

    // Start is called before the first frame update
    void Awake()
    {
        if (GameManager.instance == null) {
    		Instantiate (gameManager);
    	}
    	GameManager.instance.setGameTime(gameTime);
    }
}
