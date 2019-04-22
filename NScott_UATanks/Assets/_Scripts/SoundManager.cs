using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    /* Public Variables */
    [Header("Music")]
    public AudioSource menuMusic;
    public AudioSource gameMusic;

    [Header("SFX")]
    public List<AudioSource> tankFireSounds;
    public List<AudioSource> tankDeathSounds;
    public AudioSource bulletHitSound;
    public AudioSource powerupPickupSound;
    public AudioSource buttonPressSound;

    /* Private Variables */
    private float menuMusicBaseVolume;
    private float gameMusicBaseVolume;
    private List<float> tankFireBaseVolumes = new List<float>();
    private List<float> tankDeathBaseVolumes = new List<float>();
    private float bulletHitBaseVolume;
    private float powerupPickupBaseVolume;
    private float buttonPressBaseVolume;

    private void Awake()
    {
        // Singleton pattern
        if (GameManager.soundMgr == null)
        {
            GameManager.soundMgr = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    // Use this for initialization
    void Start ()
    {
        // Base volumes allow me to individually adjust clip volume levels before their use in game
        menuMusicBaseVolume = menuMusic.volume;
        gameMusicBaseVolume = gameMusic.volume;
        foreach (var sound in tankFireSounds)
        {
            tankFireBaseVolumes.Add(sound.volume);
        }
        foreach (var sound in tankDeathSounds)
        {
            tankDeathBaseVolumes.Add(sound.volume);
        }
        bulletHitBaseVolume = bulletHitSound.volume;
        powerupPickupBaseVolume = powerupPickupSound.volume;
        buttonPressBaseVolume = buttonPressSound.volume;
	}

    public void AdjustMusicVolume()
    {
        menuMusic.volume = menuMusicBaseVolume * GameManager.gm.musicVolume;
        gameMusic.volume = gameMusicBaseVolume * GameManager.gm.musicVolume;
        GameManager.saveMgr.SaveMusicVolume();
    }

    public void AdjustSoundVolume()
    {
        foreach (var sound in tankFireSounds)
        {
            int index = tankFireSounds.IndexOf(sound);
            sound.volume = tankFireBaseVolumes[index] * GameManager.gm.soundVolume;
        }
        foreach (var sound in tankDeathSounds)
        {
            int index = tankDeathSounds.IndexOf(sound);
            sound.volume = tankDeathBaseVolumes[index] * GameManager.gm.soundVolume;
        }
        bulletHitSound.volume = bulletHitBaseVolume * GameManager.gm.soundVolume;
        powerupPickupSound.volume = powerupPickupBaseVolume * GameManager.gm.soundVolume;
        buttonPressSound.volume = buttonPressBaseVolume * GameManager.gm.soundVolume;
        GameManager.saveMgr.SaveSoundVolume();
    }

    public void PlayMenuMusic()
    {
        float startTime = gameMusic.time;
        gameMusic.Stop();
        menuMusic.Play();
        menuMusic.time = startTime;
    }

    public void PlayGameMusic()
    {
        float startTime = menuMusic.time;
        menuMusic.Stop();
        gameMusic.Play();
        gameMusic.time = startTime;
    }

    public void StopGameMusic()
    {
        menuMusic.Stop();
        gameMusic.Stop();
    }

    public void PlayTankFireSound()
    {
        tankFireSounds[Random.Range(0, tankFireSounds.Count)].Play();
    }

    public void StopTankFireSound()
    {
        foreach (var sound in tankFireSounds)
        {
            sound.Stop();
        }
    }

    public void PlayTankDeathSound()
    {
        tankDeathSounds[Random.Range(0, tankDeathSounds.Count)].Play();
    }

    public void StopTankDeathSound()
    {
        foreach (var sound in tankDeathSounds)
        {
            sound.Stop();
        }
    }

    public void PlayBulletHitSound()
    {
        bulletHitSound.Play();
    }

    public void StopBulletHitSound()
    {
        bulletHitSound.Stop();
    }

    public void PlayPickupSound()
    {
        powerupPickupSound.Play();
    }

    public void StopPickupSound()
    {
        powerupPickupSound.Stop();
    }

    public void PlayButtonPressSound()
    {
        buttonPressSound.Play();
    }

    public void StopButtonPressSound()
    {
        buttonPressSound.Stop();
    }
}
