using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float m_TimeBoost;
    [SerializeField] private int type;
    [SerializeField] private float m_DistanceBoost;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (type == 1)
            {
                GameManager.instance.IncreaseTimeByPowerUp(this);
            }
            else if (type == 2)
            {
                print("Increase distance");
                GameManager.instance.IncreaseDistanceByPowerUp(this);
            }
            Destroy(gameObject);
        }
    }

    public float GetTimeBoost()
    {
        return m_TimeBoost;
    }

    public float GetDistanceBoost()
    {
        return m_DistanceBoost;
    }
}
