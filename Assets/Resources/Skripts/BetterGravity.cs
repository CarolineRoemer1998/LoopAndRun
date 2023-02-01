using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterGravity : MonoBehaviour
{
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    Rigidbody2D rb;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //Wenn der Spieler fällt, wird die Gravitation erhöht damit sich das Spiel schneller anfühlt
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        //Wenn der Spieler bei einem Sprung "Jump" los lässt, befor die maximal höhe errechit ist, wird die Jump höhe reduziert (Gilt nicht beim ChargeJump)
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump") && gameObject.GetComponent<PlayerMovement>().abilityType != PlayerMovement.AbilityType.Charge)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
