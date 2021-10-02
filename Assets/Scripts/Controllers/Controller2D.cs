using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller2D : MonoBehaviour
{
    public float groundSpeed = 2f;
    public float airSpeed = 3f;
    public float groundJumpForce = 5f;
    public float airJumpForce = 3f;
    public float fullAirJump = 5f;

    public float jumpCooldown = 1.2f;
    private bool canJump = true;

    public bool isGrounded = false;
    public LayerMask collideLayerMask = 0;

    public float groundCheckDistance = 0.4f;

    private Vector2 velocity;

    private SpriteRenderer spr;
    private Rigidbody2D rb;

    // default facing right
    private float faceDirection = 1;
    private float archivedFaceDirection = 1;

    public int jumpCount = 4;
    private int maxJumpCount;

    public Vector3 raycastOffset;

    public GameObject takeoffParticleSystem;
    public GameObject landParticleSystem;
    public GameObject jumpParticleSystem;
    public Transform particleSpawn;

    private void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        maxJumpCount = jumpCount;
    }

    public void Update()
    {
        float x = Input.GetAxis("Horizontal");
        bool y = Input.GetButtonDown("Jump");
        if (y && canJump && jumpCount > 0) Jump(x);
        else if(x != 0) Move(x);
    }

    public void LateUpdate()
    {
        velocity = rb.velocity;
        CollisionCheck();
    }

    // Send out rays below the object and check if they contact any colliders
    void CollisionCheck()
    {
        bool temp = isGrounded;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, collideLayerMask);
        RaycastHit2D hitL = Physics2D.Raycast(transform.position + raycastOffset, Vector2.down, groundCheckDistance, collideLayerMask);
        RaycastHit2D hitR = Physics2D.Raycast(transform.position - raycastOffset, Vector2.down, groundCheckDistance, collideLayerMask);
        if (hit || hitL || hitR) isGrounded = true;
        else isGrounded = false;
        if(isGrounded)
        {
            jumpCount = maxJumpCount;            
        }
        if(isGrounded && !temp)
        {
            SpawnParticles(landParticleSystem);
        }
    }

    public void Move(float inputDir) // Move in the X direction
    {
        float mult = (isGrounded ? groundSpeed : airSpeed);
        float x = inputDir * mult;     
        rb.velocity = new Vector2(x, rb.velocity.y);
        DoFlip(Input.GetAxisRaw("Horizontal"));
    }

    public void Jump(float inputDir)
    {
        float xMult = inputDir, yMult = 0, x = 0, y = 0;
        yMult = (isGrounded ? groundJumpForce : airJumpForce);
        if (inputDir != 0)
        {
            xMult = (isGrounded ? groundSpeed : airSpeed);
        }
        else // No input, jump higher
        {
            if(!isGrounded)
            {
                yMult = fullAirJump;
            } else if (isGrounded)
            {
                yMult = airJumpForce;                    
            }
        }

        x = xMult;
        y = yMult;

        rb.velocity = new Vector2(xMult, 0f);
        rb.AddForce(new Vector2(x, y));
        jumpCount--;
        SpawnParticles(jumpParticleSystem, true);
        if (isGrounded) SpawnParticles(takeoffParticleSystem);
        StartCoroutine(PerformCooldown(jumpCooldown, canJump));
    }

    IEnumerator PerformCooldown(float length, bool toggle)
    {
        toggle = false;
        yield return new WaitForSeconds(length);
        toggle = true;
    }

    // Flip the sprite if we swap to face another direction
    void DoFlip(float x)
    {
        if((x > faceDirection || x < faceDirection) && (x != 0))
        {
            bool flip = (x == 1 ? false : true);
            faceDirection = x;
            spr.flipX = flip;
            StartCoroutine(archiveFaceDirection());
        }
    }

    IEnumerator archiveFaceDirection()
    {
        yield return new WaitForSeconds(0.5f);
        archivedFaceDirection = faceDirection;
    }

    public void SpawnParticles(GameObject g, bool usePlayerParent = false)
    {
        Transform sp = particleSpawn;
        GameObject p = (GameObject)Instantiate(g, transform);
        if (!usePlayerParent) p.transform.parent = null;
        Destroy(p, 2f);
    }

}

