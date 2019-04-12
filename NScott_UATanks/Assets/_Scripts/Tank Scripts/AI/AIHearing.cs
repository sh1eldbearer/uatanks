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

    /// <summary>
    /// Listens for the noises being made by enemy tanks
    /// </summary>
    /// <param name="tagName">The tag for which to check for valid targets</param>
    /// <returns>True if a target tank was heard; false if it was not.</returns>
    public bool Listen (string tagName)
    {
        // As long as the tank does not have a visible target
        if (controller.targetTankData == null)
        {
            // Look for all tank objects in a radius around the tank
            Collider[] soundSources = Physics.OverlapSphere(tankData.tankTf.position, controller.hearingRadius, GameManager.gm.tankLayer);

            foreach (Collider source in soundSources)
            {
                TankData otherTank = source.GetComponent<TankData>();
                // If the other object has a TankData component, see what the tank is tagged with
                if (otherTank != null) 
                {
                    // If the other tank is not tagged as the target type
                    if (otherTank.tag != tagName)
                    {
                        // Ignore it
                        continue;
                    }
                    // If it is the target type
                    else if (otherTank.isMakingNoise)
                    {
                        // The other tank is marked as heard and its position is stored
                        controller.canHearPlayer = true;
                        controller.targetPosition = otherTank.tankTf.position;
                        return true;
                    }
                }
            }
        }
        return false;
	}
}
