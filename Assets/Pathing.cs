using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using fx;

public class Pathing : MonoBehaviour
{
    public enemyBehaviors currentBehavior = enemyBehaviors.idle;
    public float defaultSpeed;
    public List<TravelNode> nodes = new List<TravelNode>();
    private TravelNode currentNode;
    int cN = 0;
    int cL;
    private float currentSpeed;
    private Rigidbody2D rb;
    public float offsetToNode = 0.1f;

    private Animator anim;

    [Range(0, 1)]
    public float smoothDampTime = 0.1f;

    public float stompForce = 50f;

    private SpriteRenderer spr;

    public bool isAlive = true;

    public GameObject respawnEffect;

    private void Start()
    {
        currentSpeed = defaultSpeed;
        cL = nodes.Count;
        rb = GetComponent<Rigidbody2D>();
        currentNode = nodes[cN];
        spr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        transform.Find("JumpHitbox").GetComponent<BoostPlayer>().SetParentPathing(this);
    }

    public void Update()
    {
        if (!isAlive) return;
        Move();
    }

    public void Move()
    {
        if (CheckNode(cN))
        {
            cN++;
            if (cN == cL) cN = 0; // Reset to the first node 
            currentNode = nodes[cN];
            currentSpeed = currentNode.speedChange;
            UpdateBehavior();

        } else {
            Vector2 direction = (Vector2)currentNode.position.position - (Vector2)transform.position;
            direction.Normalize();
            direction *= currentSpeed;
            rb.velocity = direction;
            transform.position = Vector2.SmoothDamp(transform.position, nodes[cN].position.position, ref direction, smoothDampTime);
            if (direction.x > 0) spr.flipX = false;
            else spr.flipX = true;
        }
    }

    public bool CheckNode(int node)
    {
        Vector3 diff = transform.position - nodes[node].position.position;
        if (diff.magnitude <= offsetToNode) return true;
        return false;
    }

    void UpdateBehavior()
    {
        enemyBehaviors b = currentBehavior;
        switch(currentNode.behavior)
        {            
            case enemyBehaviors.none:
                break;
            case enemyBehaviors.idle:
                if (currentBehavior == enemyBehaviors.idle) break;
                b = currentNode.behavior;
                tag = "Enemy";
                anim.SetBool("idle", true);
                anim.SetBool("vuln", false);
                break;
            case enemyBehaviors.vulnerable:
                if (currentBehavior == enemyBehaviors.vulnerable) break;
                b = currentNode.behavior;
                tag = "Ice";
                anim.SetBool("vuln", true);
                anim.SetBool("idle", false);
                break;
        }
        gameObject.transform.Find("JumpHitbox").SendMessage("ChangeBehavior", currentBehavior);
        currentBehavior = b;
    }

    public void Kill()
    {
        isAlive = false;
        anim.SetBool("vuln", false);
        anim.SetBool("idle", false);
        anim.SetBool("fall", true);
        rb.gravityScale = 1;
        GetComponent<CapsuleCollider2D>().isTrigger = true;
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.down * stompForce);
        Fadeout();
        StartCoroutine(ResetEnemy());
    }

    private bool fadedOut = false;
    public float fadeSpeed = 0.5f;
    private float minimum = 0.0f;
    public void Fadeout()
    {
        float step = fadeSpeed * Time.deltaTime;
        if(!fadedOut)
        {
            spr.color = new Color(1f, 1f, 1f, Mathf.Lerp(spr.color.a, minimum, step));
            if (Mathf.Abs(spr.color.a - minimum) <= float.Epsilon) fadedOut = true;
        }
    }

    public float timeToRespawn = 3f;
    public float effectTime = 0.5f;

    IEnumerator ResetEnemy()
    {
        yield return new WaitForSeconds(timeToRespawn);
        GameObject a = (GameObject)Instantiate(respawnEffect, transform.parent);
        Destroy(a, effectTime);
        yield return new WaitForSeconds(effectTime);
        isAlive = true;
        currentBehavior = enemyBehaviors.idle;
        spr.color = new Color(1f, 1f, 1f, 1f);
        rb.gravityScale = 0f;
        GetComponent<CapsuleCollider2D>().isTrigger = false;
        transform.position = transform.parent.position;
        anim.SetBool("idle", true);
        anim.SetBool("fall", false);
    }


}

[Serializable]
public class TravelNode {
    public Transform position;
    public enemyBehaviors behavior;
    public float speedChange;

}

public enum enemyBehaviors { idle, vulnerable, hurt, none};


