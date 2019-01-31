using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script handles the shooting behaviors for tank objects.
 * 
 * This script should be attached to a tank object, and will automatically be attached if 
 * a TankData component is added to the tank first.
 */

public class TankShooter : MonoBehaviour
{
    /* Public Variables */


/* Private Variables */
private TankData tankData; // The TankData component of this tank object

    private void Awake()
    {
        // Component reference assignments
        tankData = this.gameObject.GetComponent<TankData>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Increases the firing timer
        tankData.firingTimer -= Time.deltaTime;
    }

    public void FireBullet()
    {
        if (tankData.firingTimer <= 0f)
        {
            // Creates a bullet at the spawn point
            GameObject newBullet = Instantiate(GameManager.gm.bulletPrefab, tankData.bulletSpawn.position, tankData.bulletSpawn.rotation);
            // Sets this tank as the bullet's owner
            newBullet.GetComponent<BulletData>().bulletOwner = tankData;
            // Sets the bullet's tag to match the tag of the tank that fired it
            newBullet.tag = this.gameObject.tag;
            // Resets the shot timer
            tankData.firingTimer = tankData.firingTimeout;
        }
    }
}
