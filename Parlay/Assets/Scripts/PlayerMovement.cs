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

    [SerializeField] public GameObject questionUI;
    public CharacterController2D controller;
	public Animator animator;

	public float runSpeed = 40f;

	float horizontalMove = 0f;
	bool jump = false;
    float distanceTravelled = 0;
    float lastPosition;

    void Start() {
        lastPosition = transform.position.x;
    }

	void Update () {
    bool movementAllowed = !questionUI.activeSelf;
    float moveSpeed = movementAllowed  ? runSpeed : 0f;
		horizontalMove = Input.GetAxisRaw("Horizontal") * moveSpeed;
		animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        if (Input.GetButtonDown("Jump") && controller.CanJump() && movementAllowed)
        {
            jump = true;
            animator.SetBool("IsJumping", true);
        }

        distanceTravelled += movementAllowed ? (transform.position.x - lastPosition) : 0;
        lastPosition = transform.position.x;
	}

    public float getDistanceTravelled() {
        return distanceTravelled;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("hit question");
        if (col.gameObject.tag == "Question")
        {
            Debug.Log("hit question");
            questionUI.SetActive(true);
            Destroy(col.gameObject);
        }
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    public void IsHit()
    {
      animator.SetTrigger("IsHit");
    }

	void FixedUpdate ()
	{
		controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
		jump = false;
	}
}
