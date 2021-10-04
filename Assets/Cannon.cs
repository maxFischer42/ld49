using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    private GameManager h;
    private Transform p;
    public float offset = 10f;
    public float range = 10f;

    public GameObject m;

    float t;
    float delay = 8f;

    private void Start()
    {
        h = GameObject.Find("GameManager").GetComponent<GameManager>();
        p = h.c.transform;
    }

    private void Update()
    {
        if (Mathf.Abs(p.position.y - (transform.position.y + offset)) <= range)
        {
            t += Time.deltaTime;
            if(t >= delay)
            {
                t = 0f;
                GameObject missile = (GameObject)Instantiate(m, transform);
                missile.transform.parent = null;
            }
        } else
        {
            t = 0f;
        }
    }


}
