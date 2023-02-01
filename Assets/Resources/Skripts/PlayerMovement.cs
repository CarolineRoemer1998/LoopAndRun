using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public enum AbilityType { DoubleJump, Dash, Charge }

    public Vector3 startPosition = new Vector3(-6, -1, 0);
    public AbilityType abilityType; //Speichert den Typ der Ability dieder Player aktuell hat

    private Rigidbody2D rb;
    //Movement
    private Vector3 movement;
    public float speed;
    public float inputX;

    //Abilities
    public float jumpHeight;
    int jumpsMax = 1; //Bestimmt die Anzhal der Jumps, wird nur beim double Jump auf 2 erhoeht
    int jumps; //Counted die Anzahl der bereits benutzten Jumps

    private float directionlock; //Speichert beim Dash die Richtig vom start des Dashes 
    float cooldown; //Cooldown fuer den Dash / Kann in final version weg
    public int dashstrength;

    public float charge;  //Speichert den die geladene Hoehe des ChargeJumps
    public float chargeMax;


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
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter2D(Collision2D collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Spikes":
                gameObject.transform.position = startPosition;
                break;
            case "Goal":

                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Setzt die Faehigkeit des Spielers (DoubleJump, Dash, Charge)
    /// </summary>
    void UpdateAbilityType()
    {
        //Switch zwischen den Abilities anhand  des Ability types
        switch (abilityType)
        {
            //Double Jump
            case AbilityType.DoubleJump:
                jumpsMax = 2;
                Jump();
                break;
            //Dash
            case AbilityType.Dash:
                jumpsMax = 1;
                DoDash();
                Jump();
                break;
            //Charge Jump
            case AbilityType.Charge:
                jumpsMax = 1;
                ChargeJump();
                break;
            default:
                break;
        }
    }

    void UpdateisGrounded()
    {
        if (!Input.GetButton("Jump")) //Wird abgefragt ob spieler "Jump" drückt weil sonnst die Jumpanzahl direkt nach absprung resettet wird, da der Ground Collider noch zu nah am bround ist
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
        if (Input.GetButtonDown("Fire3"))
        {
            directionlock = rb.velocity.x;

        }
        if (Input.GetButton("Fire3"))
        {
            rb.velocity = new Vector3(dashstrength*directionlock, 0);
        }
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
                charge += Time.deltaTime * 15;
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
