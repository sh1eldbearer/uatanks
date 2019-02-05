using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(TankMover))]
[RequireComponent(typeof(TankShooter))]
[RequireComponent(typeof(TankHealthManager))]

/*
 * This script holds settings information about a tank object.
 * 
 * This script should be attached to the top-level GameObject of a tank object.
 */

public class TankData : MonoBehaviour
{
    /* Public Variables */
    [HideInInspector]public int playerNumber; // This player's player number (determined by its index in GameManager.players)
    
    [Header("Health")]
    [Tooltip("The current number of HP this tank has.")]
    [Range(0, 15)] public int currentHP;
    [Tooltip("The maximum number of HP this tank can have.")]
    [Range(0, 15)] public int maxHP = 3;

    [Header("Movement Settings")]
    [Tooltip("The base movement speed of this tank.")]
    public float moveSpeed = 1f;
    [Tooltip("The speed at which this tank can move in reverse, represented as a percentage " +
        "of the tank's base movement speed.")]
    [Range(0.1f,1f)] public float reverseSpeedRate = 0.75f;
    [Tooltip("The base rotation speed of the tank.")]
    public float rotateSpeed = 1f;


    [Header("Firing Settings")]
    [Tooltip("The base amount of damage a bullet fired by this tank does.")]
    public int bulletDamage = 1;
    [Tooltip("The base amount of force applied to bullets fired by this tank." +
        "NOTE: Tanks should never be able to catch up with the bullets they fire!")]
    [Range(4f, 15f)] public float bulletMoveForce = 500f;
    [Tooltip("The length of time a tank must wait after firing before being able to fire another shot.")]
    [Range(0f,2f)] public float firingTimeout = 1.5f;
    [HideInInspector] public float firingTimer; // A countdown since this tank last successfully fired a shot

    [Header("GameObject and Component References")]
    [HideInInspector] public Transform tankTf; // The transform component of this tank object
    [HideInInspector] public CharacterController tankCc; // The CharacterController component of this tank object
    [HideInInspector] public TankHealthManager tankHealthMan; // The TankHealthManager component of this tank object
    [HideInInspector] public TankScorer tankScorer; // The Scorer component of this tank object
    [HideInInspector] public Transform bulletSpawn; // The bullet spawn point attached to this tank object

    /* Private Variables */

    private void Awake()
    {
        // Component reference assignments
        tankTf = this.gameObject.GetComponent<Transform>();
        tankCc = this.gameObject.GetComponent<CharacterController>();
        tankHealthMan = this.gameObject.GetComponent<TankHealthManager>();
        tankScorer = this.gameObject.GetComponent<TankScorer>();
    }

    // Use this for initialization
    private void Start ()
    {
        // Displays an error in the inspector if the bullet spawn point is not set when the game starts
        if (bulletSpawn == null)
        {
            Debug.LogError("Bullet spawn point not set.");
        }

		// Adds this tank to the appropriate list of objects in the GameManager
        if (tankTf.tag == "Player")
        {
            // This is a player tank
            GameManager.gm.players.Add(this);
            playerNumber = GameManager.gm.players.IndexOf(this) + 1;
        }
        else if (tankTf.tag == "Enemy")
        {
            // This is an enemy tank
            GameManager.gm.enemies.Add(this);
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
        reverseSpeedRate = Mathf.Clamp(reverseSpeedRate, 0.1f, 1f);
        bulletMoveForce = Mathf.Clamp(bulletMoveForce, 4f, 15f);
        firingTimeout = Mathf.Clamp(firingTimeout, 0f, 2f);
    }
}
