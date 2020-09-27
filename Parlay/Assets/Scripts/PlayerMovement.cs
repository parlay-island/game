using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* The PlayerMovement script takes the user's input and uses it to
* appropriately call the CharacterController2D and set the variables
* the control the animator transitions
* This code was inspired from a Brackey's tutorial
* https://www.youtube.com/watch?v=dwcT-Dch0bA
*/


public class PlayerMovement : MonoBehaviour {

	public CharacterController2D controller;
	public Animator animator;

	public float runSpeed = 40f;

	float horizontalMove = 0f;
	bool jump = false;

	void Update () {

		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
		animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
		if (Input.GetButtonDown("Jump") && controller.CanJump())
		{
			jump = true;
			animator.SetBool("IsJumping", true);
		}

	}

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

	void FixedUpdate ()
	{
		controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
		jump = false;
	}
}
