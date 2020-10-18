using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float m_TimeBoost = 10f;

    void OnCollisionEnter()
    {
        GameManager.instance.IncreaseTimeByPowerUp(this);
        GameObject.Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            GameManager.instance.IncreaseTimeByPowerUp(this);
            Destroy(gameObject);
        }
    }

    public float GetTimeBoost()
    {
        return m_TimeBoost;
    }
}
