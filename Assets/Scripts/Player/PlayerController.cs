using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public static GameObject player { get; private set; }
    Rigidbody2D rb;
    Vector2 movement;
    public int currentChar = 2;
    int mainAttackDamage = 1;
    bool facingLeft = false;
    bool dashAvailable = true;
    [SerializeField] int dashLength;
    [SerializeField] int dashDamage;
    [SerializeField] ParticleSystem[] ps;
    [SerializeField] float slamRadius;
    [SerializeField] int slamDamage;
    [SerializeField] LayerMask enemyMask;
    float facedDirectionOffset;
    float dashCooldown;
    [SerializeField] bool debugMode;
    AnimationHandler animator;
    bool slamming;
    public enum States
    {
        Ground,
        Air,
        Side
    }
    public States status;

    void Awake()
    {
        SetPlayerReference();
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SwitchCharacter();
        animator = GetComponent<AnimationHandler>();
    }
    void Update()
    {
        if (debugMode)
        {
            GetComponentInChildren<TextMeshPro>().SetText(status.ToString());
        }
        else
        {
            GetComponentInChildren<TextMeshPro>().SetText("");
        }
        CheckInputs();
        if (!dashAvailable)
        {
            dashCooldown += Time.deltaTime;
        }
        if (dashCooldown >= 3)
        {
            dashAvailable = true;
        }
    }
    private void FixedUpdate()
    {
        ProcessMovement();
        if (rb.velocity.x < 0)
        {
            facingLeft = true;
            facedDirectionOffset = -1;
        }
        else if (rb.velocity.x > 0)
        {
            facingLeft = false;
            facedDirectionOffset = 1;
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
        if(Input.GetButtonDown("Fire1"))
        {
            MainAttack();
        }
        if (Input.GetButton("Fire2"))
        {
            SecondaryAttack();
        }
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
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            gameObject.GetComponent<AudioSource>().volume += 0.1f;
        }
        else if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            gameObject.GetComponent<AudioSource>().volume -= 0.1f;
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
                mainAttackDamage = 5;
                GetComponent<AnimationHandler>().ReceiveCharacterChange(AnimationHandler.Characters.tank);
                break;
            case 2:
                mainAttackDamage = 2;
                GetComponent<AnimationHandler>().ReceiveCharacterChange(AnimationHandler.Characters.rogue);
                break;
        }
    }
    void MainAttack()
    {
        animator.TryAttack();
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position + new Vector3(facedDirectionOffset, 0, 0), 2);
        foreach (Collider2D i in hit)
        {
            if (i.gameObject.layer == 7)
            {
                Debug.Log("Hit " + i.gameObject.name);
                if (i.gameObject.GetComponent<HealthSystemScript>() != null)
                {
                    i.gameObject.GetComponent<HealthSystemScript>().ChangeHealth(-mainAttackDamage, false);
                }
            }
        }
    }
    void SecondaryAttack()
    {//This just checks which character you're on and calls the slam or dash when you right click, depending on said character.
        if (currentChar == 1)
        {
            Slam();
        }
        else
        {
            Dash();
        }
    }
    void Slam()
    {
        if (slamming) return;
        slamming = true;
        animator.TrySlam();
        Invoke("ProcessSlam", 1.1f);
    }
    void ProcessSlam()//This is the tank's slam attack, it uses an overlap circle to grab all the hit colliders with the right layermask, cast centred on a gameobject attached
                      //to the player gameobject which has the particle effects.
    {
        Vector3 slamlocation = new Vector2(gameObject.transform.position.x + (facedDirectionOffset * 2), gameObject.transform.position.y - 1);
        Collider2D[] hit = Physics2D.OverlapCircleAll(slamlocation, slamRadius, enemyMask);
        foreach (Collider2D i in hit)
        {
            i.gameObject.GetComponent<HealthSystemScript>().ChangeHealth(-slamDamage, true);
            if (i.GetComponent<Rigidbody2D>() != null)
            {
                Vector2 forceDirection = i.transform.position - gameObject.transform.position;
                i.gameObject.GetComponent<Rigidbody2D>().AddForce(forceDirection * 100);
            }
        }
        animator.currentState = AnimationHandler.states.idle;
        Debug.Log("Slam");
        slamming = false;
        animator.soundEffects.clip = animator.slamSound;
        animator.soundEffects.Play();
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
}

