using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSelector : MonoBehaviour
{
    private InputMaster controls;

    //Upgrade selection
    private static int bodySelected;        // 0 = none, 1 = 1 battery, 2... etc
    private static int legSelected;         // 0 = default wheel, 1 = jumping
    private static int rightArmSelected;    // 0 = default arm, 1 = gun1
    private static int leftArmSelected;   // 0 = default arm

    private List<int> legPieces;
    public List<int> rightArmPieces;
    private List<int> bodyPieces;
    private List<int> leftArmPieces;

    //private static int[] bodyPieces;
    //private static int[] legPieces;
    //private static int[] rightArmPieces;
    //private static int[] leftArmPieces;

    //Model Pieces
    private static GameObject basicArm;
    private static GameObject cannonArm;

    private void Awake()
    {
        controls = new InputMaster();
        controls.Player.ToggleLegs.performed += _ => toggleLeg();
        controls.Player.ToggleBody.performed += _ => toggleBody();
        controls.Player.ToggleRightArm.performed += _ => toggleRightArm();
        controls.Player.ToggleLeftArm.performed += _ => toggleLeftArm();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Cache references to the arm GameObjects
        basicArm = transform.Find("Model/RobotBasicArmUnScuffedUnTexed").gameObject;
        cannonArm = transform.Find("Model/BasicCannonArmUnTexed").gameObject;

        // Initialise arrays for available upgrade pieces
        //legPieces = new int[] { 0 };     // change later to only 0
        //rightArmPieces = new int[] { 0 };     // change later to only 0
        //bodyPieces = new int[] { 0 };     // change later to only 0
        //leftArmPieces = new int[] { 0 };     // change later to only 0

        legPieces = new List<int> { 0 };                            // Initialized with 0
        rightArmPieces = new List<int> { 0 };       
        bodyPieces = new List<int> { 0 };
        leftArmPieces = new List<int> { 0 };

        // Currently selected pieces
        bodySelected = 0;
        legSelected = 0;
        rightArmSelected = 0;
        leftArmSelected = 0;

        UpdateSlotLists();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateSlotLists()
    {
    /* 
     * current upgrade list:
     * upgradesFound[difficulty][0] = Jump Upgrade (legs)
     * upgradesFound[difficulty][1] = Gun Upgrade (right arm)
     * upgradesFound[difficulty][2] = Double Jump Upgrade (legs)
    */

        if (GameSettings.upgradesFound[LevelState.currentDifficulty][0] && !legPieces.Contains(1))
        {
            // Add an element of value 1 to legPieces array
            legPieces.Add(1);
        }

        if (GameSettings.upgradesFound[LevelState.currentDifficulty][1] && !rightArmPieces.Contains(1))
        {
            // Add an element of value 1 to rightArmPieces array
            rightArmPieces.Add(1);
        }

        //REMEMBER TO ADD THE "&& !contains" part!!!
        if (GameSettings.upgradesFound[LevelState.currentDifficulty][2])    //REMEMBER TO ADD THE "&& !contains" part!!!
        {
            // Nothing for now, implement in future, 
        }
    }


    public void toggleLeg()
    {
        // Find the index of the currently selected leg piece
        int currentIndex = legPieces.IndexOf(legSelected);

        // Select the next leg piece in the array, wrapping to the start if at the end
        int nextIndex = (currentIndex + 1) % legPieces.Count;
        legSelected = legPieces[nextIndex];

        Debug.Log("Leg piece selected: " + legSelected);

        if(legSelected == 1)
        {
            PlayerState.canJump = true;
        }
        else
        {
            PlayerState.canJump = false;
        }
    }

    public void toggleBody()
    {
        // Find the index of the currently selected body piece
        int currentIndex = bodyPieces.IndexOf(bodySelected);

        // Select the next body piece in the list, wrapping to the start if at the end
        int nextIndex = (currentIndex + 1) % bodyPieces.Count;
        bodySelected = bodyPieces[nextIndex];

        Debug.Log("Body piece selected: " + bodySelected);

        if (bodySelected == 0)
        {
            PlayerState.canDoubleJump = false;
        }
        else
        {
            PlayerState.canDoubleJump = true;
        }
    }

    public void toggleRightArm()
    {
        // Find the index of the currently selected right arm piece
        int currentIndex = rightArmPieces.IndexOf(rightArmSelected);

        // Select the next right arm piece in the list, wrapping to the start if at the end
        int nextIndex = (currentIndex + 1) % rightArmPieces.Count;
        rightArmSelected = rightArmPieces[nextIndex];

        Debug.Log("Right arm piece selected: " + rightArmSelected);


        if (rightArmSelected == 0)
        {
            basicArm.SetActive(true);
            cannonArm.SetActive(false);
            PlayerState.canShoot = false;
        }
        else
        {
            basicArm.SetActive(false);
            cannonArm.SetActive(true);
            PlayerState.canShoot = true;
        }
    }

    public void toggleLeftArm()
    {
        // Find the index of the currently selected left arm piece
        int currentIndex = leftArmPieces.IndexOf(leftArmSelected);

        // Select the next left arm piece in the list, wrapping to the start if at the end
        int nextIndex = (currentIndex + 1) % leftArmPieces.Count;
        leftArmSelected = leftArmPieces[nextIndex];

        Debug.Log("Left arm piece selected: " + leftArmSelected);
    }
}
