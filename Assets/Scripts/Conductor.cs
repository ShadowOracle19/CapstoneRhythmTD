using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    #region dont touch this
    private static Conductor _instance;
    public static Conductor Instance
    {
        get
        {
            if (_instance is null)
            {
                Debug.LogError("Conductor is NULL");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    #endregion

    
    public double bpm = 140.0F;

    double nextTick = 0.0F; // The next tick in dspTime
    double sampleRate = 0.0F;
    bool ticked = false;

    void Start()
    {
        double startTick = AudioSettings.dspTime;
        sampleRate = AudioSettings.outputSampleRate;

        nextTick = startTick + (60.0 / bpm);
    }

    void LateUpdate()
    {
        if (!ticked && nextTick >= AudioSettings.dspTime)
        {
            ticked = true;
            BroadcastMessage("OnTick");
        }
    }

    // Just an example OnTick here
    void OnTick()
    {
        Debug.Log("Tick");
        // GetComponent<AudioSource>().Play();
    }

    void FixedUpdate()
    {
        double timePerTick = 60.0f / bpm;
        double dspTime = AudioSettings.dspTime;

        while (dspTime >= nextTick)
        {
            ticked = false;
            nextTick += timePerTick;
        }
    }

}
