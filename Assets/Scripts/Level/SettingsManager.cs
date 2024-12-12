using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    [Header("Audio")] 
    [SerializeField] private AudioMixer masterAudioMixer;

    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private TextMeshProUGUI masterVolumeText;

    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private TextMeshProUGUI musicVolumeText;

    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private TextMeshProUGUI sfxVolumeText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Volume();
    }

    //Audio Setting Function
    void Volume()
    {
        masterAudioMixer.SetFloat("_masterVolume", Mathf.Log10(masterVolumeSlider.value) * 20);
        masterVolumeText.text = Mathf.RoundToInt(masterVolumeSlider.value * 100) + "%";

        masterAudioMixer.SetFloat("_musicVolume", Mathf.Log10(musicVolumeSlider.value) * 20);
        musicVolumeText.text = Mathf.RoundToInt(musicVolumeSlider.value * 100) + "%";

        masterAudioMixer.SetFloat("_SFXVolume", Mathf.Log10(sfxVolumeSlider.value) * 20);
        float sfxVolume = Mathf.RoundToInt(sfxVolumeSlider.value * 100);
        sfxVolumeText.text = sfxVolume + "%";
        

    }

    //Text Speed function
    public void HandleTextSpeedData(int num)
    {
        switch (num)
        {
            //Medium
            case 0:
                GameManager.Instance.textSpeed = 0.05f;
                break;
            //Slow
            case 1:
                GameManager.Instance.textSpeed = 0.1f;
                break;
            //Fast
            case 2:
                GameManager.Instance.textSpeed = 0.01f;
                break;

            default:
                break;
        }
    }

    //Video Setting functions
    public void HandleResolutionDropdownData(int num)
    {
        //1920x1080
        if(num == 0)
        {
            SetGameResolution(new Vector2(1920, 1080));
        }
        //1280x720
        else if (num == 1)
        {
            SetGameResolution(new Vector2(1280, 720));
        }
        //800x600
        else if (num == 2)
        {
            SetGameResolution(new Vector2(960, 540));
        }
    }

    void SetGameResolution(Vector2 resolution)
    {
        Screen.SetResolution((int)resolution.x, (int)resolution.y, true);
    }
}
