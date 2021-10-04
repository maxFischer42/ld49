using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleButtonHandler : MonoBehaviour
{

    public GameObject ui;
    public GameObject cutscene;

    public Slider musicSlider;
    public Slider soundSlider;

    public Text musicLabel;
    public Text soundLabel;

    public Toggle gemMode;

    public bool isOptions = false;

    public void Start()
    {
        

        if(PlayerPrefs.GetInt("HasOpened") == 0)
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("HasOpened", 1);
            PlayerPrefs.SetFloat("Music", 0.05f);
            PlayerPrefs.SetFloat("Sound", 0.3f);
            PlayerPrefs.SetInt("NoGem", 0);
        }
        if (GetComponent<AudioSource>())
        {
            GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("Music");
        }
        if (!isOptions) return;
        musicSlider.value = PlayerPrefs.GetFloat("Music");
        soundSlider.value = PlayerPrefs.GetFloat("Sound");
        gemMode.isOn = (PlayerPrefs.GetInt("NoGem") == 1 ? true : false);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Delete)) PlayerPrefs.DeleteAll();
        if (!isOptions) return;
        float mu = Mathf.Round((musicSlider.value / musicSlider.maxValue) * 100);
        float su = Mathf.Round((soundSlider.value / soundSlider.maxValue) * 100);
        musicLabel.text = mu.ToString() + "%";
        soundLabel.text = su.ToString() + "%";
    }

    public void StartGame()
    {
        ui.SetActive(false);
        cutscene.SetActive(true);
    }

    public void OnMusicChanged()
    {
        PlayerPrefs.SetFloat("Music", musicSlider.value);
    }

    public void OnSoundChanged()
    {
        PlayerPrefs.SetFloat("Sound", soundSlider.value);
    }

    public void OnGemMode()
    {
        PlayerPrefs.SetInt("NoGem", (gemMode.isOn ? 1 : 0));
    }

    public void OnExit()
    {
        Application.Quit();
        print("Quit the game; this only shows in the editor.");
    }

}
