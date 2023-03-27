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
        if (gameObject == PlayerController.player) //Checks if this gameobject is the player, if so, updates the hp on the UI
        {
            UIScript.userInterface.UpdatePlayerHP(health);
        }
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
        Debug.Log("Changing health from " + health + " to " + (health + changeAmount));
        int oldHP = health;
        health += changeAmount;
        CheckStatus();
        if (gameObject == PlayerController.player) //Checks if this gameobject is the player, if so, updates the hp on the UI
        {
            UIScript.userInterface.UpdatePlayerHP(health);
        }
        if (gameObject.GetComponent<SpriteRenderer>() != null)
        {
            if (changeAmount > 0)
            {
                gameObject.GetComponent<SpriteRenderer>().color = Color.green;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
        else
        {
            if (changeAmount > 0)
            {
                gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.green;
            }
            else
            {
                gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.red;
            }
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
