using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BeatPress : MonoBehaviour
{
    public Image beatIndicator;

    public GameObject miss;
    public GameObject good;
    public GameObject perfect;

    public bool inputPressed;

    public float lastBeat;
    // Start is called before the first frame update
    void Start()
    {
        lastBeat = 0;
    }

    // Update is called once per frame
    void Update()
    {
        beatIndicator.color = new Color(1, 1, 1, ConductorV2.instance.loopPositionInAnalog);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //miss happens between 0 and 0.3, good happens between 0.31 and 0.6, perfect happens between 0.61 and 1
            if (ConductorV2.instance.loopPositionInAnalog <= 0.3) //miss
            {
                Debug.Log("miss");
                miss.SetActive(true);
                good.SetActive(false);
                perfect.SetActive(false);
            }
            else if(ConductorV2.instance.loopPositionInAnalog < 0.6 && ConductorV2.instance.loopPositionInAnalog > 0.31) //good
            {
                Debug.Log("good");
                miss.SetActive(false);
                good.SetActive(true);
                perfect.SetActive(false);
            }
            else if(ConductorV2.instance.loopPositionInAnalog < 1 && ConductorV2.instance.loopPositionInAnalog > 0.61)
            {
                Debug.Log("perfect");
                miss.SetActive(false);
                good.SetActive(false);
                perfect.SetActive(true);
            }
        }
        
    }
}
