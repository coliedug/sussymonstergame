using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wheelplatform : MonoBehaviour
{
    [SerializeField] GameObject wheel;
    public float zRotate;
    void Start()
    {
        wheelscript cs = wheel.GetComponent<wheelscript>();
        zRotate = cs.zRotate;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(0, 0, -zRotate, Space.Self);
    }

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
