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

    PlayerController pc;

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
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -horMaxSpeed, horMaxSpeed), rb.velocity.y);
    }
    private void NoInputDeceleration()
    {
        if(movementInput.x == 0)
        {
            rb.AddForce(new Vector2(rb.velocity.x * Time.fixedDeltaTime * horDeceleration * -1, 0));
        }
    }


    // JUMP //
    private void Jump()
    {
        rb.AddForce(new Vector2(0, jumpImpulse), ForceMode2D.Impulse);
    }
    
    private void JumpArcCheck()
    {
        Debug.Log(rb.velocity.y);
        if(rb.velocity.y <= jumpDownGravStartHeight && rb.velocity.y != 0)
        {
            rb.AddForce(new Vector2(0, -jumpDownGrav));
        }
    }
}
