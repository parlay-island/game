using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Range(-30f, -1f)][SerializeField] private float m_TimeReduction = -2f;

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
