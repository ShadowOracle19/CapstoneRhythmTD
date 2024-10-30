using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConductorV2 : MonoBehaviour
{
    //Conductor instance
    public static ConductorV2 instance;
    void Awake()
    {
        instance = this;
    }

    public float bpm = 160;//song beats per minute
    public float crotchet;//Gives the time duration of a beat, calculated from the bpm
    public float songPosition;
    public float songPositionInBeats;//current song position in beats
    public float dspSongTime;//how many seconds have passed since the song started
    public AudioSource musicSource;

    public float offset;

    //The number of beats in each loop
    public float beatsPerLoop;

    //the total number of loops completed since the looping clip first started
    public int completedLoops = 0;

    //The current position of the song within the loop in beats.
    public float loopPositionInBeats;

    //The current relative position of the song within the loop measured between 0 and 1.
    public float loopPositionInAnalog;

    public float beatThreshold = 0.45f;

    // Start is called before the first frame update
    void Start()
    {
        //load the audio source attached to the conductor gameobject
        musicSource = GetComponent<AudioSource>();

        //calculate the number of seconds in each beat
        crotchet = 60 / bpm;

        //record the time when the music starts
        dspSongTime = (float)AudioSettings.dspTime;

        //Start the song
        musicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //determine how many seconds since the song started
        songPosition = (float)(AudioSettings.dspTime - dspSongTime - offset);

        //determine how many beats since the song started
        songPositionInBeats = songPosition / crotchet;

        //calculate the loop position
        if (songPositionInBeats >= (completedLoops + 1) * beatsPerLoop)
            completedLoops++;
        loopPositionInBeats = songPositionInBeats - completedLoops * beatsPerLoop + 1;

        loopPositionInAnalog = (loopPositionInBeats-1) / beatsPerLoop;

    }
}

