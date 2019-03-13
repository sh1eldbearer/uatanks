using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]

/*
 * This script allows an enemy (AI-controlled) tank to "hear" a player tank.
 * 
 * It should be attached to any enemy tank object, and will automatically be 
 * attached if an AIController component is added to the tank first.
 */

public class AIHearing : MonoBehaviour
{
    /* Public Variables */


    /* Private Variables */
    private AIController controller;
    private TankData tankData;

    private void Awake()
    {
        // Component reference assignments
        controller = this.gameObject.GetComponent<AIController>();
        tankData = this.gameObject.GetComponent<TankData>();
    }

    // Use this for initialization
    private void Start ()
    {
		
	}

    /// <summary>
    /// Listens for the noises being made by enemy tanks
    /// </summary>
    /// <param name="tagName">The tag for which to check for valid targets</param>
    public void Listen (string tagName)
    {
        if (controller.targetTankData == null)
        {
            Collider[] soundSources = Physics.OverlapSphere(tankData.tankTf.position, controller.hearingRadius, GameManager.gm.tankLayer);
            Debug.Log(soundSources);

            foreach (Collider source in soundSources)
            {
                TankData otherTank = source.GetComponent<TankData>();
                if (otherTank != null)
                {
                    if (otherTank.tag != tagName)
                    {
                        continue;
                    }
                    else if (otherTank.isMakingNoise)
                    {
                        controller.SetTarget(otherTank.tankTf);
                        break;
                    }
                }
            }
        }
	}
}
