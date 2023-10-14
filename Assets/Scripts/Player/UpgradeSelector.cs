using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class UpgradeSelector : MonoBehaviour
{
    private InputMaster controls;

    //Upgrade selection
    private static int bodySelected;        
    private static int legSelected;      
    private static int rightArmSelected;   
    private static int leftArmSelected; 

    private List<int> legPieces;
    public List<int> rightArmPieces;
    private List<int> bodyPieces;
    private List<int> leftArmPieces;

    //Model Pieces
    private static GameObject basicRightArm;
    private static GameObject basicLeftArm;
    private static GameObject basicBody;
    private static GameObject blasterRightArm;
    private static GameObject hyperBlasterRightArm;
    private static GameObject heavyArmor;
    private static GameObject gravitronArmor;
    private static GameObject powerArmor;
    private static GameObject torchLeftArm;
    private static GameObject thrusterLeftArm;
    private static GameObject torch;

    //UI Button Text updates
    TMP_Text topText;
    TMP_Text leftText;
    TMP_Text rightText;
    TMP_Text bottomText;

    //coroutines
    private Coroutine healCoroutine = null;
    private Coroutine damageCoroutine = null;

    private void Awake()
    {
        controls = new InputMaster();
        controls.Player.ToggleLegs.performed += _ => toggleLeg(true);
        controls.Player.ToggleBody.performed += _ => toggleBody(true);
        controls.Player.ToggleRightArm.performed += _ => toggleRightArm(0);
        controls.Player.ScrollRightArm.performed += ctx => OnScroll(ctx);
        controls.Player.ToggleLeftArm.performed += _ => toggleLeftArm(true);
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    public void OnScroll(InputAction.CallbackContext context)
    {
        Vector2 scrollValue = context.ReadValue<Vector2>();

        if (scrollValue.y > 0)
        {
            toggleRightArm(0);
        }
        else if (scrollValue.y < 0)
        {
            toggleRightArm(1);
        }
    }


    // Start is called before the first frame update
    void Start()
    {

        topText = GameObject.Find("GUI/Canvas/Upgrades/Top/Text").GetComponent<TMP_Text>();
        leftText = GameObject.Find("GUI/Canvas/Upgrades/Left/Text").GetComponent<TMP_Text>();
        rightText = GameObject.Find("GUI/Canvas/Upgrades/Right/Text").GetComponent<TMP_Text>();
        bottomText = GameObject.Find("GUI/Canvas/Upgrades/Bottom/Text").GetComponent<TMP_Text>();


        // Cache references to the arm GameObjects
        basicRightArm = transform.Find("Model/RobotBasicArmUnScuffedUnTexed").gameObject;
        blasterRightArm = transform.Find("Model/BasicCannonArmUnTexed").gameObject;
        hyperBlasterRightArm = transform.Find("Model/HyperBlaster").gameObject;
        basicLeftArm = transform.Find("Model/RobotBasicArmUnScuffedUnTexed (1)").gameObject;
        torchLeftArm = transform.Find("Model/TorchArm").gameObject;
        torch = transform.Find("Torch").gameObject;
        thrusterLeftArm = transform.Find("Model/ThrusterArm").gameObject;
        basicBody = transform.Find("Model/BasicTorsoUnscuffedUnTexed").gameObject;
        heavyArmor = transform.Find("Model/HeavyArmor").gameObject;
        gravitronArmor = transform.Find("Model/GravitronArmor").gameObject;
        powerArmor = transform.Find("Model/PowerArmor").gameObject;

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

        toggleBody(false);
        toggleLeftArm(false);
        toggleLeg(false);
        toggleRightArm(2);
    }

    public void UpdateSlotLists()
    {

        /* 
         * current upgrade list:
         * upgradesFound[difficulty][0] = Jump Upgrade (legs)
         * upgradesFound[difficulty][1] = Gun Upgrade (right arm)
         * upgradesFound[difficulty][2] = Hyper Blaster Upgrade (right Arm)
         * upgradesFound[difficulty][3] = Heavy Armor Upgrade (body)
         * upgradesFound[difficulty][4] = Gravitron Armor Upgrade (body)
         * upgradesFound[difficulty][5] = Power Armor Upgrade (body)
         * upgradesFound[difficulty][6] = Torch (left Arm)
         * upgradesFound[difficulty][7] = Double Jump Upgrade (legs)    (NOT YET IMPLEMENTED)
         * 
         * 
         *  bodySelected;      0 = none, 1 = heavy armor, 2 = gravitron armor, 3 = power armor
         *  legSelected;       0 = default wheel, 1 = jumping
         *  rightArmSelected;  0 = default arm, 1 = gun1, 2 = hyperblaster
         *  leftArmSelected;   0 = default arm, 1 = torch
        */

        if (GameSettings.upgradesFound[LevelState.currentDifficulty][0] && !legPieces.Contains(1))
        {
            // Add an element of value 1 to legPieces array
            legPieces.Add(1);
            legSelected = 1;
            toggleLeg(false);
        }

        if (GameSettings.upgradesFound[LevelState.currentDifficulty][1] && !rightArmPieces.Contains(1))
        {
            // Add an element of value 1 to rightArmPieces array
            rightArmPieces.Add(1);
            rightArmSelected = rightArmPieces.IndexOf(1);
            toggleRightArm(2);
        }

        //REMEMBER TO ADD THE "&& !contains" part!!!
        if (GameSettings.upgradesFound[LevelState.currentDifficulty][2] && !rightArmPieces.Contains(2))    //REMEMBER TO ADD THE "&& !contains" part!!!
        {
            // Nothing for now, implement in future, 
            rightArmPieces.Add(2);
            rightArmSelected = rightArmPieces.IndexOf(2);
            toggleRightArm(2);
        }

        //REMEMBER TO ADD THE "&& !contains" part!!!
        if (GameSettings.upgradesFound[LevelState.currentDifficulty][5] && !bodyPieces.Contains(3))    //REMEMBER TO ADD THE "&& !contains" part!!!
        {
            // Nothing for now, implement in future, 
            bodyPieces.Add(3);
            bodySelected = 3;
            toggleBody(false);
        } //REMEMBER TO ADD THE "&& !contains" part!!!

        //REMEMBER TO ADD THE "&& !contains" part!!!
        if (GameSettings.upgradesFound[LevelState.currentDifficulty][4] && !bodyPieces.Contains(2))    //REMEMBER TO ADD THE "&& !contains" part!!!
        {
            // Nothing for now, implement in future, 
            bodyPieces.Add(2);
            bodySelected = 2;
            toggleBody(false);
        }

        //REMEMBER TO ADD THE "&& !contains" part!!!
        if (GameSettings.upgradesFound[LevelState.currentDifficulty][3] && !bodyPieces.Contains(1))    //REMEMBER TO ADD THE "&& !contains" part!!!
        {
            // Nothing for now, implement in future, 
            bodyPieces.Add(1);
            bodySelected = 1;
            toggleBody(false);
        }

        if (GameSettings.upgradesFound[LevelState.currentDifficulty][6] && !leftArmPieces.Contains(1))    //REMEMBER TO ADD THE "&& !contains" part!!!
        {
            // Nothing for now, implement in future, 
            leftArmPieces.Add(1);
            leftArmSelected = 1;
            toggleLeftArm(false);
        }
    }


    public void toggleLeg(bool increment)
    {
        // Find the index of the currently selected leg piece
        int currentIndex = legPieces.IndexOf(legSelected);

        if (increment)
        {
            // Select the next leg piece in the array, wrapping to the start if at the end
            int nextIndex = (currentIndex + 1) % legPieces.Count;
        legSelected = legPieces[nextIndex];
        }

        Debug.Log("Leg piece selected: " + legSelected);

        if(legSelected == 1)
        {
            PlayerState.canJump = true;
            bottomText.text = "Jump";
        }
        else
        {
            PlayerState.canJump = false;
            bottomText.text = "-";
        }
    }

    public void toggleBody(bool increment)
    {
        // Find the index of the currently selected body piece
        int currentIndex = bodyPieces.IndexOf(bodySelected);

        if (increment) 
        {
            // Select the next body piece in the list, wrapping to the start if at the end
            int nextIndex = (currentIndex + 1) % bodyPieces.Count;
            bodySelected = bodyPieces[nextIndex];
        }

        Debug.Log("Body piece selected: " + bodySelected);

        if (bodySelected == 1)
        {
            OnSwitchAwayFromPowerArmor();
            basicBody.SetActive(false);
            powerArmor.SetActive(false);
            heavyArmor.SetActive(true);
            gravitronArmor.SetActive(false);

            PlayerState.heavyArmor = true;
            PlayerState.gravitronArmor = false;
            PlayerState.powerArmor = false;
            topText.text = "Heavy Armor";
            PlayerState.UpdateEnergyMax();
            PlayerState.upgradeSpeedModifier = 25;
            PlayerState.UpdateSpeed();
            PlayerState.UpdateGravity();
            OnSwitchToHeavyArmor();
        }
        else if (bodySelected == 2)
        {
            OnSwitchAwayFromPowerArmor();
            if (PlayerState.heavyArmor == true)
            {
                OnSwitchAwayFromHeavyArmor();
                PlayerState.upgradeSpeedModifier = 0;
            }
            basicBody.SetActive(true);
            powerArmor.SetActive(false);
            heavyArmor.SetActive(false);
            gravitronArmor.SetActive(true);

            PlayerState.heavyArmor = false;
            PlayerState.gravitronArmor = true;
            PlayerState.powerArmor = false;
            topText.text = "Gravitron Armor";
            PlayerState.UpdateEnergyMax();
            PlayerState.UpdateEnergy();
            PlayerState.UpdateSpeed();
            PlayerState.UpdateGravity();
        }
        else if (bodySelected == 3)
        {
            if (PlayerState.heavyArmor == true)
            {
                OnSwitchAwayFromHeavyArmor();
                PlayerState.upgradeSpeedModifier = 0;
            }
            basicBody.SetActive(true);
            powerArmor.SetActive(true);
            heavyArmor.SetActive(false);
            gravitronArmor.SetActive(false);

            PlayerState.heavyArmor = false;
            PlayerState.gravitronArmor = false;
            PlayerState.powerArmor = true;
            topText.text = "Energon Armor";
            PlayerState.UpdateEnergyMax();
            PlayerState.UpdateEnergy();
            PlayerState.UpdateSpeed();
            PlayerState.UpdateGravity();
            OnSwitchToPowerArmor();
        }
        else
        {
            OnSwitchAwayFromPowerArmor();
            if (PlayerState.heavyArmor == true)
            {
                OnSwitchAwayFromHeavyArmor();
                PlayerState.upgradeSpeedModifier = 0;
            }
            basicBody.SetActive(true);
            powerArmor.SetActive(false);
            heavyArmor.SetActive(false);
            gravitronArmor.SetActive(false);

            PlayerState.heavyArmor = false;
            PlayerState.gravitronArmor = false;
            PlayerState.powerArmor = false;
            topText.text = "-";
            PlayerState.UpdateEnergyMax();
            PlayerState.UpdateEnergy();
            PlayerState.UpdateSpeed();
            PlayerState.UpdateGravity();
        }
    }

    public void toggleRightArm(int direction)
    {
        // Find the index of the currently selected right arm piece
        int currentIndex = rightArmPieces.IndexOf(rightArmSelected);

        if(direction == 0) { 
            // Select the next right arm piece in the list, wrapping to the start if at the end
            int nextIndex = (currentIndex + 1) % rightArmPieces.Count;
            rightArmSelected = rightArmPieces[nextIndex];
        } 
        else if (direction == 1)
        {
            // Select the previous right arm piece in the list, wrapping to the end if at the start
            int prevIndex = (currentIndex - 1 + rightArmPieces.Count) % rightArmPieces.Count;
            rightArmSelected = rightArmPieces[prevIndex];
        }


        Debug.Log("Right arm piece selected: " + rightArmSelected);


        if (rightArmSelected == 1)
        {
            basicRightArm.SetActive(false);
            blasterRightArm.SetActive(true);
            hyperBlasterRightArm.SetActive(false);
            PlayerState.canShoot = true;
            PlayerState.gunNumber = 0;
            rightText.text = "Blaster";
        } 
        else if (rightArmSelected == 2)
        {
            basicRightArm.SetActive(false);
            blasterRightArm.SetActive(false);
            hyperBlasterRightArm.SetActive(true);
            PlayerState.canShoot = true;
            PlayerState.gunNumber = 1;
            rightText.text = "Hyper Blaster";
        }
        else
        {
            basicRightArm.SetActive(true);
            blasterRightArm.SetActive(false);
            hyperBlasterRightArm.SetActive(false);
            PlayerState.canShoot = false;
            rightText.text = "-";
        }
    }

    public void toggleLeftArm(bool increment)
    {
        // Find the index of the currently selected left arm piece
        int currentIndex = leftArmPieces.IndexOf(leftArmSelected);

        if (increment)
        {
            // Select the next left arm piece in the list, wrapping to the start if at the end
            int nextIndex = (currentIndex + 1) % leftArmPieces.Count;
            leftArmSelected = leftArmPieces[nextIndex];
        }

        Debug.Log("Left arm piece selected: " + leftArmSelected);


        

        if (leftArmSelected == 1)
        {
            basicLeftArm.SetActive(false);
            torchLeftArm.SetActive(true);
            torch.SetActive(true);

            PlayerState.torchOn = true;
            leftText.text = "Torch";

        } else
        {
            basicLeftArm.SetActive(true);
            torchLeftArm.SetActive(false);
            torch.SetActive(false);

            PlayerState.torchOn = false;
            leftText.text = "-";
        }
    }

    public void OnSwitchToPowerArmor()
    {
        if (damageCoroutine == null)
        {
            damageCoroutine = StartCoroutine(DamagePlayerOverTime(1));
        }
    }

    // Call this when switching away from power armor
    public void OnSwitchAwayFromPowerArmor()
    {
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }
    }

    private IEnumerator DamagePlayerOverTime(int damageAmount)
    {
        while (PlayerState.powerArmor)
        {
            Debug.Log("energon drain 1 health");
            // Apply damage to the player
            PlayerState.Damage(damageAmount);

            // Wait for 1(?) second
            yield return new WaitForSeconds(1.3f);
        }

        damageCoroutine = null;
    }

    private IEnumerator HealPlayerOverTime(int healAmount)
    {
        while (PlayerState.heavyArmor)
        {
            Debug.Log("heavy armor add 1 health");
            // Apply healing to the player
            PlayerState.AddPower(healAmount);

            // Wait for 1(?) second
            yield return new WaitForSeconds(2f);
        }
        healCoroutine = null;
    }
    public void OnSwitchToHeavyArmor()
    {
        if (healCoroutine == null)
        {
            healCoroutine = StartCoroutine(HealPlayerOverTime(1));
        }
    }

    // Call this when switching away from heavy armor
    public void OnSwitchAwayFromHeavyArmor()
    {
        if (healCoroutine != null)
        {
            StopCoroutine(healCoroutine);
            healCoroutine = null;
        }
    }


}
