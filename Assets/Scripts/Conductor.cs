using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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


    //public double bpm = 140.0F;

    //double nextTick = 0.0F; // The next tick in dspTime
    //double sampleRate = 0.0F;
    //bool ticked = false;

    //public AudioSource audioSource;

    //void Start()
    //{
    //    double startTick = AudioSettings.dspTime;
    //    sampleRate = AudioSettings.outputSampleRate;

    //    nextTick = startTick + (60.0 / bpm);
    //}

    //void LateUpdate()
    //{
    //    if (!ticked && nextTick >= AudioSettings.dspTime)
    //    {
    //        ticked = true;
    //        BroadcastMessage("OnTick");
    //    }
    //}

    //// Just an example OnTick here
    //void OnTick()
    //{
    //    Debug.Log("Tick");
    //    // GetComponent<AudioSource>().Play();
    //}

    //void FixedUpdate()
    //{
    //    double timePerTick = 60.0f / bpm;
    //    double dspTime = AudioSettings.dspTime;

    //    while (dspTime >= nextTick)
    //    {
    //        ticked = false;
    //        nextTick += timePerTick;
    //    }
    //}

    [SerializeField] private float _bpm;
    [SerializeField] private AudioSource _audioSource;
    //[SerializeField] public Intervals[] _intervals;
    public List<Intervals> _intervals = new List<Intervals>();

    private void Update()
    {
        foreach(Intervals interval in _intervals)
        {
            float sampledTime = (_audioSource.timeSamples / (_audioSource.clip.frequency * interval.GetIntervalLength(_bpm)));
            interval.CheckForNewInterval(sampledTime);
        }
    }

}

[System.Serializable]
public class Intervals
{
    [SerializeField] public float _steps;
    [SerializeField] public UnityEvent _trigger;
    private int _lastInterval;

    public float GetIntervalLength(float bpm)
    {
        return 60f / (bpm * _steps);
    }

    //checks if we crossed a new beat
    public void CheckForNewInterval(float interval)
    {
        if(Mathf.FloorToInt(interval) != _lastInterval)
        {
            _lastInterval = Mathf.FloorToInt(interval);
            _trigger.Invoke();
        }
    }
}
