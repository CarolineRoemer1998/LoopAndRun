using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public enum AbilityType { DoubleJump, Dash, Charge }

    public Vector3 startPosition = new Vector3(-6, -1, 0);
    public AbilityType abilityType; //Speichert den Typ der Ability dieder Player aktuell hat.
    int jumpsMax = 1; //Bestimmt die Anzhal der Jumps, wird nur beim double Jump auf 2 erhöht.
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter2D(Collision2D collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Ground":
                jumps = jumpsMax;
                break;
            case "Spikes":
                gameObject.transform.position = startPosition;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Setzt die Fähigkeit des Spielers (DoubleJump, Dash, Charge)
    /// </summary>
    void UpdateAbilityType()
    {
        //Switch zwischen den Abilities anhand  des Ability types
        switch (abilityType)
        {
            //Double Jump
            case AbilityType.DoubleJump:
                jumpsMax = 2;
                break;
            //Dash
            case AbilityType.Dash:
                jumpsMax = 1;
                break;
            //Charge Jump
            case AbilityType.Charge:
                jumpsMax = 1;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Bewegt den Player je nach Input nach links oder rechts oder führt bei GetKeyDown.Space Sprung aus
    /// </summary>
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
