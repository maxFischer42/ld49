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
    public float iceSpeed = 3f;

    public float jumpCooldown = 1.2f;
    private bool canJump = true;

    public bool isGrounded = false;
    public LayerMask collideLayerMask = 0;

    public float groundCheckDistance = 0.4f;

    private Vector2 velocity;

    private SpriteRenderer spr;
    private Rigidbody2D rb;
    private Animator anim;

    // default facing right
    private float faceDirection = 1;
    private float archivedFaceDirection = 1;

    public int minJumps = 2;
    public int jumpCount = 4;
    private int minJumpCount;
    private int maxJumpCount;

    private BoxCollider2D box;
    private CircleCollider2D circle;

    public Vector3 raycastOffset;

    public GameObject takeoffParticleSystem;
    public GameObject landParticleSystem;
    public GameObject jumpParticleSystem;
    public Transform particleSpawn;
    public GameObject breakWingParticleSystem;

    public float velocityToFall = 30;
    private bool fall = false;
    private bool bWings = false;

    public Animator wingAnimator;

    public bool freeFall = false;

    public PhysicsMaterial2D defaultPhysMat;
    public PhysicsMaterial2D slipperyPhysMat;

    public float yCap = 60f;
    public float xCap = 35f;

    public AudioClip wingFlapSound;
    public AudioClip airBoostSound;
    public AudioClip breakWingSound;
    public AudioClip freeFallSound;

    public bool isInCutscene = false;

    public void ResetJumps()
    {
        GenerateJumps();
    }

    private void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();
        circle = GetComponent<CircleCollider2D>();
        maxJumpCount = jumpCount;
        minJumpCount = minJumps;
    }

    public void Update()
    {
        if (isInCutscene) return;
        if (Time.timeScale == 0) return;
        if (rb.velocity.y > yCap) rb.velocity = new Vector2(rb.velocity.x, yCap);
        if (Mathf.Abs(rb.velocity.x) > xCap) rb.velocity = new Vector2(xCap * Mathf.Sign(rb.velocity.x), rb.velocity.y);

        // TODO replace with actual input
        float x = Input.GetAxis("Horizontal");
        bool y = Input.GetButtonDown("Jump");
        anim.SetFloat("x", Mathf.Abs(Input.GetAxisRaw("Horizontal")));
        if (y && canJump && jumpCount > 0 && !fall) Jump(x);
        else if(x != 0 && !fall) Move(x);
    }

    public void LateUpdate()
    {
        if (isInCutscene) return;
        if (Time.timeScale == 0) return;
        if (freeFall)
        {
            if((Mathf.Abs(velocity.x) < 0.1) &&(Mathf.Abs(velocity.y) < 0.01)) { //On ice, basically not moving
                IceSoftFix();
            }  else
            {
                rb.AddForce(velocity);
            }
        }


        velocity = rb.velocity;
        anim.SetBool("grounded", isGrounded);
        if(jumpCount <= 0 && !bWings && !freeFall) {
            wingAnimator.SetBool("broken", true);
            SpawnParticles(breakWingParticleSystem, true);
            bWings = true;
            GameObject.Find("SFXSource").GetComponent<AudioSource>().PlayOneShot(breakWingSound);
        }
        if(velocity.y < -velocityToFall && !freeFall) {
            anim.SetBool("fall", true);
            wingAnimator.SetBool("fall", true);
            freeFall = true; rb.sharedMaterial = slipperyPhysMat;
            GameObject.Find("SFXSource").GetComponent<AudioSource>().PlayOneShot(freeFallSound);
        }
        CollisionCheck();
    }

    // Send out rays below the object and check if they contact any colliders
    void CollisionCheck()
    {
        bool temp = isGrounded;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, collideLayerMask);
        RaycastHit2D hitL = Physics2D.Raycast(transform.position + raycastOffset, Vector2.down, groundCheckDistance, collideLayerMask);
        RaycastHit2D hitR = Physics2D.Raycast(transform.position - raycastOffset, Vector2.down, groundCheckDistance, collideLayerMask);
        if (hit || hitL || hitR)
        {
            if((hit && hit.collider.tag == "Ice") || (hitL && hitL.collider.tag == "Ice") || (hitR && hitR.collider.tag == "Ice"))
            {
                StartIce();
            } else
            {
                isGrounded = true;
                box.enabled = true;
                circle.enabled = false;
                rb.sharedMaterial = defaultPhysMat;
            }
        }
        else isGrounded = false;
        if(isGrounded)
        {

            GenerateJumps();

            anim.SetBool("fall", false);
            wingAnimator.SetBool("broken", false);
            wingAnimator.SetBool("fall", false);
            fall = false;
            bWings = false;
            freeFall = false;
        }
        if(isGrounded && !temp)
        {
            SpawnParticles(landParticleSystem);
        }
    }

    void GenerateJumps()
    {
        int j = Random.Range(minJumpCount, maxJumpCount);
        jumpCount = j;
    }

    public void Move(float inputDir) // Move in the X direction
    {
        if (isInCutscene) return;
        if (Time.timeScale == 0) return;
        if (freeFall) return;
        float mult = (isGrounded ? groundSpeed : airSpeed);
        float x = inputDir * mult;     
        rb.velocity = new Vector2(x, rb.velocity.y);
        DoFlip(Input.GetAxisRaw("Horizontal"));
    }

    public void Jump(float inputDir)
    {
        if (isInCutscene) return;
        if (Time.timeScale == 0) return;
        if (freeFall) return;
        anim.SetTrigger("jump");
        float xMult = inputDir, yMult = 0, x = 0, y = 0;
        yMult = (isGrounded ? groundJumpForce : airJumpForce);
        AudioClip a = wingFlapSound;
        if (inputDir != 0)
        {
            xMult = (isGrounded ? groundSpeed : airSpeed);
        }
        else // No input, jump higher
        {
            if(!isGrounded)
            {
                yMult = fullAirJump;
                a = airBoostSound;
            } else if (isGrounded)
            {
                yMult = airJumpForce;                    
            }
        }

        x = xMult;
        y = yMult;

        rb.velocity = new Vector2(xMult, 0f);
        rb.AddForce(new Vector2(x, y));
        wingAnimator.SetTrigger("flap");
        jumpCount--;
        SpawnParticles(jumpParticleSystem, true);
        if (isGrounded) SpawnParticles(takeoffParticleSystem);
        StartCoroutine(PerformCooldown(jumpCooldown, canJump));
        GameObject.Find("SFXSource").GetComponent<AudioSource>().PlayOneShot(a);
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

    public void IceSoftFix()
    {
        float x = transform.position.x;
        x *= iceSpeed;
        rb.AddForce(new Vector2(-x, 0f));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ice")
        {
            StartIce();
            if (!freeFall) GameObject.Find("SFXSource").GetComponent<AudioSource>().PlayOneShot(freeFallSound);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ice") StartIce();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ice")
        {
            StartIce();
            if (!freeFall) GameObject.Find("SFXSource").GetComponent<AudioSource>().PlayOneShot(freeFallSound);
        }
    }

    public void StartIce()
    {
        isGrounded = false;
        box.enabled = false;
        circle.enabled = true;
        jumpCount = 0;
        bWings = false;
        anim.SetBool("fall", true);
        wingAnimator.SetBool("fall", true);
        freeFall = true;
        rb.sharedMaterial = slipperyPhysMat;
    }

}

