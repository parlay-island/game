﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* This file attaches the camera to the player object so that it follows the 
* player around as they traverse the terrain. It only changes its x direction
* along with the player as changing the y direction would disorient the user
* each time the player jumps.
* 
* @author: Andres Montoya
*/

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    private Transform playerTransform;
    Camera camera;

    public float offset;

    [SerializeField]
    public float sceneWidth = 10;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 temp = transform.position;
        temp.x = playerTransform.position.x;
        temp.x += offset;
        transform.position = temp;
    }

    // Adjust the camera's height so the desired scene width fits in view
    // even if the screen/window size changes dynamically.
    void Update()
    {
        float unitsPerPixel = sceneWidth / Screen.width;

        float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;

        camera.orthographicSize = desiredHalfHeight;
    }
}
