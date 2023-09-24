using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

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
    public bool startled = false;      //enable AI awareness of player when hit
    public string orbReferenceName = "EnergyOrb";
    private Light agentLight;


    // Start is called before the first frame update
    void Start()
    {
        currentEnergy = currentEnergy + (LevelState.currentDifficulty * 5);
        agentLight = GetComponent<Light>();
        agentLight.enabled = false;
    }

    public void Damage(int damage)
    {
        StartCoroutine(Startle());
        StartCoroutine(DamageLight());
        int newEnergy = currentEnergy - damage;
        if(newEnergy > 0)
        {
            currentEnergy = newEnergy;
        }
        else
        {
            Death();
        }
    }

    public void AddPower(int collected)
    {
        currentEnergy += collected;
    }

    void Death()
    {
        // Play a death sound? 
        // Instantiate a dead spider object?

        // Instantiate energy orb
        Instantiate(Resources.Load<GameObject>(orbReferenceName), transform.position, transform.rotation);
        LevelState.kills++;
        Destroy(this.gameObject);
    }

    // resets isHit on the next frame, so it can be detected by the behaviour scripts' update function
    public IEnumerator Startle()
    {
        startled = true;
        //Debug.Log("enemy startled)");
        yield return null; // wait for the next frame
        startled = false;
    }
    private IEnumerator DamageLight()
    {
        agentLight.enabled = true;
        yield return new WaitForSeconds(0.2f); // wait for the next frame
        agentLight.enabled = false;
    }
}
