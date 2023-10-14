using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ReferenceManager : MonoBehaviour
{
    public static ReferenceManager instance;

    public GameObject player;
    public CameraFollow mainCamera;
    public GameObject spawnPoint;
    public Image currentEnergyImage;
    public Image maxEnergyImage;
    public LevelFinish levelFinish;

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