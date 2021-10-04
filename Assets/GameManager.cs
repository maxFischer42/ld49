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
    public Boss b;
    


    public Typewriter tw;

    public GameObject preMiniboss;
    public GameObject miniBoss;
    public GameObject postMiniboss;

    public Dialogue[] midOutro;
    public Dialogue[] outro;

    public UIHandler ui;

    public GameObject jumpMessage;

    public GameObject preBoss;
    public GameObject boss;
    public GameObject postBoss;

    public AudioSource musicPlayer;
    public AudioSource sfxPlayer;
    public GameObject gems;


    // Reset any playerprefs set in a previous playthrough
    public void Start()
    {
        HandlePlayerPrefs();
    }

    public void HandlePlayerPrefs()
    {
        PlayerPrefs.SetInt("Gem", 0);
        musicPlayer.volume = PlayerPrefs.GetFloat("Music");
        sfxPlayer.volume = PlayerPrefs.GetFloat("Sound");
        if (PlayerPrefs.GetInt("NoGem") > 0) Destroy(gems);
        
    }

    public void StartMB()
    {
        preMiniboss.SetActive(false);
        miniBoss.SetActive(true);
        m.enabled = true;
        InCutscene(false);
        FreeCam();
    }

    public void StartB()
    {
        preBoss.SetActive(false);
        boss.SetActive(true);
        b.enabled = true;
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

    public void BossBattleIntro()
    {
        cam.GetComponent<CameraFollower>().enabled = false;
        cam.position = new Vector3(-0.5f, 1082, -10);
    }

    public void MiniBossBattleOutro()
    {
        tw.Speak(midOutro);
        postMiniboss.SetActive(true);
        miniBoss.SetActive(false);
    }

    public void BossBattleOutro()
    {
        tw.Speak(outro);
        // END THE GAME HERE!!!!!
        Initiate.Fade("Ending", Color.black, 0.4f);
    }

    public void FreeCam()
    {
        cam.GetComponent<CameraFollower>().enabled = true;
    }

    public void AddGem(int count)
    {
        int g = PlayerPrefs.GetInt("Gem");
        g += count;
        if(g >= 50)
        {
            UpdatePlayerJumps();
            g -= 50;
        }
        PlayerPrefs.SetInt("Gem", g);
        ui.UpdateGems();
    }

    public void UpdatePlayerJumps()
    {
        GameObject j = (GameObject)Instantiate(jumpMessage, c.transform);
        j.transform.parent = null;
        c.maxJumpCount++;
        c.minJumpCount++;
        StartCoroutine(HandleEffectFadeout(j));
        Destroy(j, 5f);
    }

    IEnumerator HandleEffectFadeout(GameObject g)
    {
        for (int i = 0; i < 300; i++)
        {
            g.transform.position = new Vector3(g.transform.position.x, g.transform.position.y + 0.01f);
            yield return new WaitForSeconds(1f);
        }
    }
}


[Serializable]
public class Dialogue
{
    public Sprite portrait;
    public string story;
}
