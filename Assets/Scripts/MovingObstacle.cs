using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    Transform movingObject;
    int count = 0;                      //Counter
    public int length = 2000;

    // Start is called before the first frame update
    void Start()
    {
        movingObject = GetComponent<Transform>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if(count < length)
        {
            count++;
            Vector3 position = movingObject.position;
            position.z = position.z - 0.01f;
            //Debug.Log("Moving Backward");
            movingObject.position = position;
        }
        else if (count < length*2)
        {
            count++;
            Vector3 position = movingObject.position;
            position.z = position.z + 0.01f;
            //Debug.Log("Moving Forward");
            movingObject.position = position;
        }
        else
        {
            count = 0;
        }
    }
}
