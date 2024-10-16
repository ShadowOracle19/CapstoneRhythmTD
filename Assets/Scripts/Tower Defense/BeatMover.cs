using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatMover : MonoBehaviour
{
    public Transform endPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, endPos.position, Time.deltaTime * 1);

        if(transform.position == endPos.position)
        {
            Destroy(gameObject, 4);
        }
    }
}
