using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    public string scene;
    public Color c = Color.white;
    public float t = 0.75f;
    public bool startFade = true;

    // Start is called before the first frame update
    void Start()
    {
        if (startFade) DoFade(scene);
    }

    public void DoFade(string n)
    {
        if (n == null) n = scene;
        Initiate.Fade(n, c, t);
    }
}
