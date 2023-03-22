using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSystem : MonoBehaviour
{
    PickupSystem touchedPS;

    enum PickupType
    {
        Player,
        HealthPickup,
        Pickup2,
        Pickup3
    }
    [SerializeField] PickupType type;
    [SerializeField] int healthPickupHPChange = 1;

    private void Awake()
    {
        if(gameObject.GetComponent<PlayerController>() != null)
        {
            type = PickupType.Player;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Ends the script if the touched object is not a pickup
        if(!collision.gameObject.GetComponent<PickupSystem>())
        { return; }

        touchedPS = collision.gameObject.GetComponent<PickupSystem>();
        if (type == PickupType.Player)
        {
            PickupType pickedUpItemType = touchedPS.type;
            touchedPS.DestroyPickup();
            ProcessPickup(pickedUpItemType);
        }

    }
    void DestroyPickup()
    {
        Destroy(gameObject);
    }

    void ProcessPickup(PickupType inputType)
    {
        switch (inputType)
        {
            case PickupType.HealthPickup:
                gameObject.GetComponent<HealthSystemScript>().ChangeHealth(healthPickupHPChange, false);
                break;
            case PickupType.Pickup2:
                //Behaviour goes here
                break;
            case PickupType.Pickup3:
                //Behaviour goes here
                break;
        }
    }
}
