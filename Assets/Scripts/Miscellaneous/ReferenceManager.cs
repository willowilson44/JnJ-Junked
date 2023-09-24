using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceManager : MonoBehaviour
{
    public static ReferenceManager instance;

    public GameObject player;
    public GameObject spawnPoint;
    public string[] enemies =
    {
        "Scrapper",
        "Guard",
        "Dalek"
    };

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}