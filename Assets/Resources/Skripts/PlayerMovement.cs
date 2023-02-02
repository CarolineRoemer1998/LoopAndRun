using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public enum AbilityType { DoubleJump, Dash, Charge }

    public Vector3 startPosition = new Vector3(-6, -1, 0);
    public AbilityType abilityType; //Speichert den Typ der Ability dieder Player aktuell hat
    public Animator animator;

    private Rigidbody2D rb;

    //Movement
    private Vector3 movement;
    public float speed;
    public float inputX;

    //Abilities
    public float jumpHeight;
    int jumpsMax = 1; //Bestimmt die Anzhal der Jumps, wird nur beim double Jump auf 2 erhoeht
    int jumps; //Counted die Anzahl der bereits benutzten Jumps

    private float directionlock = 1; //Speichert beim Dash die Richtig vom start des Dashes 
    float cooldown; //Cooldown fuer den Dash / Kann in final version weg
    public int dashstrength;

    public float charge;  //Speichert den die geladene Hoehe des ChargeJumps
    public float chargeMax;

    //Animation States
    private float stillTimer;
    private bool inDash;

    //Groundcheck 
    public bool isGrounded;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        UpdateAbilityType();
        UpdateisGrounded();
        InvertPlayer();
        Updateanimations();
    }

    /// <summary>
    /// Collision mit Spikes/Acid oder Ziel
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter2D(Collision2D collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Spikes":
            case "Acid":
                gameObject.transform.position = startPosition;
                break;
            case "Goal":
                // TODO: Change Ability
                break;
            default:
                break;
        }
    }

    void InvertPlayer()
    {
        if (rb.velocity.x > 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        } 
        else if(rb.velocity.x < 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;

        }
    }

    void Updateanimations()
    {
        //StillTime / Berechnet wie lange der Player Still stand und stilt kleine animation ab
        if (rb.velocity.x == 0 && rb.velocity.y == 0)
            stillTimer += Time.deltaTime;
        if (rb.velocity.x != 0 || rb.velocity.y != 0)
            stillTimer = 0;

        //Übergibt an die Animation die Werte mit denen bestimmt wird welche Animation abgespielt werden soll
        animator.SetBool("InDash", inDash);
        animator.SetFloat("StillTime", stillTimer);
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("Charging", charge);
    }

    /// <summary>
    /// Setzt die Faehigkeit des Spielers (DoubleJump, Dash, Charge)
    /// </summary>
    void UpdateAbilityType()
    {
        //Switch zwischen den Abilities anhand  des Ability types
        switch (abilityType)
        {
            case AbilityType.DoubleJump:
                jumpsMax = 2;
                Jump();
                break;
            case AbilityType.Dash:
                jumpsMax = 1;
                DoDash();
                Jump();
                break;
            case AbilityType.Charge:
                jumpsMax = 1;
                ChargeJump();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Updatet isGrounded.
    /// Fragt ab, ob Spieler "Jump" drückt, um direktes, erneutes Springen zu verhindern, 
    /// wenn Spieler zu nah an Boden ist und dadurch Jumpanzahl resettet wird.
    /// </summary>
    void UpdateisGrounded()
    {
        if (!Input.GetButton("Jump")) 
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
            if (isGrounded)
            {
                jumps = 0;
            }
        }
    }

    /// <summary>
    /// Bewegt den Player je nach Input nach links oder rechts 
    /// </summary>
    void MovePlayer()
    {
        inputX = Input.GetAxis("Horizontal");
        movement = new Vector3(speed * inputX, 0);
        movement *= Time.deltaTime;
        rb.velocity = new Vector3(speed * inputX, rb.velocity.y);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumps < jumpsMax)
        {
            rb.velocity = new Vector3(movement.x, 0);
            isGrounded = false;
            rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
            jumps += 1;
        }
    }

    void DoDash()
    {
        Physics2D.gravity = new Vector2(0, -9.81f);
        if (inputX > 0)
        {
            directionlock = 1;
        }
        else if (inputX < 0)
        {
            directionlock = -1;
        }
        if (Input.GetButton("Fire3"))
        {
            Physics2D.gravity = new Vector2(0, 0);
            rb.velocity = new Vector3(dashstrength * directionlock, 0);
            inDash = true;
        }
        else
            inDash = false;
     }

    void ChargeJump()
    {
        if (isGrounded)
        {
            //Wenn Jump laedt der Sprung auf
            if (Input.GetButton("Jump"))
            {
                if (charge < chargeMax)
                {
                charge += Time.deltaTime * 30;
                }
            }
            //Wenn losgelassen wird, wird der charge in sprung haehe genutzt
            else if (charge > 0)
            {
                rb.AddForce(Vector2.up * charge, ForceMode2D.Impulse);
                isGrounded = false;
                charge = 0;
            }
        }
    }
    
    //Nur zur Sicherheit gespeichert.
    void DoDashOld()
    {
        if (Input.GetButton("Fire3") && cooldown < 0)
        {
            if (inputX < 0)
            {
                rb.AddForce(Vector2.left * dashstrength, ForceMode2D.Impulse);
                cooldown = 5;
            }
            if (inputX > 0)
            {
                rb.AddForce(Vector2.right * dashstrength, ForceMode2D.Impulse);
                cooldown = 5;
            }
        }
        cooldown -= Time.deltaTime;
    }
}
