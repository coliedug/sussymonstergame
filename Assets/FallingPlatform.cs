using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    private float fallDelay = 1f;
    private float respawnDelay = 5f;
    private float destroyDelay = 2f;
    GameObject player;
    Vector3 startingPosition;
    [SerializeField] private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerController.player;
        startingPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }

    //coroutine that changes rigidbody of the object, causing it to fall
    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(fallDelay);
        rb.bodyType = RigidbodyType2D.Dynamic;
        //after respawn delay, spawns self at starting location
        yield return new WaitForSeconds(respawnDelay);
        rb.bodyType = RigidbodyType2D.Kinematic;
        Instantiate(gameObject, startingPosition, Quaternion.identity);
        Destroy(gameObject, destroyDelay);
    }

}
