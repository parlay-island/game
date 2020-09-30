using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Range(-30f, -1f)][SerializeField] private float m_TimeReduction = -2f;
  	[SerializeField] private Timer m_Timer;

    void Update () {
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
      if(collision.gameObject.tag == "Player")
      {
        hitPlayer();
      }
    }

    private void hitPlayer()
    {
      m_Timer.AddTime(m_TimeReduction);
      Debug.Log(m_Timer.getCurrTime());
    }

}
