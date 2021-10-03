using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerObject : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            SendMessageUpwards("StartEvent");
            GameObject.Find("GameManager").GetComponent<GameManager>().InCutscene(true);
        }
    }
}
