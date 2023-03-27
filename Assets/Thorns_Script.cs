using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorns_Script : MonoBehaviour
{
    //script is same as "FlyingEnemyScript", just without a.i. element (player takes damage upon colliding)
    GameObject player;
    float damageTick;
    [SerializeField] int contactDamage;
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerController.player;
    }

 
    void OnTriggerStay2D(Collider2D collision)
    {
        TickDamage();
        //couldn't get it to work to where it's confirms whether it's only the player entering trigger
        //would have to fix if in case of other colliders coming into contact with thorns causes player to take damage
        //works otherwise
        if (collision.gameObject.transform.parent == player)
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
