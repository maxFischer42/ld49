using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{

    bool isAlive = true;
    public float timeToRespawn = 4f;
    float effectTime = 0.5f;
    Animator anim;
    SpriteRenderer sp;
    public float boostEffect = 200f;

    private void Start()
    {
        anim = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
    }

    public void Update()
    {
        if(GameObject.Find("Player").GetComponent<Controller2D>().freeFall)
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
            GetComponent<CircleCollider2D>().isTrigger = true;
        } else
        {
            GetComponent<BoxCollider2D>().isTrigger = false;
            GetComponent<CircleCollider2D>().isTrigger = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Kill();
        if (collision.gameObject.tag == "Player")
        {
            
            if (collision.gameObject.GetComponent<Controller2D>().freeFall) return;
            print("boost!");
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(collision.gameObject.GetComponent<Rigidbody2D>().velocity.x, 0f);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * boostEffect); // Reset the player's jump count
            collision.gameObject.GetComponent<Controller2D>().jumpCount++;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Kill();
    }

    public void Kill()
    {
        anim.SetTrigger("Kill");
        isAlive = false;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        StartCoroutine(ResetEnemy());
    }

    IEnumerator ResetEnemy()
    {
        yield return new WaitForSeconds(timeToRespawn);
        anim.SetTrigger("Respawn");
        yield return new WaitForSeconds(effectTime);
        isAlive = true;
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<CircleCollider2D>().enabled = true;
    }
}
