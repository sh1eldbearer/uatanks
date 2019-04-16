using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    [Header("Tank Setting Constants")]
    public const int MIN_HP = -1;
    public const int MAX_HP = 15;
    public const float MIN_MOVE_SPEED = 5f;
    public const float MAX_MOVE_SPEED = 20f;
    public const float MIN_TURN_SPEED = 20f;
    public const float MAX_TURN_SPEED = 90f;
    public const int MIN_BULLET_DAMAGE = 1;
    public const int MAX_BULLET_DAMAGE = 5;
    public const float MIN_BULLET_SPEED = 2.5f;
    public const float MAX_BULLET_SPEED = 10f;
    public const float MIN_FIRING_COOLDOWN = 0.5f;
    public const float MAX_FIRING_COOLDOWN = 3f;
}
