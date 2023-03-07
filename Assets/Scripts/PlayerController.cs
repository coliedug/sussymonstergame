using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public static GameObject player { get; private set; }
    Rigidbody2D rb;
    [SerializeField] int moveSpeed = 10;
    Vector2 movement;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Sprite[] sprites;
    [SerializeField] int currentChar = 1;
    [SerializeField] int jumpheight = 10;
    int mainAttackDamage = 1;
    bool facingLeft = false;
    bool doubleJumpAvailable = true;
    bool dashAvailable = true;
    [SerializeField] int dashLength;
    [SerializeField] float maxMoveSpeed;
    [SerializeField] ParticleSystem[] ps;
    enum States
    {
        Ground,
        Air
    }
    States status;

    void Awake()
    {
        SetPlayerReference();
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SwitchCharacter();
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        status = States.Ground;
        doubleJumpAvailable = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        status = States.Air;
    }
    void Update()
    {
        GetComponentInChildren<TextMeshPro>().SetText(status.ToString());
        CheckInputs();
    }
    private void FixedUpdate()
    {
        ProcessMovement();
        if (movement.x < 0)
        {
            facingLeft = true;
            gameObject.GetComponentInChildren<SpriteRenderer>().flipX = true;
        }
        else if (movement.x > 0)
        {
            facingLeft = false;
            gameObject.GetComponentInChildren<SpriteRenderer>().flipX = false;
        }
        if (rb.velocity.x > maxMoveSpeed | rb.velocity.x < -maxMoveSpeed)
        {
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -moveSpeed, moveSpeed), rb.velocity.y);
        }
        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    void SetPlayerReference()
    {
        if (player == null)
        {
            player = gameObject;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void CheckInputs()
    {
        if(Input.GetButtonDown("Jump") && doubleJumpAvailable)
        {
            if (status == States.Air)
            {
                doubleJumpAvailable = false;
            }
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, jumpheight));
        }
        if(Input.GetButtonDown("Fire1"))
        {
            MainAttack();
        }
        if (Input.GetButtonDown("Fire2"))
        {
            SecondaryAttack();
        }
        movement.x = Input.GetAxisRaw("Horizontal");
        if(Input.GetKeyDown(KeyCode.E))
        {
            if (currentChar < 2)
            {
                currentChar++;
            }
            else
            {
                currentChar = 1;
            }
            SwitchCharacter();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Dash();
        }
    }
    void ProcessMovement()
    {
        rb.AddForce(movement.normalized * 300 * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }
    void SwitchCharacter()
    {
        switch(currentChar)
        {
            case 1:
                sr.sprite = sprites[0];
                moveSpeed = 4;
                mainAttackDamage = 5;
                break;
            case 2:
                sr.sprite = sprites[1];
                moveSpeed = 8;
                mainAttackDamage = 2;
                break;
        }
    }
    void MainAttack()
    {
        RaycastHit2D hit = CastRayAtSide(5);
        if (hit.collider != null)
        {
            Debug.Log("Hit " + hit.collider.gameObject.name);
            if (hit.collider.gameObject.GetComponent<HealthSystemScript>() != null)
            {
                hit.collider.gameObject.GetComponent<HealthSystemScript>().ChangeHealth(-mainAttackDamage);
            }
        }
    }
    void SecondaryAttack()
    {
        if (currentChar == 1)
        {
            if(facingLeft)
            {
                ps[0].Play();
            }
            else
            {
                ps[1].Play();
            }
        }
    }
    void Dash()
    {
        if (!dashAvailable | currentChar == 1)
        {
            return;
        }

        RaycastHit2D hit = CastRayAtSide(dashLength);
        if (hit.collider != null)
        {
            Debug.Log("dash hit wall");
            Vector2 dashLocation = hit.collider.transform.position;
            rb.MovePosition(dashLocation);
        }
        else
        {
            float facedDirectionOffset;
            if (facingLeft)
            {
                facedDirectionOffset = -1;
            }
            else
            {
                facedDirectionOffset = 1;
            }
            Vector2 dashLocation = transform.position + transform.right * facedDirectionOffset * dashLength;
            rb.MovePosition(dashLocation);
        }
    }
    RaycastHit2D CastRayAtSide(float rayLength)
    {
        Debug.Log("Casting Ray");
        float facedDirectionOffset;
        if (facingLeft)
        {
            facedDirectionOffset = -1;
        }
        else
        {
            facedDirectionOffset = 1;
        }
        Debug.DrawRay(gameObject.transform.position, new Vector3(rayLength * facedDirectionOffset, 0), Color.red, 1f);
        Vector3 rayStart = gameObject.transform.position + new Vector3(0.6f * facedDirectionOffset, 0, 0);
        RaycastHit2D hit = Physics2D.Raycast(rayStart, transform.right * facedDirectionOffset, rayLength);
        return (hit);
    }
}

