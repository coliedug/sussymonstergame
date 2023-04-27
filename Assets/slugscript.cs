using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slugscript : MonoBehaviour
{
    public float speed;
    private int startingPoint;
    [SerializeField] Transform[] points;
    float damageTick;
    [SerializeField] int contactDamage;
    GameObject player;

    private int i;

    void Start()
    {
        transform.position = points[startingPoint].position;
        player = PlayerController.player;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, points[i].position) < 0.2f)
        {
            i++;
            if (i == points.Length)
            {
                i = 0;
            }
            if(i == 0)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            if(i == 1)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
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
