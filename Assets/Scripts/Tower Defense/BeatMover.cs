using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatMover : MonoBehaviour
{
    public Transform endPos;
    public Vector3 originPos;

    public float positionInBeats;
    public float positionInAnalog;
    public float loops;

    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.position;   
    }

    // Update is called once per frame
    void Update()
    {

        positionInAnalog = ConductorV2.instance.beatDuration;

        //transform.position = Vector3.Lerp(transform.position, endPos.position, Time.deltaTime);
        transform.position = Vector3.LerpUnclamped(transform.position, endPos.position, positionInAnalog);

        if(transform.position == endPos.position)
        {
            Destroy(gameObject, 4);
        }
    }
}
