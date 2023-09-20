using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
 * Author: Josh Wilson
 * 
 * Description:
 *  - A simple script for setting guard health according to difficulty, decrementing when hit, and handling death
 *  
 */

public class EnemyHealth : MonoBehaviour
{

    // Enemy Health
    public int currentEnergy = 15;


    // Start is called before the first frame update
    void Start()
    {
        currentEnergy = currentEnergy + (LevelState.currentDifficulty * 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void PlayerHit(int damage)
    {
        int newEnergy = currentEnergy - damage;
        if(newEnergy > 0)
        {
            currentEnergy = newEnergy;
        }
        else
        {
            Dead();
        }
    }

    void Dead()
    {
        // Play a death sound? 
        // Instantiate a dead spider object in it's place?
        Destroy(this.gameObject);
    }
}
