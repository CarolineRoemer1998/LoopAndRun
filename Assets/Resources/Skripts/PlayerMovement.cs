using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public enum AbilityType { DoubleJump, Dash, Charge }

    public AbilityType abilityType; //Speichert den Typ der Ability dieder Player aktuell hat.
    int jumpsmax = 1; //Bestimmt die Anzhal der Jumps, wird nur beim double Jump auf 2 erhöht.
    int jumps;
    bool isGrounded = true;
    public int dashstrength;
    public float speed;
    public float inputX;
    public float jumpHeight;
    private Rigidbody2D _rigidBody2D;
    //Cooldown für den Dash
    float cooldown;
    //Speichert den die geladene Höhe des ChargeJumps
    public bool charging = false;
    public float charge;

    private void Start()
    {
        _rigidBody2D = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAbilityType();
        MovePlayer();
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            jumps = jumpsmax;
        }
    }

    void UpdateAbilityType()
    {
        //Switch zwischen den Abilities anhand  des Ability types
        switch (abilityType)
        {
            //Double Jump
            case AbilityType.DoubleJump:
                jumpsmax = 2;
                Jump();
                break;
            //Dash
            case AbilityType.Dash:
                jumpsmax = 1;
                DoDash();
                Jump();
                break;
            //Charge Jump
            case AbilityType.Charge:
                jumpsmax = 1;
                ChargeJump();
                break;
            default:
                break;
        }
    }

    void MovePlayer()
    {
        inputX = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(speed * inputX, 0);
        movement *= Time.deltaTime;
        transform.Translate(movement);
    }

    void Jump()
    {
        if (Input.GetButton("Jump"))
        {
            if (jumps > 0)
            {
                isGrounded = false;
                _rigidBody2D.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
                jumps -= 1;
            }
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
            //Wenn JUmp lädt der Sprung auf.
            if (Input.GetButton("Jump"))
            {
                charge += Time.deltaTime * 10;
                charging = true;
            }
            //Wenn losgelassen wird, wird der charge in sprung höhe genutzt
            else if (charge > 0)
            {
                _rigidBody2D.AddForce(Vector2.up * charge, ForceMode2D.Impulse);
                isGrounded = false;
                charge = 0;
            }
        }
    }
}
