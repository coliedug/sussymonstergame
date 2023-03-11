using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class FlyingEnemyScript : MonoBehaviour
{
    GameObject player;
    float damageTick;
    [SerializeField] int contactDamage;
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerController.player;
        GetComponent<AIDestinationSetter>().target = player.transform;
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
