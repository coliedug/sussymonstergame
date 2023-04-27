using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatform : MonoBehaviour
{
    public float speed;
    public int startingPoint;
    public Transform[] points;
    public bool flipped;
    GameObject Switch;

    private int i;

    void Start()
    {
        transform.position = points[startingPoint].position;
        Switch = GameObject.Find("Switch");
        switchScript cs = Switch.GetComponent<switchScript>();
        flipped = cs.flipped;
        print(flipped);
    }


    // Update is called once per frame
    void Update()
    {
        switchScript cs = Switch.GetComponent<switchScript>();
        flipped = cs.flipped;
        if (Vector2.Distance(transform.position, points[i].position) < 0.2f)
        {
            i++;
            if(i == points.Length)
            {
                i = 0;
            }
        }
        if (flipped == true)
        {
        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
        }
        
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        print("y");
        if(collision.transform.position.y > transform.position.y)
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        print("n");
        collision.transform.SetParent(null);
    }*/
}
