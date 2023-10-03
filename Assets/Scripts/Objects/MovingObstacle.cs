using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    Transform movingObject;
    int count = 0;                      //Counter
    public int length = 2000;
    public bool moveInX = false;
    public bool moveInY = false;
    public bool moveInZ = false;
    public float moveSpeedDirectionX = 0.01f;
    public float moveSpeedDirectionY = 0.01f;
    public float moveSpeedDirectionZ = 0.01f;

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
            if (moveInZ) { position.z = position.z - moveSpeedDirectionZ; }
            if (moveInX) { position.x = position.x - moveSpeedDirectionX; }
            if (moveInY) { position.y = position.y - moveSpeedDirectionY; }

            movingObject.position = position;
        }
        else if (count < length*2)
        {
            count++;
            Vector3 position = movingObject.position;
            if (moveInZ) { position.z = position.z + moveSpeedDirectionZ; }
            if (moveInX) { position.x = position.x + moveSpeedDirectionX; }
            if (moveInY) { position.y = position.y + moveSpeedDirectionY; }
            //Debug.Log("Moving Forward");
            movingObject.position = position;
        }
        else
        {
            count = 0;
        }
    }
}
