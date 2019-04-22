using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]

/*
 * This script handles the collision behavior for the bullet prefab object.
 * 
 * This script should be attached to the bullet prefab object.
 */

public class BulletCollider : MonoBehaviour
{
    /* Public Variables */

    /* Private Variables */
    private BulletData bulletData; // The BulletData component of this bullet object

    // Use this for initialization
    private void Start ()
    {
        // Component reference assignments
        bulletData = this.gameObject.GetComponent<BulletData>();
    }

    private void OnCollisionEnter(Collision other)
    {
        TankData otherTankData = other.gameObject.GetComponent<TankData>();
        BulletCollider otherBulletCollider = other.gameObject.GetComponent<BulletCollider>();

        if (otherTankData == bulletData.bulletOwner)
        {
            Physics.IgnoreCollision(this.gameObject.GetComponent<Collider>(), other.collider);
            return;
        }
        // If this bullet hits a tank
        else if (otherTankData != null)
        {
            // If this bullet was fired by a player tank
            if (this.tag == "Player")
            {
                // Bullet collided with an enemy tank, or collided with a player tank and friendly fire is enabled
                if (other.gameObject.tag == "Enemy" ||
                    (other.gameObject.tag == "Player" && GameManager.gm.friendlyFire))
                {
                    // Damages the tank
                    int otherTankHP = otherTankData.tankHealthMan.Damage(bulletData.bulletDamage);

                    // If the other tank was destroyed by this shot
                    if (otherTankHP != -1)
                    {
                        bulletData.bulletOwner.tankScorer.IncreaseScore(otherTankHP);
                    }
                }
                else
                {
                    // Nothing to do but go die
                }
            }
            // If this bullet was fired by an enemy tank
            else if (this.tag == "Enemy")
            {
                // Bullet collided with a player tank, or collided with an enemy tank and enemy friendly fire is enabled
                if (other.gameObject.tag == "Player" ||
                    (other.gameObject.tag == "Enemy" && GameManager.gm.enemyFriendlyFire))
                {
                    // Damages the tank
                    otherTankData.tankHealthMan.Damage(bulletData.bulletDamage);
                }
                else
                {
                    // Nothing to do but go die
                }
            }
        }
        // Ignore bullet collisions
        else if (otherBulletCollider != null && !GameManager.gm.bulletCollisions)
        {
            Physics.IgnoreCollision(this.GetComponent<Collider>(), other.collider);
            //this.gameObject.GetComponent<Collider>().enabled = false;
            return;
        }
        else
        {
            // Do nothing else except go die
        }
        GameManager.soundMgr.PlayBulletHitSound();
        Die();
    }

    private void OnCollisionExit(Collision collision)
    {
        this.gameObject.GetComponent<Collider>().enabled = true;
    }

    /// <summary>
    /// Destroys this bullet object.
    /// </summary>
    public void Die()
    {
        // Destroys the bullet
        Destroy(this.gameObject);
    }
}
