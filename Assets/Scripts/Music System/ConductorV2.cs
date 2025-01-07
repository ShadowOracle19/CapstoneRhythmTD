using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConductorV2 : MonoBehaviour
{
    //Conductor instance
    public static ConductorV2 instance;
    void Awake()
    {
        instance = this;
    }

    public bool isInTestingEnvironment = false;

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


    //Beat Thresholds
    //Note:
    //When checking beat threshold check late, miss, early, great, and then perfect
    public float missBeatThreshold = 0.45f;
    public float earlyBeatThreshold = 0.45f;
    public float greatBeatThreshold = 0.45f;
    public float perfectBeatThreshold = 0.61f;

    public float beatDuration;
    public int numberOfBeats;

    //public bool threshold;

    private int beatTrack;

    private int lastInterval;

    //Dynamic Music tracks
    public AudioSource drums;
    public AudioSource bass;
    public AudioSource piano;
    public AudioSource guitarH;
    public AudioSource guitarM;

    public AudioSource _ping;

    public AudioClip bpmTrack1; //80
    public AudioClip bpmTrack2; //100

    [Header("Metronome")]
    public GameObject ticker;
    public bool ping = false;//true left, false right
    public Vector3 rotationLeft;
    public Vector3 rotationRight;
    private Quaternion currentRotation;
    public Vector3 currentEulerAngles;
    

    public List<UnityEvent> triggerEvent = new List<UnityEvent>();

    public bool pauseConductor = false;
    public TextMeshProUGUI countInText;
    public bool countingIn = false;


    // Start is called before the first frame update
    void Start()
    {
        //if(isInTestingEnvironment)//if in a testing environment thats not the game scene play this to start conductor
        //{
        //    //load the audio source attached to the conductor gameobject
        //    musicSource = GetComponent<AudioSource>();

        //    //calculate the number of seconds in each beat
        //    crotchet = 60 / bpm;

        //    //record the time when the music starts
        //    dspSongTime = (float)AudioSettings.dspTime;

        //    completedLoops = 0;
        //    numberOfBeats = 0;
        //    beatTrack = 0;
        //    beatDuration = 0;

        //    //Start the song
        //    musicSource.Play();
        //}
        
    }

    public void CountUsIn(int _bpm)
    {
        ////record the time when the music starts
        AudioConfiguration config = AudioSettings.GetConfiguration();
        AudioSettings.Reset(config);
        dspSongTime = (float)AudioSettings.dspTime;

        PlayMusic();

        pauseConductor = true;
        //load the audio source attached to the conductor gameobject
        musicSource = GetComponent<AudioSource>();
        bpm = _bpm;
        completedLoops = 0;
        numberOfBeats = 0;
        beatTrack = 0;
        beatDuration = 0;
        countingIn = true;
        StartCoroutine(CountIn());
    }

    IEnumerator CountIn()
    {
        countInText.gameObject.SetActive(true);
        for (int i = 1; i <= 4; i++)
        {
            Debug.Log("count in " + i);
            countInText.text = i.ToString();
            _ping.Play();
            yield return new WaitForSeconds(60 / bpm);
        }
        StartConductor();
        yield return null;
    }

    public void StartConductor()
    {

        countingIn = false;
        CombatManager.Instance.knockEmDead.SetActive(true);
        CombatManager.Instance.knockEmDead.GetComponent<Animator>().SetTrigger("KnockEmDead");

        pauseConductor = false;
        countInText.gameObject.SetActive(false);
        Debug.Log("Conductor Start");


        //calculate the number of seconds in each beat
        crotchet = 60 / bpm;

        

        completedLoops = 0;
        numberOfBeats = 0;
        beatTrack = 0;
        beatDuration = 0;

        drums.volume = 0;
        bass.volume = 0;
        piano.volume = 0;
        guitarH.volume = 0;
        guitarM.volume = 0;

        if (GameManager.Instance.tutorialRunning)
        {
            CombatManager.Instance.metronome.SetActive(true);
            CursorTD.Instance.movementSequence = true;
        }

        musicSource.Play();
        //Start the song
        
        //musicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseConductor) return;

        if (GameManager.Instance.isGamePaused)
        {
            PauseMusic();
            return;
        }
        else
        {
            ResumeMusic();
        }

       

        //determine how many seconds since the song started
        songPosition = (float)(AudioSettings.dspTime - dspSongTime - offset);

        //determine how many beats since the song started
        songPositionInBeats = songPosition / crotchet;

        //calculate the loop position
        if (songPositionInBeats >= (completedLoops + 1) * beatsPerLoop)
            completedLoops++;
        loopPositionInBeats = songPositionInBeats - completedLoops * beatsPerLoop + 1;

        loopPositionInAnalog = (loopPositionInBeats-1) / beatsPerLoop;

        if (songPositionInBeats >= numberOfBeats + 1 * 1)
        {
            numberOfBeats++;
        }

        beatDuration = songPositionInBeats - numberOfBeats * 1;

        //threshold = InThreshHold();

        if(ping)
        {
            
            currentEulerAngles = Vector3.Lerp(currentEulerAngles, rotationLeft, beatDuration);
            currentRotation.eulerAngles = currentEulerAngles;
            ticker.transform.rotation = currentRotation;
        }
        else
        {
            
            currentEulerAngles = Vector3.Lerp(currentEulerAngles, rotationRight, beatDuration);
            currentRotation.eulerAngles = currentEulerAngles;
            ticker.transform.rotation = currentRotation;
        }


        TriggerBeatEvent(musicSource.timeSamples / (musicSource.clip.frequency * crotchet));
    }

    public void Tick()
    {
        ping = !ping;
    }

    public bool InThreshHold()
    {
        if(beatDuration >= perfectBeatThreshold)
        {
            Debug.Log("perfect Beat Hit");
            return true;
        }
        else if (beatDuration >= earlyBeatThreshold)//good beat hit
        {
            Debug.Log("early Beat Hit");
            return true;
        }
        else
        {
            Debug.Log("miss Beat Hit");
            return false;

        }


    }

    public bool Beat()
    {
        if (GameManager.Instance.isGamePaused)
            return false;

        if (songPositionInBeats >= beatTrack + 4 * 1)
        {
            beatTrack++;
            return true;
        }
        else
            return false;

        
    }

    public void TriggerBeatEvent(float interval)
    {
        if(Mathf.FloorToInt(interval) != lastInterval)
        {
            lastInterval = Mathf.FloorToInt(interval);
            foreach(UnityEvent _event in triggerEvent.ToArray())
            {
                _event.Invoke();
            }
        }
    }

    public void PauseMusic()
    {
        musicSource.Pause();
        drums.Pause();
        bass.Pause();
        piano.Pause();
        guitarH.Pause();
        guitarM.Pause();
    }

    public void ResumeMusic()
    {
        musicSource.UnPause();
        drums.UnPause();
        bass.UnPause();
        piano.UnPause();
        guitarH.UnPause();
        guitarM.UnPause();
    }

    public void StopMusic()
    {
        musicSource.Stop();
        drums.Stop();
        bass.Stop();
        piano.Stop();
        guitarH.Stop();
        guitarM.Stop();
    }

    public void PlayMusic()
    {
        Debug.Log("music started");
        drums.Play();
        bass.Play();
        piano.Play();
        guitarH.Play();
        guitarM.Play();

        
    }
}

public enum _BeatResult
{
    late, miss, early, great, perfect
}

