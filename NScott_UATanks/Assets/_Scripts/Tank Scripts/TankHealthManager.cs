﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script monitors the health of a tank object, and adjusts values as needed.
 * 
 * This script should be attached to a tank object, and will automatically be attached if 
 * a TankData component is added to the tank first.
 */

public class TankHealthManager : MonoBehaviour
{
    /* Public Variables */


    /* Private Variables */
    private TankData tankData; // Used to access the tank's current and maximum health values

    void Awake()
    {
        // Component reference assembly
        tankData = this.gameObject.GetComponent<TankData>();
    }

    // Use this for initialization
    void Start ()
    {
        // Sets the tank's max HP to the starting value in the GameManager
        tankData.maxHP = GameManager.gm.playerStartingHP;
        // Sets the tank's starting HP to its max HP
        tankData.currentHP = tankData.maxHP;
    }

    void Update()
    {
        // If this tank's health has reached or exceeded zero
        if (tankData.currentHP <= 0)
        {
            // Destroys this tank
            Die();
        }

        // Makes sure the tank's maximum health cnanot exceed the game-defined maximum
        tankData.maxHP = Mathf.Clamp(tankData.maxHP, GameManager.gm.playerStartingHP, GameManager.gm.playerHPCap);
        // Makes sure the tank's current health cannot exceed its maximum health
        tankData.currentHP = Mathf.Clamp(tankData.currentHP, -1, tankData.maxHP);
    }

    /// <summary>
    /// Adjusts the current hit point value of the tank object.
    /// </summary>
    /// <param name="changeValue">The amount to adjust the current hit points by. 
    /// Enter a negative number to increase the current health value, 
    /// and a positive number to reduce the current health value.</param>
    /// <returns>Returns a positive value if the tank was destroyed by the shot 
    /// (Used for adjusting scores).</returns>
    public int Damage(int changeValue)
    {
        tankData.currentHP -= changeValue;

        // If this tank's health has reached or exceeded zero
        if (tankData.currentHP <= 0)
        {
            // Informs the calling function that the tank was destroyed
            return tankData.maxHP;
        }
        else
        {
            // Informs the calling function that the tank was not destroyed
            return -1;
        }
    }

    /// <summary>
    /// Adjusts the current hit point value of the tank object.
    /// </summary>
    /// <param name="changeValue">The amount to adjust the current hit points by. 
    /// Enter a positive number to increase the current health value, 
    /// and a negative number to reduce the current health value.</param>
    public void Heal(int changeValue)
    {
        tankData.currentHP += changeValue;
    }

    /// <summary>
    /// Destroys this tank, and removes it from the appropriate TankData list 
    /// in the GameManager
    /// </summary>
    public void Die()
    {
        // Looks at the tag of this tank, and removes this tank from the appropriate TankData list
        if (tankData.tankTf.tag == "Player")
        {
            // Tank is a player
            GameManager.gm.players.Remove(tankData);
        }
        else if (tankData.tankTf.tag == "Enemy")
        {
            // Tank is an enemy
            GameManager.gm.enemies.Remove(tankData);
        }

        // Kill the tank!
        Destroy(this.gameObject);
    }
}