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
        Invuln,
        Coin
    }
    [SerializeField] PickupType type;
    [SerializeField] int healthPickupHPChange = 1;
    [SerializeField] float invulnTime;
    [SerializeField] int coinScoreAmount = 10;

    [SerializeField] AudioSource soundPlayer;
    [SerializeField] AudioClip pickupSound;

    private void Awake()
    {
        if(gameObject.GetComponent<PlayerController>() != null)
        {
            type = PickupType.Player;
        }
    }
    private void Start()
    {
        if (gameObject != PlayerController.player)
        {
            soundPlayer = PlayerController.player.GetComponent<AudioSource>();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Ends the script if the touched object is not a pickup
        if(!collision.gameObject.GetComponent<PickupSystem>())
        { return; }

        touchedPS = collision.gameObject.GetComponent<PickupSystem>();
        if (type == PickupType.Player)
        {
            PickupType pickedUpItemType = touchedPS.type;
            soundPlayer.PlayOneShot(pickupSound);
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
                gameObject.GetComponent<HealthSystemScript>().ChangeHealth(touchedPS.healthPickupHPChange, false);
                break;
            case PickupType.Invuln:
                gameObject.GetComponent<HealthSystemScript>().StartInvuln(touchedPS.invulnTime);
                break;
            case PickupType.Coin:
                ScoreSystem.scoreSystem.UpdateScore(touchedPS.coinScoreAmount);
                break;
        }
    }
}
