using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    /* Public Variables */
    public PowerupScriptableObject powerupProperties;
    [HideInInspector] public TankData attachedTank;
    public bool ignoreLifespan = false;

    /* Private Variables */
    private float lifeSpan;
    private Transform powerupTf;
    private Collider powerupCollider;
    private MeshRenderer powerupRenderer;
    private bool pickedUp = false;

    // Use this for initialization
    void Start ()
    {
        lifeSpan = GameManager.gm.powerupLifespan;
        powerupTf = this.gameObject.GetComponent<Transform>();
        powerupCollider = this.gameObject.GetComponent<Collider>();
        powerupRenderer = this.gameObject.GetComponent<MeshRenderer>();
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (pickedUp == false)
        {
            if (!ignoreLifespan)
            {
                // If this object exists in the game world too long, it will despawn
                lifeSpan -= Time.deltaTime;
                if (lifeSpan <= 0)
                {
                    Destroy(this.gameObject);
                }
            }
        }
        else
        {
            // If this object was picked up, it runs for the duration set in the scriptable object, then destroys itself
            lifeSpan -= Time.deltaTime;
            if (lifeSpan <= 0)
            {
                RemoveEffect();
            }
        }
	}

    public void ApplyEffect(TankData targetTank)
    {
        pickedUp = true;
        attachedTank = targetTank;
        powerupTf.parent = targetTank.GetComponent<Transform>();
        powerupCollider.enabled = false;
        powerupRenderer.enabled = false;
        lifeSpan = powerupProperties.duration;
        EffectMath(ref targetTank, true);
        if (powerupProperties.permanance == PowerupPermanence.Permanent)
        {
            Destroy(this.gameObject);
        }
    }

    public void RemoveEffect()
    {
        if (powerupProperties.permanance == PowerupPermanence.Temporary)
        {
            EffectMath(ref attachedTank, false);
            Destroy(this.gameObject);
        }
    }

    private void EffectMath(ref TankData tankData, bool addEffect)
    {
        int addRemoveModifier = 1;
        if (!addEffect)
        {
            addRemoveModifier *= -1;
        }

        switch (powerupProperties.statToAffect)
        {
            case PowerupStat.IncreaseCurrentHP:
                tankData.currentHP += (int)(addRemoveModifier * powerupProperties.valueAdjust);
                break;
            case PowerupStat.IncreaseMaxHP:
                tankData.maxHP += (int)(addRemoveModifier * powerupProperties.valueAdjust);
                tankData.currentHP += (int)(addRemoveModifier * powerupProperties.valueAdjust);
                break;
            case PowerupStat.IncreaseMoveSpeed:
                tankData.currentMoveSpeed += addRemoveModifier * powerupProperties.valueAdjust;
                break;
            case PowerupStat.IncreaseTurnSpeed:
                tankData.currentTurnSpeed += addRemoveModifier * powerupProperties.valueAdjust;
                break;
            case PowerupStat.IncreaseBulletDamage:
                tankData.currentBulletDamage += (int)(addRemoveModifier * powerupProperties.valueAdjust);
                break;
            case PowerupStat.IncreaseBulletSpeed:
                tankData.currentBulletMoveSpeed += addRemoveModifier * powerupProperties.valueAdjust;
                break;
            case PowerupStat.DecreaseFiringCooldown:
                tankData.currentFiringCooldown += addRemoveModifier * powerupProperties.valueAdjust;
                tankData.overrideFiringCooldown = addEffect;
                break;
        }
    }
}
