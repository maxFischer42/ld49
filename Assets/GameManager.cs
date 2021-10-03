using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{

    public bool startedMiniBoss = false;
    public Transform cam;
    public Controller2D c;

    public Miniboss m;


    public Typewriter tw;

    public GameObject preMiniboss;
    public GameObject miniBoss;
    public GameObject postMiniboss;

    public Dialogue[] midOutro;

    public void StartMB()
    {
        preMiniboss.SetActive(false);
        miniBoss.SetActive(true);
        m.enabled = true;
        InCutscene(false);
        FreeCam();
    }

    public void InCutscene(bool state)
    {
        c.isInCutscene = state;
    }

    public void MiniBossBattleIntro()
    {
        cam.GetComponent<CameraFollower>().enabled = false;
        cam.position = new Vector3(-0.5f, 540, -10);
    }

    public void MiniBossBattleOutro()
    {
        tw.Speak(midOutro);
        postMiniboss.SetActive(true);
        miniBoss.SetActive(false);
    }

    public void FreeCam()
    {
        cam.GetComponent<CameraFollower>().enabled = true;
    }
}

[Serializable]
public class Dialogue
{
    public Sprite portrait;
    public string story;
}
