using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystemScript : MonoBehaviour
{
    [SerializeField] int health;

    public void ChangeHealth(int changeAmount)
    {
        int oldHP = health;
        health += changeAmount;
        CheckStatus();
        if (gameObject.GetComponent<SpriteRenderer>() != null)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        }
        Invoke("ReturnColour", 0.1f);
    }

    void CheckStatus()
    {
        if (health <= 0)
        {
            Kill();
        }
    }

    void Kill()
    {
        Destroy(gameObject);
    }

    void ReturnColour()
    {
        if (gameObject.GetComponent<SpriteRenderer>() != null)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        }
    }
}
