using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Typewriter : MonoBehaviour
{
    Text txt;
    string story;
    public float d = 0.05f;
    public Image carrot;
    public Image portrait;
    bool isWriting = false;
    public AudioClip a;

    private void Start()
    {
        txt = transform.GetChild(0).transform.Find("Text").GetComponent<Text>();
        //Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.");
    }

    private Queue<Dialogue> sentences = new Queue<Dialogue>();

    public void Speak(Dialogue[] d)
    {
        sentences.Clear();

        foreach (Dialogue sentence in d)
        {
            sentences.Enqueue(sentence);
        }

        Write();
        
    }

    public void Write()
    {
        Dialogue a = sentences.Dequeue();
        transform.GetChild(0).gameObject.SetActive(true);
        portrait.sprite = a.portrait;
        carrot.enabled = false;
        Time.timeScale = 0f;
        txt.text = "";
        story = a.story;
        StopAllCoroutines();
        StartCoroutine("PlayText");
    }

    private void Update()
    {
        if (txt.text == story) carrot.enabled = true;
        if(Input.GetButtonDown("Jump"))
        {
            if (txt.text != story)
            {
                StopAllCoroutines();
                txt.text = story;
            } else if (txt.text == story)
            {
                txt.text = "";
                story = "";
                Time.timeScale = 1f;
                transform.GetChild(0).gameObject.SetActive(false);
                isWriting = false;
                if (sentences.Count > 0) Write();
            }
        }
    }

    IEnumerator PlayText()
    {
        foreach (char c in story)
        {
            txt.text += c;
            GameObject.Find("SFXSource").GetComponent<AudioSource>().PlayOneShot(a);
            yield return new WaitForSecondsRealtime(d);
        }
    }
}
