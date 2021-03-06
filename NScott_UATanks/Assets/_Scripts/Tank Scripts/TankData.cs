﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(TankMover))]
[RequireComponent(typeof(TankShooter))]
[RequireComponent(typeof(TankHealthManager))]
[RequireComponent(typeof(TankNoisemaker))]

/*
 * This script holds settings information about a tank object.
 * 
 * This script should be attached to the top-level GameObject of a tank object.
 */

public class TankData : MonoBehaviour
{
    /* Public Variables */
    [HideInInspector]public int playerNumber; // This player's player number (determined by its index in GameManager.players)
    [Space]
    public GameObject tankVisuals;
    public GameObject tankCanvas;
    public Camera tankCamera;

    [Header("Health")]
    [Tooltip("The number of lives this tank currently has.")]
    public int currentLives;
    [Tooltip("The current number of HP this tank has.")]
    [Range(0, 15)] public int currentHP;
    [Tooltip("The maximum number of HP this tank can have.")]
    [Range(0, 15)] public int maxHP = 3;
    [Tooltip("Enable tank damage")]
    public bool takesDamage = true;

    [Header("Movement Settings")]
    [Tooltip("The movement speed of this tank.")]
    public float currentMoveSpeed = 8f;
    [Tooltip("The rotation speed of the tank.")]
    public float currentTurnSpeed = 45f;

    [Header("Firing Settings")]
    [Tooltip("The base amount of damage a bullet fired by this tank does.")]
    public int currentBulletDamage = 1;
    [Tooltip("The base amount of force applied to bullets fired by this tank." +
        "NOTE: Tanks should never be able to catch up with the bullets they fire!")]
    [Range(4f, 15f)] public float currentBulletMoveSpeed = 5f;
    [Tooltip("The length of time a tank must wait after firing before being able to fire another shot.")]
    [Range(0f, 2f)] public float currentFiringCooldown = 1.5f;
    public bool overrideFiringCooldown = false;
    [HideInInspector] public float firingTimer; // A countdown since this tank last successfully fired a shot

    [Header("Sound Settings")]
    public bool isMakingNoise = false;
    [Tooltip("The time it takes for the sound of this tank firing to fade away.")]
    [Range(0.1f, 1f)] public float noiseDecayTime = 0.25f;

    [Header("GameObject and Component References")]
    [HideInInspector] public Transform tankTf; // The transform component of this tank object
    [HideInInspector] public TankMover tankMover; // The TankMover component of this tank object
    [HideInInspector] public TankShooter tankShooter; // The TankShooter component of this tank object
    [HideInInspector] public CharacterController tankCc; // The CharacterController component of this tank object
    [HideInInspector] public TankHealthManager tankHealthMan; // The TankHealthManager component of this tank object
    [HideInInspector] public TankScorer tankScorer; // The Scorer component of this tank object
    [HideInInspector] public TankNoisemaker tankNoisemaker; // The TankNoisemaker component of this tank object
    public Transform bulletSpawn; // The bullet spawn point attached to this tank object
    // The Transform components of the GameObjects used as origin points for obstacle avoidance raycasting
    [HideInInspector] public Transform originRayCastTf;
    [HideInInspector] public Transform leftRaycastTf;
    [HideInInspector] public Transform rightRaycastTf;

    /* Private Variables */


    private void Awake()
    {
        // Component reference assignments
        tankTf = this.gameObject.GetComponent<Transform>();
        tankMover = this.gameObject.GetComponent<TankMover>();
        tankShooter = this.gameObject.GetComponent<TankShooter>();
        tankCc = this.gameObject.GetComponent<CharacterController>();
        tankHealthMan = this.gameObject.GetComponent<TankHealthManager>();
        tankScorer = this.gameObject.GetComponent<TankScorer>();
        tankNoisemaker = this.gameObject.GetComponent<TankNoisemaker>();
    }

    // Use this for initialization
    private void Start ()
    {
        // Displays an error in the inspector if the bullet spawn point is not set when the game starts
        if (bulletSpawn == null)
        {
            Debug.LogError("Bullet spawn point not set.");
        }

		// Adds this tank to the appropriate list of objects in the GameManager and initialize settings
        if (tankTf.tag == "Player")
        {
            // This is a player tank
            playerNumber = GameManager.gm.players.IndexOf(this) + 1;
            if (this.gameObject.GetComponentInChildren<Camera>() != null)
            {
                switch (playerNumber)
                {
                    case 1:
                        GameManager.gm.player1Camera = this.gameObject.GetComponentInChildren<Camera>();
                        break;
                    case 2:
                        GameManager.gm.player2Camera = this.gameObject.GetComponentInChildren<Camera>();
                        break;
                }
            }
            currentHP = GameManager.gm.playerStartingHP;
            maxHP = currentHP;
            currentLives = GameManager.gm.playerStartingLives;
            currentMoveSpeed = GameManager.gm.playerStartingMoveSpeed;
            currentTurnSpeed = GameManager.gm.playerStartingTurnSpeed;
            currentBulletDamage = GameManager.gm.playerStartingBulletDamage;
            currentBulletMoveSpeed = GameManager.gm.playerStartingMoveSpeed;
            currentFiringCooldown = GameManager.gm.playerStartingFiringCooldown;
        }
        else if (tankTf.tag == "Enemy")
        {
            // This is an enemy tank
            GameManager.gm.enemies.Add(this);
            currentHP = GameManager.gm.enemyStartingHP;
            maxHP = currentHP;
            currentLives = 1;
            currentMoveSpeed = GameManager.gm.enemyStartingMoveSpeed;
            currentTurnSpeed = GameManager.gm.enemyStartingTurnSpeed;
            currentBulletDamage = GameManager.gm.enemyStartingBulletDamage;
            currentBulletMoveSpeed = GameManager.gm.enemyStartingMoveSpeed;
            currentFiringCooldown = GameManager.gm.enemyStartingFiringCooldown;
        }
        else
        {
            // This tank didn't have its tag set, so I don't know what it is
            Debug.LogError("Tank object " + tankTf.name + " was not properly tagged, and could not be added to a TankData list.");
        }

        // Makes sure the firing timer is zeroed out to start the game, allowing the player to immediately fire
        firingTimer = 0f;
	}

    // Update is called once per frame
    private void Update()
    {
        // Ensures certain values can't be accidentally set too high or low in the inspector
        if (this.tag == "Player")
        {
            // Player settings
            currentHP = Mathf.Clamp(currentHP, Constants.MIN_HP, maxHP);
            maxHP = Mathf.Clamp(maxHP, currentHP, GameManager.gm.playerHPCap);
            currentMoveSpeed = Mathf.Clamp(currentMoveSpeed, Constants.MIN_MOVE_SPEED, GameManager.gm.playerMoveSpeedCap);
            currentTurnSpeed = Mathf.Clamp(currentTurnSpeed, Constants.MIN_TURN_SPEED, GameManager.gm.playerTurnSpeedCap);
            currentBulletDamage = Mathf.Clamp(currentBulletDamage, Constants.MIN_BULLET_DAMAGE, GameManager.gm.playerBulletDamageCap);
            currentBulletMoveSpeed = Mathf.Clamp(currentBulletMoveSpeed, Constants.MIN_BULLET_SPEED, GameManager.gm.playerBulletSpeedCap);
            if (overrideFiringCooldown)
            {
                currentFiringCooldown = Mathf.Clamp(currentFiringCooldown, 0.1f, Constants.MAX_FIRING_COOLDOWN);
            }
            else
            {
                currentFiringCooldown = Mathf.Clamp(currentFiringCooldown, Constants.MIN_FIRING_COOLDOWN, Constants.MAX_FIRING_COOLDOWN);
            }
        }
        else
        {
            // Enemy settings
            currentHP = Mathf.Clamp(currentHP, Constants.MIN_HP, maxHP);
            maxHP = Mathf.Clamp(maxHP, currentHP, GameManager.gm.enemyHPCap);
            currentMoveSpeed = Mathf.Clamp(currentMoveSpeed, Constants.MIN_MOVE_SPEED, GameManager.gm.enemyMoveSpeedCap);
            currentTurnSpeed = Mathf.Clamp(currentTurnSpeed, Constants.MIN_TURN_SPEED, GameManager.gm.enemyTurnSpeedCap);
            currentBulletDamage = Mathf.Clamp(currentBulletDamage, Constants.MIN_BULLET_DAMAGE, GameManager.gm.enemyBulletDamageCap);
            currentBulletMoveSpeed = Mathf.Clamp(currentBulletMoveSpeed, Constants.MIN_BULLET_SPEED, GameManager.gm.enemyBulletSpeedCap);
            if (!overrideFiringCooldown)
            {
                currentFiringCooldown = Mathf.Clamp(currentFiringCooldown, 0.1f, Constants.MAX_FIRING_COOLDOWN);
            }
            else
            {
                currentFiringCooldown = Mathf.Clamp(currentFiringCooldown, Constants.MIN_FIRING_COOLDOWN, Constants.MAX_FIRING_COOLDOWN);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Powerup")
        {
            GameManager.soundMgr.PlayPickupSound();
            other.GetComponent<Powerup>().ApplyEffect(this);
        }
    }

    public void CopyTankData(TankData otherTankData)
    {
        playerNumber = otherTankData.playerNumber;
        tankCamera = otherTankData.tankCamera;
        currentLives = otherTankData.currentLives;
        maxHP = otherTankData.maxHP;
        currentHP = otherTankData.currentHP;
        currentTurnSpeed = otherTankData.currentTurnSpeed;
        currentMoveSpeed = otherTankData.currentMoveSpeed;
        currentBulletDamage = otherTankData.currentBulletDamage;
        currentBulletMoveSpeed = otherTankData.currentBulletMoveSpeed;
    }
}
