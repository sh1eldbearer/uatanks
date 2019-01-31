using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script adjusts the score accumulated by a player tank.
 * It should only be attached to a player tank object - never to an enemy tank!
 */

public class TankScorer : MonoBehaviour
{
    /* Public Variables */


    /* Private Variables */
    private TankData tankData; // Used to get the player number of this tank object

    private void Awake()
    {
        // Component reference assignments
        tankData = this.gameObject.GetComponent<TankData>();
    }

    /// <summary>
    /// Increases a player's score whenever they destroy an enemy tank
    /// </summary>
    /// <param name="deadTankHP">The max HP value of the tank that was destroyed.</param>
    public void IncreaseScore(int deadTankHP)
    {
        // Gets the base tank value
        int scoreValue = GameManager.gm.baseTankValue;

        // If the enemy tank had more max health than the starting value, it's worth more points
        if (deadTankHP > GameManager.gm.enemyStartingHP)
        {
            scoreValue += (int)((GameManager.gm.baseTankValue * GameManager.gm.bonusModifer) * (deadTankHP - GameManager.gm.enemyStartingHP));
        }

        // Determines which player's score to add the point value to
        if (tankData.playerNumber == 1)
        {
            GameManager.gm.p1Score += scoreValue;
        }
        else if (tankData.playerNumber == 2)
        {
            GameManager.gm.p2Score += scoreValue;
        }
    }
}
