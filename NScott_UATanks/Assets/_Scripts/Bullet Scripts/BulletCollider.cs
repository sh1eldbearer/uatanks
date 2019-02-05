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
        // If this bullet hits another bullet, it gets destroyed
        if (other.gameObject.GetComponent<BulletData>() == true)
        {
            Die();
        }
        // If this bullet was fired by a player tank
        else if (this.tag == "Player")
        {
            // Bullet collided with an enemy tank, or collided with a player tank and friendly fire is enabled
            if (other.gameObject.tag == "Enemy" ||
                (other.gameObject.tag == "Player" && GameManager.gm.friendlyFire))
            {
                // Damages the tank
                int deadTankHP = other.collider.GetComponent<TankHealthManager>().Damage(bulletData.bulletDamage);

                //if the other tank was destroyed by this shot
                if (deadTankHP != -1)
                {
                    bulletData.bulletOwner.tankScorer.IncreaseScore(deadTankHP);
                }
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
                int deadTankHP = other.collider.GetComponent<TankHealthManager>().Damage(bulletData.bulletDamage);

                //if the other tank was destroyed by this shot
                if (deadTankHP != -1)
                {
                    bulletData.bulletOwner.tankScorer.IncreaseScore(deadTankHP);
                }
            }
        }

        Die();
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
