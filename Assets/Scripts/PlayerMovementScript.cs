using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 movementInput;

    [Header("Walking Values")]
    [SerializeField] float horMaxSpeed;
    [SerializeField] float horAcceleration;
    [SerializeField] float horDeceleration;

    [Header("Jump Values")]
    [SerializeField] float jumpImpulse;
    [SerializeField] float jumpDownGrav;
    [SerializeField] float jumpDownGravStartHeight;

    bool doubleJumpAvailable = false;

    int touchedGroundObjects;
    PlayerController pc;
    public enum States
    {
        Ground,
        Air,
        Side
    }
    States status;
    [SerializeField] LayerMask groundMask;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pc = PlayerController.player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    private void Update()
    {
        movementInput.x = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
        if (rb.velocity.x > 0)
        {
            pc.GetComponentInChildren<SpriteRenderer>().flipX = true;
        }
        else if (rb.velocity.x < 0)
        {
            pc.GetComponentInChildren<SpriteRenderer>().flipX = false;
        }
        status = PlayerGroundedCheck();
    }
    private void FixedUpdate()
    {
        NoInputDeceleration();
        HorizontalMovement();
        JumpArcCheck();
    }

    // HORIZONTAL MOVEMENT //
    private void HorizontalMovement()
    {
        Vector2 acceleration = new Vector2(horAcceleration * Time.fixedDeltaTime * movementInput.x, 0);
        rb.AddForce(acceleration);
        if (pc.currentChar == 1)
        {
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -horMaxSpeed, horMaxSpeed), rb.velocity.y);
        }
        else if (pc.currentChar == 2)
        {
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -horMaxSpeed * 1.5f, horMaxSpeed * 1.5f), rb.velocity.y);
        }
    }
    private void NoInputDeceleration()
    {
        if(movementInput.x == 0)
        {
            rb.AddForce(new Vector2(rb.velocity.x * Time.fixedDeltaTime * horDeceleration * -1, 0));
        }
    }
    private void Jump()
    {
        if (status == States.Air)
        {
            if (doubleJumpAvailable && pc.currentChar == 2)
            {
                doubleJumpAvailable = false;
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(new Vector2(0, jumpImpulse), ForceMode2D.Impulse);
            }
            else
            {
                //don't jump
            }
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, jumpImpulse), ForceMode2D.Impulse);
        }
    }
    
    private void JumpArcCheck()
    {
        if(rb.velocity.y <= jumpDownGravStartHeight && rb.velocity.y != 0 && status == States.Air)
        {
            Debug.Log("DownGravOn");
            rb.AddForce(new Vector2(0, -jumpDownGrav));
        }
    }

    States PlayerGroundedCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), Vector2.down, 1.1f, groundMask);
        if (hit == false)
        {
            pc.status = PlayerController.States.Air;
            return(States.Air);
        }
        else
        {
            pc.status = PlayerController.States.Ground;
            doubleJumpAvailable = true;
            return (States.Ground);
        }
    }
}



/*
States PlayerCollisionCheck(Collision2D collision)
{
    Vector2 relativePos = collision.GetContact(0).point - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
    float theta = Mathf.Atan(relativePos.y / relativePos.x);
    if (theta < 0)
    {
        theta += Mathf.PI * 2;
    }
    theta = theta * 180 / Mathf.PI; //Convert from radians to degrees
    if (relativePos.x < 0 && relativePos.y < 0)
    {
        theta += 180;
    }
    switch (theta)
    {
        case > 45 and < 135:
            return (States.Air);
        case > 240 and < 300:
            return (States.Ground);
        default:
            return (States.Side);

    }
}
*/