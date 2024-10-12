using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorTD : MonoBehaviour
{
    private bool isMoving;
    private Vector3 originPos, targetPos;
    private float timeToMove = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.anyKey)
        //{
        //    switch (Input.inputString)
        //    {
        //        case "w":
        //            transform.position = new Vector3();
        //            break;

        //        case "a":
        //            Debug.Log("a");
        //            break;

        //        case "s":
        //            Debug.Log("s");
        //            break;

        //        case "d":
        //            Debug.Log("d");
        //            break;

        //        default:
        //            break;
        //    }
        //}

        if(Input.GetKeyUp(KeyCode.W) && !isMoving)
        {
            StartCoroutine(MovePlayer(Vector3.up));
        }
        else if (Input.GetKeyUp(KeyCode.A) && !isMoving)
        {
            StartCoroutine(MovePlayer(Vector3.left));
        }
        else if (Input.GetKeyUp(KeyCode.S) && !isMoving)
        {
            StartCoroutine(MovePlayer(Vector3.down));
        }
        else if (Input.GetKeyUp(KeyCode.D) && !isMoving)
        {
            StartCoroutine(MovePlayer(Vector3.right));
        }
    }

    private IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;

        float elapsedTime = 0;

        originPos = transform.position;

        targetPos = originPos + direction;

        while(elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(originPos, targetPos, elapsedTime / timeToMove);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;

        isMoving = false;

        yield return null;
    }
}
