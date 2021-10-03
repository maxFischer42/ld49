using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPlayer : MonoBehaviour
{
    private Pathing parentPathing;
    public float boostEffect;
    private BoxCollider2D b;

    private void Start()
    {
        b = GetComponent<BoxCollider2D>();
    }

    public void SetParentPathing(Pathing p)
    {
        parentPathing = p;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!parentPathing.isAlive) return;
        if(collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<Controller2D>().freeFall) return;
            parentPathing.Kill();
            print("boost!");
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(collision.gameObject.GetComponent<Rigidbody2D>().velocity.x, 0f);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * boostEffect); // Reset the player's jump count
            collision.gameObject.GetComponent<Controller2D>().ResetJumps();
        }
    }

    // Called from Pathing.cs when behavior change is triggered
    public void ChangeBehavior(enemyBehaviors e)
    {
        switch(e)
        {
            case enemyBehaviors.idle:
                b.enabled = false;
                break;
            case enemyBehaviors.vulnerable:
                b.enabled = true;
                break;
            case enemyBehaviors.hurt:
                b.enabled = false;
                break;
        }
    }

}
