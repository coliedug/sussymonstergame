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
}
