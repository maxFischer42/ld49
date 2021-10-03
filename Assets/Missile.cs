using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float timeToApex = 2f;
    public float launchSpeed = 5f;
    public float aimForce = 3f;
    float t = 0f;
    public Transform target;

    bool stop = false;

    private Rigidbody2D rb;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector3.up * launchSpeed;
    }

    private void Update()
    {
        t += Time.deltaTime;
        if (stop) return;
        if (t > timeToApex)
        {
            Vector2 dir = target.position - transform.position;
            dir.Normalize();

            // rotate towards the player
            float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
            rb.velocity = Vector3.zero;
            dir *= aimForce;
            rb.velocity = dir;
            stop = true;
            Destroy(gameObject, 6f);
        } else
        {
            rb.velocity = Vector3.up * launchSpeed;
        }
    }


}
