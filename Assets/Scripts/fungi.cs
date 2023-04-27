using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fungi : MonoBehaviour
{
    public Animator animator;
    public float fungiAcceleration = 0.25f;
    public float explosionPoint = 5f;
    public float animationspeed;
    public bool explode = false;
    GameObject explodeHitbox;

    private void Start()
    {
        explodeHitbox = GameObject.Find("explosionhitbox");
    }

    private void Update()
    {
        if(animationspeed >= 5f)
        {
            explodeHitbox.SetActive(true);
        }
        else
        {
            explodeHitbox.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(animator.speed < explosionPoint)
            {
                animator.speed = animator.speed + fungiAcceleration;
                animationspeed = animator.speed;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animator.speed = 1;
            animationspeed = animator.speed;
        }
    }
}
