using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * GameManager used for configuring game settings by designers, and for disseminating that information
 * to the rest of the game at runtime.
 * 
 * This should be placed as a component on an empty game object within the game scene.
 */

public class GameManager : MonoBehaviour
{
    /* Public Variables */
    [HideInInspector] public static GameManager gm; // Reference to a GameManager object for singleton pattern

    [Header("Game Settings")]
    [Tooltip("Determines if player tanks can damage each other.")]
    public bool friendlyFire = false;
    [Tooltip("Determines if enemy tanks can damage each other")]
    public bool enemyFriendlyFire = false;

    [Header("Player Tank Settings")]
    [Tooltip("The amount of HP a player tank starts with.")]
    [Range(1, 5)] public int playerStartingHP = 3;
    [Tooltip("The maximum amount of HP a player can earn after upgrades during a game.")]
    [Range(5, 15)] public int playerHPCap = 10;

    [Header("Enemy Tank Settings")]
    [Tooltip("The amount of HP an enemy tank starts with.")]
    [Range(1, 5)] public int enemyStartingHP = 3;
    [Tooltip("The maximum amount of HP an enemy can earn after upgrades during a game.")]
    [Range(5, 15)] public int enemyHPCap = 15;

    [Header("Bullet Settings")]
    [Tooltip("The bullet prefab object instantiated by a tank when it fires.")]
    public GameObject bulletPrefab;
    [Tooltip("The length of time a bullet should exist in the game world before destroying itself.")]
    [Range(1f, 5f)] public float bulletLifeSpan = 5f;

    [Header("Game Cameras & Camera Settings")]
    [Tooltip("The camera used to render player 1's display")]
    public Camera player1Camera;
    [Tooltip("Determines if player 1's camera can rotate with the tank at it moves.")]
    public bool p1RotateCamera = false;
    [Space]
    [Tooltip("The camera used to render player 2's display")]
    public Camera player2Camera;
    [Tooltip("Determines if player 2's camera can rotate with the tank as it moves.")]
    public bool p2RotateCamera = false;

    [Header("UI Settings")]
    [Tooltip("The length of time it takes for any cooldown bars to fade out when " +
        "their countdowns have completed.")]
    [Range(0.1f, 2f)] public float barFadeTime = 0.25f;

    [Header("Tank Lists")]
    [Tooltip("The list of currently active player tanks. The game is over if there are " +
        "no active players remaining in the game.")]
    public List<TankData> players;
    [Tooltip("The list of currently active enemy tanks.")]
    public List<TankData> enemies;

    [Header("Game Status")]
    [Tooltip("Player 1's score.")]
    public int p1Score = 0;
    [Tooltip("Player 2's score.")]
    public int p2Score = 0;
    [Space]
    [Header("Point Values")]
    [Tooltip("The base value an enemy tank is worth when it's destroyed.")]
    [Range(10,10000)] public int baseTankValue = 100;
    [Tooltip("For every hit point over the starting value, an enemy tank will be worth " +
        "this percentage more points. (Example: if this value is set to 1.5, an enemy tank " +
        "with one more hit point than the starting value would be worth <Base Value> + " +
        "(<Bonus Value> X <Base Value>) X <Number of HP over the starting value>).)")]
    [Range(0f, 3f)] public float bonusModifer = 0.5f;
    // TODO: Increased values for enemy tanks of different personality types?

    /* Private Variables */

    private void Awake()
    {
        // Singleton pattern for game manager preservation
        if (gm == null)
        {
            gm = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // Ensures certain values can't be accidentally set too high or low in the inspector
        playerStartingHP = Mathf.Clamp(playerStartingHP, 1, 15);
        playerHPCap = Mathf.Clamp(playerHPCap, 5, 15);
        enemyStartingHP = Mathf.Clamp(enemyStartingHP, 1, 15);
        enemyHPCap = Mathf.Clamp(enemyHPCap, 5, 15);
        barFadeTime = Mathf.Clamp(barFadeTime, 0.1f, 2f);
        baseTankValue = Mathf.Clamp(baseTankValue, 10, 10000);
        bonusModifer = Mathf.Clamp(bonusModifer, 0f, 3f);

        // Updates player's scores
        try
        {
            p1Score = players[0].score;
        }
        catch { }

        try
        {
            p2Score = players[1].score;
        }
        catch { }
    }
}