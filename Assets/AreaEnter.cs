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
        if (collision.gameObject.tag == "AreaEnter") EnterArea(collision.gameObject.name);
    }

    public void EnterArea(string s)
    {
        if(s != currentArea) StartCoroutine(DoFade(s));
    }

    IEnumerator DoFade(string s)
    {
        t.text = s;
        StartCoroutine(FadeImage(false));
        yield return new WaitForSeconds(linger);
        StartCoroutine(FadeImage(true));
        t.color = new Color(1, 1, 1, 0);
        currentArea = s;
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
