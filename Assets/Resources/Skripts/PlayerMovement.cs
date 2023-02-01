using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public enum AbilityType { DoubleJump, Dash, Charge }

    public Vector3 startPosition = new Vector3(-6, -1, 0);
    public AbilityType abilityType; //Speichert den Typ der Ability dieder Player aktuell hat
    int jumpsMax = 1; //Bestimmt die Anzhal der Jumps, wird nur beim double Jump auf 2 erhoeht
    int jumps; //Counted die Anzahl der bereits benutzten Jumps
    bool isGrounded = true;
    public int dashstrength;
    public float speed;
    public float inputX;
    public float jumpHeight;
    private Rigidbody2D _rigidBody2D;
    Vector3 movement;
    //Cooldown fuer den Dash
    float cooldown;
    //Speichert den die geladene Hoehe des ChargeJumps
    public bool charging = false;
    public float charge;
    public float chargeMax;

    private void Start()
    {
        _rigidBody2D = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        UpdateAbilityType();
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter2D(Collision2D collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Ground":
                jumps = 0;
                isGrounded = true;
                break;
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

    /// <summary>
    /// Bewegt den Player je nach Input nach links oder rechts 
    /// </summary>
    void MovePlayer()
    {
        inputX = Input.GetAxis("Horizontal");
        movement = new Vector3(speed * inputX, 0);
        movement *= Time.deltaTime;
        transform.Translate(movement);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumps < jumpsMax)
        {
            _rigidBody2D.velocity = new Vector3(movement.x, 0);
            isGrounded = false;
            _rigidBody2D.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
            jumps += 1;
        }
    }

    void DoDash()
    {
        if (Input.GetButton("Fire3") && cooldown < 0)
        {
            if (inputX < 0)
            {
                _rigidBody2D.AddForce(Vector2.left * dashstrength, ForceMode2D.Impulse);
                cooldown = 5;
            }
            if (inputX > 0)
            {
                _rigidBody2D.AddForce(Vector2.right * dashstrength, ForceMode2D.Impulse);
                cooldown = 5;
            }
        }
        cooldown -= Time.deltaTime;
     }
    void ChargeJump()
    {
        if (isGrounded)
        {
            //Wenn Jump laedt der Sprung auf
            if (Input.GetButton("Jump") && charge < chargeMax)
            {
                charge += Time.deltaTime * 10;
                charging = true;
            }
            //Wenn losgelassen wird, wird der charge in sprung haehe genutzt
            else if (charge > 0)
            {
                _rigidBody2D.AddForce(Vector2.up * charge, ForceMode2D.Impulse);
                isGrounded = false;
                charge = 0;
            }
        }
    }
}
