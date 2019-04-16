using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerupPermanence
{
    Temporary,
    Permanent
}

public enum PowerupStat
{
    IncreaseCurrentHP,
    IncreaseMaxHP,
    IncreaseMoveSpeed,
    IncreaseTurnSpeed,
    IncreaseBulletDamage,
    IncreaseBulletSpeed,
    DecreaseFiringCooldown,
}

[CreateAssetMenu(fileName = "New Powerup", menuName = "Powerup")]

public class PowerupScriptableObject : ScriptableObject
{
    [Header("Is this powerup permanent, or temporary?"), Space(-10),
     Header("\tThe \"Increase Current HP\" stat should be marked permanent.")]
    public PowerupPermanence permanance;
    [Header("If the powerup is temporary, how long should its effect last?")]
    public float duration;
    [Header("What state should this powerup affect?")]
    public PowerupStat statToAffect;
    [Header("By what value should this powerup adjust the chosen stat?"), Space(-10),
     Header("\tHP - Integer"), Space(-10),
     Header("\tMove Speed - Float"), Space(-10),
     Header("\tTurn Speed - Float"), Space(-10),
     Header("\tBullet Damage - Integer"), Space(-10),
     Header("\tBullet Speed - Float"), Space(-10),
     Header("\tFiring Speed - Negative Float")]
    public float valueAdjust;
}
