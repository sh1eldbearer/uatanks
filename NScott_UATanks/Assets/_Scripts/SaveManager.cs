using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private void Awake()
    {
        // Singleton pattern
        if (GameManager.saveMgr == null)
        {
            GameManager.saveMgr = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }

        // Checks for default values when the game loads, and sets the appropriate values in the GameManager
        if (!PlayerPrefs.HasKey(Constants.HIGHSCORE_KEY))
        {
            PlayerPrefs.SetInt(Constants.HIGHSCORE_KEY, 0);
        }

        if (!PlayerPrefs.HasKey(Constants.SOUNDVOLUME_KEY))
        {
            PlayerPrefs.SetFloat(Constants.SOUNDVOLUME_KEY, 1f);
        }

        if (!PlayerPrefs.HasKey(Constants.MUSICVOLUME_KEY))
        {
            PlayerPrefs.SetFloat(Constants.MUSICVOLUME_KEY, 1f);
        }

        LoadAllValues();
    }

    // Use this for initialization
    void Start ()
    {
        GameManager.soundMgr.AdjustMusicVolume();
        GameManager.soundMgr.AdjustSoundVolume();
    }

    /// <summary>
    /// Loads all the values stored in PlayerPrefs to the GameManager.
    /// </summary>
    public void LoadAllValues()
    {
        LoadHighScore();
        LoadSoundVolume();
        LoadMusicVolume();
    }

    /// <summary>
    /// Saves all the values stored in the GameManager to PlayerPrefs.
    /// </summary>
    public void SaveAllValues()
    {
        SaveHighScore();
        SaveSoundVolume();
        SaveMusicVolume();
    }

    /// <summary>
    /// Loads the high score stored in PlayerPrefs to the GameManager.
    /// </summary>
    public void LoadHighScore()
    {
        GameManager.gm.highScore = PlayerPrefs.GetInt(Constants.HIGHSCORE_KEY);
    }

    /// <summary>
    /// Saves the high score from the GameManager to PlayerPrefs.
    /// </summary>
    public void SaveHighScore()
    {
        PlayerPrefs.SetInt(Constants.HIGHSCORE_KEY, GameManager.gm.highScore);
    }

    /// <summary>
    /// Loads the sound volume setting stored in PlayerPrefs to the GameManager.
    /// </summary>
    public void LoadSoundVolume()
    {
        GameManager.gm.soundVolume = PlayerPrefs.GetFloat(Constants.SOUNDVOLUME_KEY);
    }

    /// <summary>
    /// Saves the sound volume setting from the GameManager to PlayerPrefs.
    /// </summary>
    public void SaveSoundVolume()
    {
        PlayerPrefs.SetFloat(Constants.SOUNDVOLUME_KEY, GameManager.gm.soundVolume);
    }

    /// <summary>
    /// Loads the music volume setting stored in PlayerPrefs to the GameManager.
    /// </summary>
    public void LoadMusicVolume()
    {

        GameManager.gm.musicVolume = PlayerPrefs.GetFloat(Constants.MUSICVOLUME_KEY);
    }

    /// <summary>
    /// Saves the music volume setting from the GameManager to PlayerPrefs.
    /// </summary>
    public void SaveMusicVolume()
    {
        PlayerPrefs.SetFloat(Constants.MUSICVOLUME_KEY, GameManager.gm.musicVolume);
    }
}
