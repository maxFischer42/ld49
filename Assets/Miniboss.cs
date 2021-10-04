using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miniboss : MonoBehaviour
{
    public float moveSpeed = 5f;

    public enum mbState { idle, sin, attack_1, attack_2, move }
    public mbState state;

    // Idle
    public Transform idleLocation;

    // Move
    public Transform[] moveLocations;

    [Range(0, 1)]
    public float smoothDampTime = 0.1f;
    public float locationOffset = 0.1f;

    public float genericMoveSpeed = 5f;

    private float x;
    private float y;

    public float hoverDis = 0.5f;
    public float hoverSpeed = 0.5f;
    float hoverTimer = 0f;
    float hoverDir = -1;
    float hoverMax = 1f;

    public float sinSpeed = 1f;
    float sinTimer = 0f;
    public float sinMax = 3f;
    float sinDir = -1;

    float actionTimer = 0f;
    float actionMax = 1f;
    Vector2 actionTimerRange = new Vector2(1f, 2.5f);

    public GameObject attackOnePrefab;
    bool hasAttackedOnce = false;

    public GameObject attackTwoPrefab;
    bool hasAttackedTwice = false;

    // Don't let the boss escape these bounds in the x axis
    Vector2 bounds = new Vector2(-11, 11);

    int target = -1;

    private Rigidbody2D rb;
    private SpriteRenderer sp;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        GetComponent<MinibossHealth>().enabled = true;
    }

    void Update()
    {
        actionTimer += Time.deltaTime;
        if(actionTimer >= actionMax)
        {
            ResetState();
        }
        HandleState();
    }

    private void FixedUpdate()
    {
        x = transform.position.x;
        y = transform.position.y;
        if (x < bounds.x || x > bounds.y) ResetState();
    }

    void HandleState()
    {
        switch(state)
        {
            case mbState.idle:
                hoverTimer += Time.deltaTime;
                if (hoverTimer >= hoverMax)
                {
                    hoverTimer = 0f;
                    hoverDir *= -1;
                }
                Idle();
                break;
            case mbState.sin:
                sinTimer += Time.deltaTime;
                if (sinTimer >= sinMax)
                {
                    sinTimer = 0f;
                    sinDir *= -1;
                }
                Sin();
                break;
            case mbState.attack_1:
                AttackOne();
                break;
            case mbState.attack_2:
                AttackTwo();
                break;
            case mbState.move:
                Move();
                break;
        }
    }

    void ResetState()
    {
        actionMax = Random.Range(actionTimerRange.x, actionTimerRange.y);
        actionTimer = 0f;
        hoverTimer = 0f;
        sinTimer = 0f;
        target = -1;
        hasAttackedOnce = false;
        hasAttackedTwice = false;
        state = (mbState)Random.Range(0, 5);
    }

    void Idle()
    {
        // Hover at idle location
        // If not at idle location, go to it
        if(AtPoint(idleLocation.position))
        {
            Hover();
            // just hover for undisclosed amount of time
        } else
        {
            MoveToLocation(idleLocation.position);
            // move to idle location
        }

    }

    void Sin()
    {
        MoveSin();
    }

    void AttackOne()
    {
        if (hasAttackedOnce) return;
        GameObject a = (GameObject)Instantiate(attackOnePrefab, transform);
        hasAttackedOnce = true;
        a.transform.parent = null;
        Destroy(a, 10f);    
    }

    void AttackTwo()
    {
        if (hasAttackedTwice) return;
        GameObject a = (GameObject)Instantiate(attackTwoPrefab, transform);
        hasAttackedTwice = true;
        a.transform.parent = null;
    }

    void Move()
    {
        if (target == -1)
        {
            int r = Random.Range(0, moveLocations.Length);
            target = r;
        }
        Vector3 pos = moveLocations[target].position;
        MoveToLocation(pos);

    }


    ////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////


    bool AtPoint(Vector3 p)
    {
        Vector3 dis = p - transform.position;
        var a = dis.magnitude;
        if (Mathf.Abs(a) <= locationOffset) return true;
        return false;
    }

    void Hover()
    {
        Vector2 v = rb.velocity;
        transform.position = Vector2.SmoothDamp(transform.position, transform.position + new Vector3(0, hoverDir * hoverDis, 0), ref v, smoothDampTime, hoverSpeed);
    }

    void MoveToLocation(Vector3 loc)
    {
        Vector2 direction = (Vector2)loc - (Vector2)transform.position;
        direction.Normalize();
        direction *= genericMoveSpeed;
        rb.velocity = direction;
        transform.position = Vector2.SmoothDamp(transform.position, loc, ref direction, smoothDampTime);
        if (direction.x > 0) sp.flipX = false;
        else sp.flipX = true;
    }

    void MoveSin()
    {
        Vector3 newPosition = transform.position;
        newPosition.x += Mathf.Sin(Time.time / sinSpeed) * Time.deltaTime * sinDir * sinSpeed;
        newPosition.y += (Mathf.Sin(Time.time * sinSpeed) * Time.deltaTime ) / 4;
        transform.position = newPosition;
    }

}
