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
    enum States
    {
        Ground,
        Latched,
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
        if (status != States.Latched)
        {
            status = States.Ground;
        }
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
        if(Input.GetButtonDown("Jump") && status != States.Air)
        {
            Debug.Log("Jump");
            rb.AddForce(new Vector2(0, jumpheight * 1000));
        }
        if(Input.GetButtonDown("Fire1"))
        {
            MainAttack();
        }
        movement.x = Input.GetAxisRaw("Horizontal");
        if(Input.GetButtonDown("Fire2"))
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
    }
    void ProcessMovement()
    {
        rb.velocity = movement.normalized * moveSpeed * Time.fixedDeltaTime;
    }
    void SwitchCharacter()
    {
        switch(currentChar)
        {
            case 1:
                sr.sprite = sprites[0];
                moveSpeed = 300;
                mainAttackDamage = 5;
                break;
            case 2:
                sr.sprite = sprites[1];
                moveSpeed = 600;
                mainAttackDamage = 2;
                break;
        }
    }
    void MainAttack()
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
        Vector3 rayStart = gameObject.transform.position + new Vector3(0.6f * facedDirectionOffset, 0, 0);
        RaycastHit2D hit = Physics2D.Raycast(rayStart, transform.right * facedDirectionOffset, 2);
        Debug.DrawLine(rayStart, rayStart + new Vector3(2 * facedDirectionOffset, 0, 0), Color.red, 0.5f);
        if (hit.collider != null)
        {
            Debug.Log("Hit " + hit.collider.gameObject.name);
            if (hit.collider.gameObject.GetComponent<HealthSystemScript>() != null)
            {
                hit.collider.gameObject.GetComponent<HealthSystemScript>().ChangeHealth(-mainAttackDamage);
            }
        }
    }
}

