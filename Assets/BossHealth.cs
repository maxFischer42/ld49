using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public Vector2 weaknessRange = new Vector2(4f, 10f);
    float currentTarget;
    float t;

    public int hp = 10;

    bool weak = true;
    bool taking = false;

    Animator anim;
    SpriteRenderer s;

    public float boostEffect = 100f;

    public Sprite fall;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.enabled = true;
        s = GetComponent<SpriteRenderer>();
        SwapStates();
    }

    void SwapStates()
    {
        weak = !weak;
        currentTarget = Random.Range(weaknessRange.x, weaknessRange.y);
        t = 0f;
        anim.SetBool("weak", weak);
        switch (weak)
        {
            case true:
                tag = "Enemy";
                break;
            case false:
                tag = "Ice";
                break;
        }
    }

    private void Update()
    {
        t += Time.deltaTime;
        if (t >= currentTarget) SwapStates();
        if (hp <= 0)
        {
            anim.enabled = false;
            GetComponent<Boss>().enabled = false;
            s.sprite = fall;
            GetComponent<CapsuleCollider2D>().enabled = false;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            GetComponent<Rigidbody2D>().AddForce(Vector2.down * 50f);
            Destroy(gameObject, 10f);
            GameObject.Find("Player").GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GameObject.Find("GameManager").GetComponent<GameManager>().BossBattleOutro();
            enabled = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<Controller2D>().freeFall) return;

            if (weak && !taking)
            {
                taking = true;
                print("boost!");
                collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(collision.gameObject.GetComponent<Rigidbody2D>().velocity.x, 0f);
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * boostEffect); // Reset the player's jump count
                collision.gameObject.GetComponent<Controller2D>().jumpCount++;
                StartCoroutine("DoDamage");
            }
        }
    }

    IEnumerator DoDamage()
    {
        Color c = s.color;
        s.color = Color.red;
        yield return new WaitForSeconds(1f);
        s.color = c;
        hp--;
        taking = false;
        GetComponent<Boss>().actionTimerRange = new Vector2(GetComponent<Boss>().actionTimerRange.x - 0.05f, GetComponent<Boss>().actionTimerRange.y - 0.25f);
        SwapStates();
    }
}
