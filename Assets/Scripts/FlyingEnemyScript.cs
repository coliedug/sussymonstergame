using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class FlyingEnemyScript : MonoBehaviour
{
    GameObject player;
    float damageTick;
    [SerializeField] int contactDamage;

    void Start()
    {
        player = PlayerController.player;
        GetComponent<AIDestinationSetter>().target = player.transform;
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
        if (damageTick >= 0.5f)
        {
            player.GetComponent<HealthSystemScript>().ChangeHealth(-contactDamage, false);
            damageTick = 0;
        }
    }
}
