using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSelector : MonoBehaviour
{
    private InputMaster controls;

    //Upgrade selection
    private static int[] bodyPieces;
    private static int bodySelected;        // 0 = none, 1 = 1 battery, 2... etc
    private static int[] legPieces;
    private static int legSelected;         // 0 = default wheel, 1 = jumping
    private static int[] rightArmPieces;
    private static int rightArmSelected;    // 0 = default arm, 1 = gun1
    private static int[] leftArmPieces;
    private static int leftArmSelected;   // 0 = default arm

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
        bodyPieces = new int[] { 0 };     // change later to only 0
        legPieces = new int[] { 0, 1 };     // change later to only 0
        rightArmPieces = new int[] { 0, 1 };     // change later to only 0
        leftArmPieces = new int[] { 0 };     // change later to only 0

        bodySelected = 0;
        legSelected = 0;
        rightArmSelected = 0;
        leftArmSelected = 0;

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public static void toggleLeg()
    {
        // Find the index of the currently selected leg piece
        int currentIndex = Array.IndexOf(legPieces, legSelected);

        // Select the next leg piece in the array, wrapping to the start if at the end
        int nextIndex = (currentIndex + 1) % legPieces.Length;
        legSelected = legPieces[nextIndex];

        Debug.Log("Leg piece selected: " + legSelected);
    }

    public static void toggleBody()
    {
        // Find the index of the currently selected body piece
        int currentIndex = Array.IndexOf(bodyPieces, bodySelected);

        // Select the next body piece in the array, wrapping to the start if at the end
        int nextIndex = (currentIndex + 1) % bodyPieces.Length;
        bodySelected = bodyPieces[nextIndex];

        Debug.Log("Body piece selected: " + bodySelected);
    }

    public static void toggleRightArm()
    {
        // Find the index of the currently selected right arm piece
        int currentIndex = Array.IndexOf(rightArmPieces, rightArmSelected);

        // Select the next right arm piece in the array, wrapping to the start if at the end
        int nextIndex = (currentIndex + 1) % rightArmPieces.Length;
        rightArmSelected = rightArmPieces[nextIndex];

        Debug.Log("Right arm piece selected: " + rightArmSelected);
    }

    public static void toggleLeftArm()
    {
        // Find the index of the currently selected left arm piece
        int currentIndex = Array.IndexOf(leftArmPieces, leftArmSelected);

        // Select the next left arm piece in the array, wrapping to the start if at the end
        int nextIndex = (currentIndex + 1) % leftArmPieces.Length;
        leftArmSelected = leftArmPieces[nextIndex];

        Debug.Log("Left arm piece selected: " + leftArmSelected);
    }
}
