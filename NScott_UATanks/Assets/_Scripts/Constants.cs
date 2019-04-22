using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    // Main menu button string constants
    public const string START_GAME_BUTTON_STRING = "Start Game";
    public const string OPTIONS_BUTTON_STRING = "Options";
    public const string CREDITS_BUTTON_STRING = "Credits";
    public const string MENU_OPEN_STRING = ">>>";
    public const string MAP_WIDTH_STRING = "Map Width: ";
    public const string MAP_HEIGHT_STRING = "Map Height: ";
    public const string SOUND_VOLUME_STRING = "Sound Volume: ";
    public const string MUSIC_VOLUME_STRING = "Music Volume: ";

    // Tank Setting constants
    public const int MIN_PLAYER_LIVES = 1;
    public const int MAX_PLAYER_LIVES = 6;
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

    // Player pref key values
    public const string HIGHSCORE_KEY = "HighScore";
    public const string SOUNDVOLUME_KEY = "SoundVolume";
    public const string MUSICVOLUME_KEY = "MusicVolume";
}
