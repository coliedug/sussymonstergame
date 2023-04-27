using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PitsScript : MonoBehaviour
{
    GameObject player;
    [SerializeField] GameObject respawnObject;
    Transform respawnLocation;
    
    float damageTick;
    [SerializeField] int contactDamage;

    void Start()
    {
        player = PlayerController.player;
        respawnLocation = respawnObject.transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TickDamage();
            player.transform.position = respawnLocation.position;
        }
    }

    void TickDamage()
    {
        damageTick += Time.deltaTime;
        Debug.Log(damageTick);
        if (damageTick >= 0f)
        {
            player.GetComponent<HealthSystemScript>().ChangeHealth(-contactDamage, false);
            damageTick = 0;
        }
    }
}
