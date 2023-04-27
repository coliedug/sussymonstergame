using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpike : MonoBehaviour
{
    private float fallDelay = 0f;
    private float respawnDelay = 5f;
    public float destroyDelay = 0.5f;
    float damageTick;
    GameObject player;
    Vector3 startingPosition;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] int contactDamage;
    //setting up variables
    void Start()
    {
        player = PlayerController.player;
        startingPosition = transform.position;
        print(startingPosition);
        gameObject.GetComponent<Collider2D>().enabled = true;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    //when the player collides with hitbox that causes the spike to fall
    private void OnTriggerEnter2D(Collider2D collision2D)
    {
        if (collision2D.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }

    //when the player collides with the spike itself
    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*TickDamage();
        gameObject.GetComponent<Collider2D>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;*/
        if (collision.gameObject.CompareTag("Player"))
        {
            TickDamage();
            gameObject.GetComponent<Collider2D>().enabled = false;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        else if(collision.gameObject.layer == 6)
        {
            print("collision with ground");
            gameObject.GetComponent<Collider2D>().enabled = false;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    //coroutine that causes spike to fall
    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(fallDelay);
        rb.bodyType = RigidbodyType2D.Dynamic;
        yield return new WaitForSeconds(respawnDelay);
        rb.bodyType = RigidbodyType2D.Kinematic;
        Instantiate(gameObject, startingPosition, Quaternion.identity);
        Destroy(gameObject, destroyDelay);
    }

    //function that causes player to take damage
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
