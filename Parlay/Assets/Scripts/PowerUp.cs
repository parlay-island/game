using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] public float m_TimeBoost;
    [SerializeField] public int type;
    [SerializeField] public float m_DistanceBoost;

    private void Awake()
    {
        m_TimeBoost = Random.Range(10, 30);
        type = Random.Range(0, 4);
        m_DistanceBoost = Random.Range(100, 200);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (type == 1)
            {
                print("Increase time");
                GameManager.instance.IncreaseTimeByPowerUp(this);
            }
            else if (type == 2)
            {
                print("Increase distance");
                GameManager.instance.IncreaseDistanceByPowerUp(this);
            } else if (type == 3)
            {
                print("Retry PowerUp");
                GameManager.instance.AddRetry(this);
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
