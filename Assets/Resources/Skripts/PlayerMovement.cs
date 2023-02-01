using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public enum AbilityType { DoubleJump, Dash, Charge }

    public AbilityType abilityType; //Speichert den Typ der Ability dieder Player aktuell hat.
    int jumpsmax = 1; //Bestimmt die Anzhal der Jumps, wird nur beim double Jump auf 2 erh�ht.
    int jumps;
    public float speed;
    float inputX;
    public float jumpHeight;
    private Rigidbody2D _rigidBody2D;

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
                break;
            //Dash
            case AbilityType.Dash:
                jumpsmax = 1;
                break;
            //Charge Jump
            case AbilityType.Charge:
                jumpsmax = 1;
                break;
            default:
                break;
        }
    }

    void MovePlayer()
    {
        inputX = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(speed * inputX, 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumps > 0)
            {
                _rigidBody2D.AddForce(Vector2.up * jumpHeight);
                jumps -= 1;
            }
        }
        movement *= Time.deltaTime;
        transform.Translate(movement);
    }
}
