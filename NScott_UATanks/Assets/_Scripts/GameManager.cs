﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]

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

    [Header("General Game Settings")]
    [Tooltip("Determines if player tanks can damage each other.")]
    public bool friendlyFire = false;
    [Tooltip("Determines if enemy tanks can damage each other")]
    public bool enemyFriendlyFire = false;

    [Header("General Tank Settings")]
    [Tooltip("The speed at which tanks can move in reverse, represented as a percentage " +
        "of the tank's base movement speed.")]
    [Range(0.1f, 1f)] public float reverseSpeedRate = 0.75f;

    [Header("Player Tank Prefabs")]
    public GameObject playerPrefab;

    [Header("Player Tank Settings")]
    [Tooltip("The amount of HP a player tank starts with.")]
    [Range(1, 5)] public int playerStartingHP = 3;
    [Tooltip("The maximum amount of HP a player can earn after upgrades during a game.")]
    [Range(5, 15)] public int playerHPCap = 10;
    [Tooltip("The starting movement speed of player tanks.")]
    [Range(5f, 10f)] public float playerStartingMoveSpeed = 8f;
    [Tooltip("The starting turning speed of player tanks.")]
    [Range(20f, 90f)] public float playerStartingTurnSpeed = 45f;
    [Tooltip("The starting damage of bullets fired by the player.")]
    [Range(1, 3)] public int playerStartingBulletDamage = 1;
    [Tooltip("The starting movement speed of bullets fired by the player.")]
    [Range(2.5f, 5f)] public float playerStartingBulletSpeed = 5f;
    [Tooltip("The starting length of the player's firing cooldown.")]
    [Range(1f, 3f)] public float playerStartingFiringCooldown = 1.5f;

    [Header("Enemy Tank Prefabs")]
    public GameObject standardEnemyPrefab;
    public GameObject cowardEnemyPrefab;
    public GameObject reaperEnemyPrefab;
    public GameObject captainEnemyPrefab;

    [Header("Enemy Tank Settings")]
    [Tooltip("The amount of HP an enemy tank starts with.")]
    [Range(1, 5)] public int enemyStartingHP = 3;
    [Tooltip("The maximum amount of HP an enemy can earn after upgrades during a game.")]
    [Range(5, 15)] public int enemyHPCap = 15;
    [Tooltip("The starting movement speed of enemy tanks.")]
    [Range(5f, 10f)] public float enemyStartingMoveSpeed = 8f;
    [Tooltip("The starting turning speed of enemy tanks.")]
    [Range(20f, 90f)] public float enemyStartingTurnSpeed = 45f;
    [Tooltip("The starting damage of bullets fired by the enemy.")]
    [Range(1, 3)] public int enemyStartingBulletDamage = 1;
    [Tooltip("The starting movement speed of bullets fired by the enemy.")]
    [Range(2.5f, 5f)] public float enemyStartingBulletSpeed = 5f;
    [Tooltip("The starting length of the enemy's firing cooldown.")]
    [Range(1f, 3f)] public float enemyStartingFiringCooldown = 1.5f;
    [Tooltip("The distance at which an enemy tank considers itself \"close enough\" to a waypoint.")]
    [Range(1f, 10f)] public float waypointCloseEnoughDistance = 2f;
    [Tooltip("The distance at which an enemy tank considers itself \"close enough\" to a target tank.")]
    [Range(1f, 25f)] public float tankCloseEnoughDistance = 15f;
    [Tooltip("The maximum length of time an enemy tank will pursue a player without seeing them, before " +
        "returning to their default behavior.\nNOTE: Reaper tanks will ignore this value!")]
    [Range(1f, 10f)] public float maxPursuitTime = 3f;

    [Header("Bullet Settings")]
    [Tooltip("The bullet prefab object instantiated by a tank when it fires.")]
    public GameObject bulletPrefab;
    [Tooltip("The length of time a bullet should exist in the game world before destroying itself.")]
    [Range(1f, 5f)] public float bulletLifeSpan = 5f;
    [Tooltip("Determines whether or not bullets should destroy each other when they collide.")]
    public bool bulletCollisions = false;

    [Header("Input Controllers")]
    public GameObject p1InputController;
    public GameObject p2InputController;

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

    [Header("Layers & LayerMasks")]
    [Tooltip("The layer for tank objects.")]
    public LayerMask tankLayer;

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

    [Header("Spawn Point Lists")]
    [Tooltip("The list of player spawn points active in the game.")]
    public List<Transform> playerSpawnPoints;
    [Tooltip("The list of enemy spawn points active in the game.")]
    public List<Transform> enemySpawnPoints;
    [Tooltip("The list of power-up spawn points active in the game.")]
    public List<Transform> powerupSpawnPoints;

    [Header("Game Status")]
    [Tooltip("Player 1's score.")]
    public int p1Score = 0;
    [Tooltip("Player 2's score.")]
    public int p2Score = 0;
    [Space]
    [Tooltip("The number of players to spawn at game start.")]
    //[HideInInspector] 
    public int initialNumberOfPlayers = 1;

    [Header("Point Values")]
    [Tooltip("The base value an enemy tank is worth when it's destroyed.")]
    [Range(10,10000)] public int baseTankValue = 100;
    [Tooltip("For every hit point over the starting value, an enemy tank will be worth " +
        "this percentage more points. (Example: if this value is set to 1.5, an enemy tank " +
        "with one more hit point than the starting value would be worth <Base Value> + " +
        "(<Bonus Value> X <Base Value>) X <Number of HP over the starting value>).)")]
    [Range(0f, 3f)] public float bonusModifer = 0.5f;

    [Header("Tank Materials")]
    public Material playerBody;
    public Material playerCannon;
    public Material enemyStandardBody;
    public Material enemyStandardCannon;
    public Material enemyCowardBody;
    public Material enemyCowardCannon;
    public Material enemyReaperBody;
    public Material enemyReaperCannon;
    public Material enemyCaptainBody;
    public Material enemyCaptainCannon;

    /* Private Variables */

    private void Awake()
    {
        // Singleton pattern for game manager preservation
        if (gm == null)
        {
            gm = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        for (int index = 1; index <= initialNumberOfPlayers; index++)
        {
            if (index > 2)
            {
                break;
            }

            SpawnNewPlayer();

            switch (index)
            {
                case 1:
                    p1InputController.SetActive(true);
                    break;
                case 2:
                    p2InputController.SetActive(true);
                    break;
            }
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
            p1Score = players[0].tankScorer.score;
        }
        catch { }

        try
        {
            p2Score = players[1].tankScorer.score;
        }
        catch { }
    }

    private void SpawnNewPlayer()
    {
        int spawnIndex = UnityEngine.Random.Range(0, playerSpawnPoints.Count);
        Instantiate(playerPrefab, playerSpawnPoints[spawnIndex].position, playerSpawnPoints[spawnIndex].rotation, playerSpawnPoints[spawnIndex].GetComponent<RegisterSpawnPoint>().roomData.roomTf);
    }
}