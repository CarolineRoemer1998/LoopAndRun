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
        //Wenn der Spieler f�llt, wird die Gravitation erh�ht damit sich das Spiel schneller anf�hlt
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        //Wenn der Spieler bei einem Sprung "Jump" los l�sst, befor die maximal h�he errechit ist, wird die Jump h�he reduziert (Gilt nicht beim ChargeJump)
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump") && gameObject.GetComponent<PlayerMovement>().abilityType != PlayerMovement.AbilityType.Charge)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
