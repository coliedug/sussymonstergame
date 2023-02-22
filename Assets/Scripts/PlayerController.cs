using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Awake()
    {
        SetPlayerReference();
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SwitchCharacter();
        
    }
    void Update()
    {
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
        if(Input.GetButtonDown("Jump") && currentChar != 2)
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
            if (currentChar < 3)
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
                break;
            case 2:
                sr.sprite = sprites[1];
                moveSpeed = 150;
                break;
            case 3:
                sr.sprite = sprites[2];
                moveSpeed = 600;
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

