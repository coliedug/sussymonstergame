using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatform : MonoBehaviour
{
    public float speed;
    public int startingPoint;
    public Transform[] points;
    public bool flipped;
    [SerializeField] GameObject Switch;

    private int i;

    void Start()
    {
        transform.position = points[startingPoint].position;
    }

    void Update()
    {
        /*if(Switch == null)
        {
            if (Vector2.Distance(transform.position, points[i].position) < 0.2f)
            {
                i++;
                if (i == points.Length)
                {
                    i = 0;
                }
            }
            transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
        }
        else
        {
            switchScript cs = Switch.GetComponent<switchScript>();
            flipped = cs.flipped;
            if (Vector2.Distance(transform.position, points[i].position) < 0.2f)
            {
                i++;
                if (i == points.Length)
                {
                    i = 0;
                }
            }
            if (flipped == true)
            {
                transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
            }

            
        }*/

        
        
        
    }

    private void FixedUpdate()
    {
        if (Switch == null)
        {
            if (Vector2.Distance(transform.position, points[i].position) < 0.2f)
            {
                i++;
                if (i == points.Length)
                {
                    i = 0;
                }
            }
            transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.fixedDeltaTime);
        }
        else
        {
            switchScript cs = Switch.GetComponent<switchScript>();
            flipped = cs.flipped;
            if (Vector2.Distance(transform.position, points[i].position) < 0.2f)
            {
                i++;
                if (i == points.Length)
                {
                    i = 0;
                }
            }
            if (flipped == true)
            {
                transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.fixedDeltaTime);
            }


        }
    }

    /*private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.transform.position.y > transform.position.y)
        {
            collision.transform.SetParent(transform);
            print("start collision");
        }
    }*/


    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null);
        print("end collision");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.transform.position.y > transform.position.y)
        {
            collision.transform.SetParent(transform);
            print("start collision");
        }
    }

}

