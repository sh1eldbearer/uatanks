using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script handles the lifespan timer of the bullet prefab object. If a bullet exists
 * in the game world longer than the amount of time set in BulletData, it automatically
 * destroys itself.
 * 
 * This script should be attached to the bullet prefab object.
 */

public class BulletTimer : MonoBehaviour
{
    /* Public Variables */

    /* Private Variables */
    private BulletData bulletData; // The BulletData component of this bullet object
    private BulletCollider bulletCollider; // The BulletCollider component of this bullet object

	private void Awake ()
    {
        // Component reference assignments
        bulletData = this.gameObject.GetComponent<BulletData>();
        bulletCollider = this.gameObject.GetComponent<BulletCollider>();
	}
	
	// Update is called once per frame
	private void Update ()
    {
        // Decreases this bullet's life space
        bulletData.bulletLifeSpan -= Time.deltaTime;

        // When the bullet reaches the end of its lifespan, destroy the bullet
        if (bulletData.bulletLifeSpan <= 0)
        {
            bulletCollider.Die();
        }
	}
}
