using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public GameObject deathEffect;
    public AudioClip sfx;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameObject e = (GameObject)Instantiate(deathEffect, transform);
            e.transform.parent = null;
            GameObject.Find("SFXSource").GetComponent<AudioSource>().PlayOneShot(sfx);
            Destroy(e, 0.2f);
            Destroy(gameObject);

            //coins ++ or whatever
        }
    }
}
