using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Range(-30f, -1f)][SerializeField] private float m_TimeReduction = -2f;
    [SerializeField] private LayerMask enemyMask;
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
      Vector2 lineCastPos = myTrans.position - myTrans.right * myWidth;
      Vector2 transRight = new Vector2(myTrans.right.x, myTrans.right.y);
      bool isGrounded = Physics2D.Linecast(lineCastPos, lineCastPos + Vector2.down, enemyMask);
      bool isBlocked = Physics2D.Linecast(lineCastPos, lineCastPos - transRight * .1f, enemyMask);

      if(!isGrounded || isBlocked)
      {
        Vector3 currRotation = myTrans.eulerAngles;
        currRotation.y += 180;
        myTrans.eulerAngles = currRotation;
      }
      Vector2 myVel = myBody.velocity;
      myVel.x = -myTrans.right.x * speed;
      myBody.velocity = myVel;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
      Vector2 normal = collision.contacts[0].normal;
		  Vector2 topSide = new Vector2 (0f, -1f);
		  bool isTopHit = normal == topSide;
      if(collision.gameObject.tag == "Player")
      {
        if(!isTopHit)
          GameManager.instance.DeductTimeByEnemy(this);
        else
        {
          speed = 0;
          myBody.constraints = RigidbodyConstraints2D.None;
          GameObject.Destroy(gameObject.GetComponent<BoxCollider2D>());
          GameObject.Destroy(gameObject, 1f);
        }

      }
    }

    public float GetTimeReduction()
    {
      return m_TimeReduction;
    }

}
