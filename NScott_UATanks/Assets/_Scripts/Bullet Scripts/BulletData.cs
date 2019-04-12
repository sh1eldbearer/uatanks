using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(BulletMover))]
[RequireComponent(typeof(BulletCollider))]
[RequireComponent(typeof(BulletTimer))]

/*
 * This script holds settings information about the bullet prefab object.
 * 
 * This script should be attached to the bullet prefab object.
 */

public class BulletData : MonoBehaviour
{
    /* Public Variables */
    [HideInInspector] public TankData bulletOwner; // The tank that fired this bullet (used for scoring)

    [HideInInspector] public float moveForce; // The amount of movement force to apply to this bullet
    [HideInInspector] public int bulletDamage; // The amount damage this bullet does when it collides with another tank
    [HideInInspector] public float bulletLifeSpan = 5f; // The length of time a bullet should exist before destroying itself

    [HideInInspector] public Transform bulletTf; // The Transform component of this bullet object 
    [HideInInspector] public Rigidbody bulletRb; // The Rigidbody component of this bullet object

    /* Private Variables */

    private void Awake()
    {
        // Component reference assignments
        bulletTf = this.gameObject.GetComponent<Transform>();
        bulletRb = this.gameObject.GetComponent<Rigidbody>();
    }

    // Use this for initialization
    private void Start()
    {
        moveForce = bulletOwner.currentBulletMoveSpeed;
        bulletDamage = bulletOwner.currentBulletDamage;
        bulletLifeSpan = GameManager.gm.bulletLifeSpan;
    }
}
