using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class MinibossHandler : MonoBehaviour
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
        gm.MiniBossBattleIntro();
        introTimeline.SetActive(true);
        eventStarted = true;

    }


    private void Update()
    {
        if(eventStarted)
        {
            time += Time.deltaTime;
            if(time > 4f && counter == 0)
            {
                counter = 1;
            } else if (time > 5f && counter == 2)
            {
                gm.startedMiniBoss = true;
                gm.StartMB();
                gm.c.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                Destroy(gameObject);
            }
        }
        if(counter == 1)
        {
            counter = 2;
            var c = startDialogues;
            gm.tw.Speak(c);
        }
    }


}
