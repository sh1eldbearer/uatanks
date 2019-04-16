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
        // If this bullet hits a tank
        if (other.gameObject.GetComponent<TankData>() != null)
        {
            // If this bullet was fired by a player tank
            if (this.tag == "Player")
            {
                // Bullet collided with an enemy tank, or collided with a player tank and friendly fire is enabled
                if (other.gameObject.tag == "Enemy" ||
                    (other.gameObject.tag == "Player" && GameManager.gm.friendlyFire))
                {
                    // Damages the tank
                    int otherTankHP = other.collider.GetComponent<TankHealthManager>().Damage(bulletData.bulletDamage);

                    // If the other tank was destroyed by this shot
                    if (otherTankHP != -1)
                    {
                        bulletData.bulletOwner.tankScorer.IncreaseScore(otherTankHP);
                    }
                }
                else
                {
                    return;
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
                    other.collider.GetComponent<TankHealthManager>().Damage(bulletData.bulletDamage);
                }
                else
                {
                    return;
                }
            }
        }
        // Ignore bullet collisions
        else if (other.gameObject.GetComponent<BulletCollider>() != null && !GameManager.gm.bulletCollisions)
        {
            Physics.IgnoreCollision(this.GetComponent<Collider>(), other.collider);
            //this.gameObject.GetComponent<Collider>().enabled = false;
            return;
        }
        else
        {
            // Do nothing else except go die
        }

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
