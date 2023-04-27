using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystemScript : MonoBehaviour
{
    [SerializeField] int health;
    [SerializeField] bool hammerRequired;
    bool isInvulnerable;

    private void Start()
    {
        if (gameObject == PlayerController.player) //Checks if this gameobject is the player, if so, updates the hp on the UI
        {
            UIScript.userInterface.UpdatePlayerHP(health);
        }
    }
    private void Update()
    {
       // Debug.Log(isInvulnerable);
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
        if (gameObject == PlayerController.player) //Checks if this gameobject is the player, if so, updates the hp on the UI
        {
            UIScript.userInterface.UpdatePlayerHP(health);
        }
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

    public void StartInvuln(float invulnTime)
    {
        Debug.Log("Starting Coroutine");
        StartCoroutine(ProcessInvuln(invulnTime));
    }
    IEnumerator ProcessInvuln(float invulnTime)
    {
        isInvulnerable = true;
        Debug.Log("Invuln");
        yield return new WaitForSeconds(invulnTime);
        isInvulnerable = false;
        Debug.Log("Not Invuln");
    }
}
