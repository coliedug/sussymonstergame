using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystemScript : MonoBehaviour
{
    [SerializeField] int health;

    public void ChangeHealth(int changeAmount)
    {
        Debug.Log(gameObject.name + ": I got hit");
        int oldHP = health;
        health += changeAmount;
        CheckStatus();
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
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
        Debug.Log("Destroying " + gameObject);
        Destroy(gameObject);
    }

    void ReturnColour()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
