using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystemScript : MonoBehaviour
{
    [SerializeField] int health;
    Color normalColour;
    [SerializeField] bool hammerRequired;

    private void Start()
    {
        if (gameObject.GetComponent<SpriteRenderer>() != null)
        {
            normalColour = gameObject.GetComponent<SpriteRenderer>().color;
        }
        else
        {
            normalColour = gameObject.GetComponentInChildren<SpriteRenderer>().color;
        }
    }

    public void ChangeHealth(int changeAmount, bool hammerAttack)
    {
        if (hammerRequired && !hammerAttack)
        {
            return;
        }
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
            gameObject.GetComponent<SpriteRenderer>().color = normalColour;
        }
        else
        {
            gameObject.GetComponentInChildren<SpriteRenderer>().color = normalColour;
        }
    }
}
