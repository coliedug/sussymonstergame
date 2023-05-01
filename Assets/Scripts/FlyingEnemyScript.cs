using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class FlyingEnemyScript : AIDestinationSetter
{
    GameObject player;
    float damageTick;
    [SerializeField] int contactDamage;
    
    //Shit Magnus did if not working just delete v
    Vector3 originalPosition;

    void Start()
    {
        player = PlayerController.player;
        GetComponent<AIDestinationSetter>().target = player.transform;
        target = player.transform;

        //Shit Magnus did if not working just delete v
        originalPosition = gameObject.transform.position;

    }
    private void FixedUpdate()
    {
        if(GetComponent<AIPath>().desiredVelocity.x >= 0.01f)
        {
            GetComponentInChildren<SpriteRenderer>().flipX = false;
        }
        else if (GetComponent<AIPath>().desiredVelocity.x <= -0.01f)
        {
            GetComponentInChildren<SpriteRenderer>().flipX = true;
        }
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject == player)
        {
            TickDamage();
        }
    }

    void TickDamage()
    {
        damageTick += Time.deltaTime;
        Debug.Log(damageTick);
        if (damageTick >= 0.2f)
        {
            player.GetComponent<HealthSystemScript>().ChangeHealth(-contactDamage, false);
            damageTick = 0;
        }
    }

    //Shit Magnus did if not working just delete v
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            GetComponent<AIPath>().canMove = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GetComponent<AIPath>().canMove = false;
            gameObject.transform.position = originalPosition;
        }
        
    }
}
