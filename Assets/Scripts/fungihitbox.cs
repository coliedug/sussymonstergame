using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fungihitbox : MonoBehaviour
{
    GameObject player;
    float damageTick;
    [SerializeField] int contactDamage;

    public void Start()
    {
        player = PlayerController.player;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TickDamage();
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
