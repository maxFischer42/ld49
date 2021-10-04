using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossHandler : MonoBehaviour
{
    public GameObject startTrigger;
    public GameObject introTimeline;
    public GameManager gm;

    public Dialogue[] startDialogues;

    bool eventStarted = false;
    int counter = 0;
    float time = 0f;

    public void StartEvent()

    {
        Destroy(startTrigger);
        gm.BossBattleIntro();
        introTimeline.SetActive(true);
        eventStarted = true;

    }

    private void Update()
    {
        if (eventStarted)
        {
            time += Time.deltaTime;
            if (time > 5f && counter == 0)
            {
                counter = 1;
            }
            else if (time > 7f && counter == 2)
            {
                gm.startedMiniBoss = true;
                gm.StartB();
                gm.c.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                Destroy(gameObject);
            }
        }
        if (counter == 1)
        {
            counter = 2;
            var c = startDialogues;
            gm.tw.Speak(c);
        }
    }
}
