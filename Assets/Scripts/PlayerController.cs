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
    [SerializeField] int dashDamage;
    [SerializeField] float maxMoveSpeed;
    [SerializeField] ParticleSystem[] ps;
    [SerializeField] float slamRadius;
    [SerializeField] int slamDamage;
    [SerializeField] LayerMask enemyMask;
    float facedDirectionOffset;
    int touchedObjects;
    float dashCooldown;
    float slamCharge;
    enum States
    {
        Ground,
        Air,
        Side
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
        if (collision.gameObject.layer == 7)
        { return; }
        touchedObjects++;
        status = PlayerCollisionCheck(collision);
        if (status != States.Air && currentChar == 2)
        {
            doubleJumpAvailable = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7)
        { return; }
        touchedObjects--;
        if (touchedObjects == 0)
        {
            status = States.Air;
        }
    }
    void Update()
    {
        GetComponentInChildren<TextMeshPro>().SetText(status.ToString());
        CheckInputs();
        if (!dashAvailable)
        {
            dashCooldown += Time.deltaTime;
        }
        if (dashCooldown >= 3)
        {
            dashAvailable = true;
        }
        if (slamCharge > 1)
        {
            Debug.Log("Slam Ready");
        }
    }
    private void FixedUpdate()
    { //This is where the of the physics based update calculations, it's all just rigidbody movement basically.
        ProcessMovement();
        if (movement.x < 0)
        {
            facingLeft = true;
            facedDirectionOffset = -1f;
            gameObject.GetComponentInChildren<SpriteRenderer>().flipX = true;
        }
        else if (movement.x > 0)
        {
            facingLeft = false;
            facedDirectionOffset = 1;
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
    {//This just sets a globally accessible variable for other scrips to grab if they need to access the player gameobject
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
    {//This is where player input is processed, it's called every update
        if(Input.GetButtonDown("Jump"))
        {
            if (status != States.Air)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(new Vector2(0, jumpheight));
            }
            else if (currentChar == 2 && doubleJumpAvailable)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(new Vector2(0, jumpheight));
                doubleJumpAvailable = false;
            }
        }
        if(Input.GetButtonDown("Fire1"))
        {
            MainAttack();
        }
        if (Input.GetButton("Fire2"))
        {
            SecondaryAttack();
        }
        if (Input.GetButtonDown("Fire2"))
        {
            slamCharge = 0;
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
        if (Input.GetButtonUp("Fire2"))
        {
            if(facingLeft)
            {
                Slam(0);
            }
            else
            {
                Slam(1);
            }
        }

    }
    void ProcessMovement()
    {
        rb.AddForce(movement.normalized * 300 * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }
    void SwitchCharacter()
    {//This is the main function for switching characters, it changes all the variables that need changed.
        switch(currentChar)
        {
            case 1:
                sr.sprite = sprites[0];
                moveSpeed = 4;
                mainAttackDamage = 5;
                doubleJumpAvailable = false;
                break;
            case 2:
                sr.sprite = sprites[1];
                moveSpeed = 8;
                mainAttackDamage = 2;
                doubleJumpAvailable = true;
                break;
        }
    }
    void MainAttack()
    {//The main attack, right now it's just a straight ray in the direction the player is facing, later on I'll add an actual attack profile, depending on
     //which character is attacking, like the tank will have a bigger swing etc.
        RaycastHit2D hit = CastRayAtSide(5);
        if (hit.collider != null)
        {
            Debug.Log("Hit " + hit.collider.gameObject.name);
            if (hit.collider.gameObject.GetComponent<HealthSystemScript>() != null)
            {
                hit.collider.gameObject.GetComponent<HealthSystemScript>().ChangeHealth(-mainAttackDamage, false);
            }
        }
    }
    void SecondaryAttack()
    {//This just checks which character you're on and calls the slam or dash when you right click, depending on said character.
        if (currentChar == 1)
        {
            slamCharge += Time.deltaTime;
        }
        else
        {
            Dash();
        }
    }
    void Slam(int facedDirection)
    {//This is the tank's slam attack, it uses an overlap circle to grab all the hit colliders with the right layermask, cast centred on a gameobject attached
        //to the player gameobject which has the particle effects.
        if (Input.GetButtonUp("Fire2") && slamCharge > 1)
        {
            ps[facedDirection].Play();
            Collider2D[] hit = Physics2D.OverlapCircleAll(ps[facedDirection].gameObject.transform.position, slamRadius, enemyMask);
            foreach (Collider2D i in hit)
            {
                i.gameObject.GetComponent<HealthSystemScript>().ChangeHealth(-slamDamage, true);
                if (i.GetComponent<Rigidbody2D>() != null)
                {
                    Vector2 forceDirection = i.transform.position - ps[facedDirection].gameObject.transform.position;
                    i.gameObject.GetComponent<Rigidbody2D>().AddForce(forceDirection * 100);
                }
            }
        }
    }
    void Dash()
    {
        if (!dashAvailable | currentChar == 1)
        {
            return;
        }
        RaycastHit2D[] hits = Physics2D.CircleCastAll(gameObject.transform.position + new Vector3(0.6f * facedDirectionOffset, 0), 2, transform.right * facedDirectionOffset, dashLength);
        foreach (RaycastHit2D i in hits)
        {
            if (i.collider.GetComponent<HealthSystemScript>() != null)
            {
                i.collider.GetComponent<HealthSystemScript>().ChangeHealth(-dashDamage, false);
            }
            else
            {
                Vector2 dashLocation = i.collider.transform.position;
                rb.MovePosition(dashLocation);
                return;
            }
        }
        rb.MovePosition(gameObject.transform.position + new Vector3(dashLength * facedDirectionOffset, 0, 0));
        dashAvailable = false;

    }
    RaycastHit2D CastRayAtSide(float rayLength)
    {
        Debug.Log("Casting Ray");
        Debug.DrawRay(gameObject.transform.position, new Vector3(rayLength * facedDirectionOffset, 0), Color.red, 1f);
        Vector3 rayStart = gameObject.transform.position + new Vector3(0.6f * facedDirectionOffset, 0, 0);
        RaycastHit2D hit = Physics2D.Raycast(rayStart, transform.right * facedDirectionOffset, rayLength);
        return (hit);
    }
    States PlayerCollisionCheck(Collision2D collision)
    {
        Vector2 relativePos = collision.GetContact(0).point - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        float theta = Mathf.Atan(relativePos.y / relativePos.x);
        if (theta < 0)
        {
            theta += Mathf.PI * 2;
        }
        theta = theta * 180 / Mathf.PI; //Convert from radians to degrees
        Debug.Log("Theta: " + theta);
        switch (theta)
        {
            case >45 and <135:
                return(States.Air);
            case >240 and < 300:
                return (States.Ground);
            default:
                return (States.Side);

        }
    }
}

