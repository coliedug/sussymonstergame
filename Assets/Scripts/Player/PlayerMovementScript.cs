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

    public bool doubleJumpAvailable = false;

    int touchedGroundObjects;
    PlayerController pc;
    AnimationHandler animator;
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
        animator = GetComponent<AnimationHandler>();
    }

    // Update is called once per frame
    private void Update()
    {
        movementInput.x = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            JumpValidation();
        }
    }
    private void FixedUpdate()
    {
        NoInputDeceleration();
        HorizontalMovement();
        JumpArcCheck();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        status = PlayerGroundedCheck();
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        status = PlayerGroundedCheck();
    }

    // HORIZONTAL MOVEMENT //
    private void HorizontalMovement()
    {
        Vector2 acceleration = new Vector2(horAcceleration * Time.fixedDeltaTime * movementInput.x, 0);
        if (pc.currentChar == 1)
        {
            rb.AddForce(acceleration);
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -horMaxSpeed, horMaxSpeed), rb.velocity.y);
            //Debug.Log("Using character 1 values");
        }
        else if (pc.currentChar == 2)
        {
            rb.AddForce(acceleration * 1.5f);
            //Debug.Log("Using character 2 values");
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -1.5f*horMaxSpeed, 1.5f * horMaxSpeed), rb.velocity.y);
        }
        animator.TryMove(rb.velocity.x);
    }
    private void NoInputDeceleration()
    {
        if(movementInput.x == 0)
        {
            rb.AddForce(new Vector2(rb.velocity.x * Time.fixedDeltaTime * horDeceleration * -1, 0));
        }
    }
    private void JumpValidation()
    {
        if (status == States.Air)
        {
            if (doubleJumpAvailable && pc.currentChar == 2)
            {
                doubleJumpAvailable = false;
                ExecuteJump();
            }
        }
        else
        {
            ExecuteJump();
        }
    }

    private void ExecuteJump()
    {
        animator.currentState = AnimationHandler.states.jumping;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(new Vector2(0, jumpImpulse), ForceMode2D.Impulse);
        //animator.currentState = AnimationHandler.states.idle;
    }
    
    private void JumpArcCheck()
    {
        if(rb.velocity.y <= jumpDownGravStartHeight && rb.velocity.y != 0 && status == States.Air)
        {
            rb.AddForce(new Vector2(0, -jumpDownGrav));
        }
    }

    States PlayerGroundedCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), Vector2.down, 1f, groundMask);
        if (hit == false)
        {
            print("air");
            pc.status = PlayerController.States.Air;
            return(States.Air);
        }
        else
        {
            print("ground");
            pc.status = PlayerController.States.Ground;
            if(pc.currentChar == 2)
            {
                doubleJumpAvailable = true;
            }
            else
            {
                doubleJumpAvailable = false;
            }
            animator.currentState = AnimationHandler.states.idle;
            return (States.Ground);
        }
    }
}