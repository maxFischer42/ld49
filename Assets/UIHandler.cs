using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{

    public GameObject gemHandler;
    public Text gems;
    public Text gemShadow;
    public void UpdateGems()
    {
        if(gemHandler.activeInHierarchy == false) // activate the gem text when the first gem is collected
        {
            gemHandler.SetActive(true);
        }
        string s = PlayerPrefs.GetInt("Gem").ToString();
        gems.text = s;
        gemShadow.text = s;
    }
}
