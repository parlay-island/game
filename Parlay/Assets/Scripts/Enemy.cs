using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This file contains the business logic for the Enemy object.
 * This has the enemy move randomly in a direction as long as there is ground beneath it.
 * If the enemy makes contact with the player object it has the Game Manager reduce the total time.
 * 
 * @author: Holly Ansel
 */

public class Enemy : MonoBehaviour
{
    [Range(-30f, -1f)][SerializeField] private float m_TimeReduction = -2f;
    [SerializeField] private LayerMask enemyMask;
    public Animator animator;
    public float speed = 5;
    private Rigidbody2D myBody;
    private Transform myTrans;
    private float myWidth, myHeight;

    void Awake()
  	{
          myBody = GetComponent<Rigidbody2D>();
          myTrans = transform;
          SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
          myWidth = mySprite.bounds.extents.x;
          myHeight = mySprite.bounds.extents.y;
  	}

    void FixedUpdate()
    {
      bool movementAllowed = !GameManager.instance.IsQuestionShown();
      Vector2 lineCastPos = myTrans.position - myTrans.right * myWidth;
      Vector2 transRight = new Vector2(myTrans.right.x, myTrans.right.y);
      bool isGrounded = Physics2D.Linecast(lineCastPos, lineCastPos + Vector2.down, enemyMask);
      bool isBlocked = Physics2D.Linecast(lineCastPos, lineCastPos - transRight * .1f, enemyMask);
      float moveSpeed = movementAllowed ? speed : 0f;
      if(!isGrounded || isBlocked)
      {
        Rotate();
      }
      Move(moveSpeed);
    }

    private void Rotate()
    {
      Vector3 currRotation = myTrans.eulerAngles;
      currRotation.y += 180;
      myTrans.eulerAngles = currRotation;

    }
    private void Move(float moveSpeed)
    {
      Vector2 myVel = myBody.velocity;
      myVel.x = -myTrans.right.x * moveSpeed;
      myBody.velocity = myVel;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
      Vector2 normal = collision.contacts[0].normal;
		  Vector2 topSide = new Vector2 (0f, -1f);
		  bool isTopHit = normal == topSide;
      PlayerMovement player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
      if(collision.gameObject.tag == "Player" && !player.IsRecovering())
      {
        if(!isTopHit)
        {
          Rotate();
          Move(speed);
          GameManager.instance.DeductTimeByEnemy(this);
          animator.SetTrigger("EnemyAttack");
          player.IsHit();
        }
        else
        {
          speed = 0;
          myBody.constraints = RigidbodyConstraints2D.None;
          Object.Destroy(gameObject.GetComponent<BoxCollider2D>());
          GameObject.Destroy(gameObject, 1f);
        }
      }
    }

    public float GetTimeReduction()
    {
      return m_TimeReduction;
    }

}
