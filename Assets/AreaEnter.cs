using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaEnter : MonoBehaviour
{
    public string currentArea;

    public Text t;

    public float linger = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "AreaEnter") EnterArea(collision.gameObject.name, collision.gameObject.GetComponent<SongHolder>().mySong);
    }

    public void EnterArea(string s, AudioClip a)
    {
        if (s != currentArea)
        {
            t.enabled = true;
            GameObject.Find("GameManager").GetComponent<Jukebox>().PlaySong(a);
            StartCoroutine(DoFade(s));
        }
    }

    IEnumerator DoFade(string s)
    {
        t.text = s;
        StartCoroutine(d());
        StartCoroutine(FadeImage(false));
        yield return new WaitForSeconds(linger);
        StartCoroutine(FadeImage(true));
        t.color = new Color(1, 1, 1, 0);
        currentArea = s;
    }

    IEnumerator d()
    {
        yield return new WaitForSeconds(5f);
        t.enabled = false;
    }

    IEnumerator FadeImage(bool fadeAway)
    {
        if (fadeAway)
        {
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                t.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
        else
        {
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                t.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
    }

}
