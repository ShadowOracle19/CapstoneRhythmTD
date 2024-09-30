using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    //Gameplay/General


    //Video


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

    void Volume()
    {
        masterAudioMixer.SetFloat("_masterVolume", Mathf.Log10(masterVolumeSlider.value) * 20);
        masterAudioMixer.SetFloat("_musicVolume", Mathf.Log10(musicVolumeSlider.value) * 20);
        masterAudioMixer.SetFloat("_SFXVolume", Mathf.Log10(sfxVolumeSlider.value) * 20);
    }
}
