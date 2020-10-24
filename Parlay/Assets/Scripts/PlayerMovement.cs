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

    private const float PLAYER_RECOVERY_TIME = 1.5f;

    [SerializeField] public GameObject questionUI;
    public CharacterController2D controller;
	public Animator animator;

	public float runSpeed = 40f;

	  float horizontalMove = 0f;
	  bool jump = false;
    float distanceTravelled = 0;
    float lastPosition;
    private float hitTimer;
    private bool isHit;

    void Start() {
        lastPosition = transform.position.x;
        isHit = false;
    }

	void Update () {
    if(isHit)
    {
      hitTimer -= Time.deltaTime;
      if(hitTimer <= 0f)
      {
        isHit = false;
      }
    }
    bool movementAllowed = !questionUI.activeSelf;
    float moveSpeed = movementAllowed  ? runSpeed : 0f;
		horizontalMove = Input.GetAxisRaw("Horizontal") * moveSpeed;
		animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        if (Input.GetButtonDown("Jump") && controller.CanJump() && movementAllowed)
        {
            jump = true;
            animator.SetBool("IsJumping", true);
        }

        this.distanceTravelled += movementAllowed ? (transform.position.x - lastPosition) : 0;
        lastPosition = transform.position.x;
	}

    public float getDistanceTravelled() {
        return this.distanceTravelled;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Question")
        {
            questionUI.SetActive(true);
            Destroy(col.gameObject);
        }
        if (col.gameObject.tag == "FallDetector") 
        {
            if (GameManager.instance)
            {
                GameManager.instance.playerFallen = true;
            }
        }
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    public bool IsRecovering()
    {
      return isHit;
    }

    public void IsHit()
    {
      isHit = true;
      hitTimer = PLAYER_RECOVERY_TIME;
      animator.SetTrigger("IsHit");
    }

	void FixedUpdate ()
	{
		controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
		jump = false;
	}
}
