using UnityEngine;
using System.Collections;

public class LoadURL : MonoBehaviour
{

    public void OpenURL(string url)
    {
        Application.OpenURL(url);
        Debug.Log("is this working?");
    }

}