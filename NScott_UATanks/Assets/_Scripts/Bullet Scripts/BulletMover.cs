using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]

/*
 * This script handles the movement for the bullet prefab object.
 * 
 * This script should be attached to the bullet prefab object.
 */

public class BulletMover : MonoBehaviour
{
    /* Public Variables */


    /* Private Variables */
    private BulletData bulletData; // The BulletData component of this bullet object

    void Awake()
    {
        // Component reference assignments
        bulletData = this.gameObject.GetComponent <BulletData>();
    }

    // Use this for initialization
    private void Start()
    {
        // Applies the bullet's movement speed
        bulletData.bulletRb.AddForce(bulletData.bulletTf.forward * bulletData.moveForce * 100f);
    }
}
