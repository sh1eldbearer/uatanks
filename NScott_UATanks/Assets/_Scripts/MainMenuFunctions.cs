using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuFunctions : MonoBehaviour
{
    /* Public Variables */
    [Header("Start Game Menu")]
    public Button startButton;
    public GameObject startPanel;
    [Space]
    public ToggleGroup difficultyToggles;
    public Toggle easyToggle;
    public Toggle mediumToggle;
    public Toggle hardToggle;
    public Toggle hellToggle;
    [Space]
    public Dropdown mapTypeMenu;
    public GameObject seedMenu;
    public InputField seedInputField;
    public Text widthText;
    public Slider widthSlider;
    public Text heightText;
    public Slider heightSlider;

    [Header("Options Menu")]
    public Button optionsButton;
    public GameObject optionsPanel;
    [Space]
    public Text soundText;
    public Slider soundSlider;
    public Text musicText;
    public Slider musicSlider;

    /* Private Variables */
    private Text startText;
    private Text optionsText;

	// Use this for initialization
	void Start ()
    {
        startText = startButton.GetComponentInChildren<Text>();
        optionsText = optionsButton.GetComponentInChildren<Text>();

        soundText.text = Constants.SOUND_VOLUME_STRING + GameManager.gm.soundVolume * 100;
        soundSlider.value = GameManager.gm.soundVolume * 100;
        musicText.text = Constants.MUSIC_VOLUME_STRING + GameManager.gm.musicVolume * 100;
        musicSlider.value = GameManager.gm.musicVolume * 100;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void ToggleStartPanel()
    {
        if (startPanel.activeInHierarchy)
        {
            startPanel.SetActive(false);
            startText.text = Constants.START_GAME_STRING;
        }
        else
        {
            startPanel.SetActive(true);
            startText.text = Constants.MENU_OPEN_STRING;
            optionsPanel.SetActive(false);
            optionsText.text = Constants.OPTIONS_STRING;
        }
    }

    public void ToggleOptionsPanel()
    {
        if (optionsPanel.activeInHierarchy)
        {
            optionsPanel.SetActive(false);
            optionsText.text = Constants.OPTIONS_STRING;
        }
        else
        {
            optionsPanel.SetActive(true);
            optionsText.text = Constants.MENU_OPEN_STRING;
            startPanel.SetActive(false);
            startText.text = Constants.START_GAME_STRING;
        }
    }

    public void ChangePlayerCount(int playerCount)
    {
        GameManager.gm.initialNumberOfPlayers = playerCount;
    }

    public void ChangeDifficulty()
    {
        foreach (var toggle in difficultyToggles.ActiveToggles())
        {
            if (toggle == easyToggle)
            {
                GameManager.gm.playerStartingLives = Constants.MAX_PLAYER_LIVES;
                GameManager.gm.SetSpawnRates(49, 50, 1, 0);
            }
            else if (toggle == mediumToggle)
            {
                GameManager.gm.playerStartingLives = (int)(Constants.MAX_PLAYER_LIVES * 0.66f);
                GameManager.gm.SetSpawnRates(60, 20, 10, 10);
            }
            else if (toggle == hardToggle)
            {
                GameManager.gm.playerStartingLives = (int)(Constants.MAX_PLAYER_LIVES * 0.33f);
                GameManager.gm.SetSpawnRates(30, 10, 30, 30);
            }
            else if (toggle == hellToggle)
            {
                GameManager.gm.playerStartingLives = Constants.MIN_PLAYER_LIVES;
                GameManager.gm.SetSpawnRates(0, 0, 0, 100);
            }
        }
    }

    public void SetDifficulty()
    {
        if (mapTypeMenu.value == 2)
        {
            GameManager.gm.mapGenerateType = MapGenerateType.ProvidedSeed;
            seedMenu.SetActive(true);
        }
        else
        {
            if (mapTypeMenu.value == 0)
            {
                GameManager.gm.mapGenerateType = MapGenerateType.RandomMap;
            }
            else
            {
                GameManager.gm.mapGenerateType = MapGenerateType.MapOfTheDay;
            }
            seedMenu.SetActive(false);
        }
    }

    public void SetSeedValue()
    {
        if (seedInputField.text == "" || seedInputField.text == "-")
        {
            GameManager.gm.providedSeed = 0;
        }
        else if (long.Parse(seedInputField.text) < int.MinValue)
        {
            seedInputField.text = int.MinValue.ToString();
            GameManager.gm.providedSeed = int.MinValue;
        }
        else if (long.Parse(seedInputField.text) > int.MaxValue)
        {
            seedInputField.text = int.MaxValue.ToString();
            GameManager.gm.providedSeed = int.MaxValue;
        }
        else
        {
            GameManager.gm.providedSeed = int.Parse(seedInputField.text);
        }
    }

    public void SetMapWidth()
    {
        GameManager.gm.mapWidth = (int)(widthSlider.value);
        widthText.text = Constants.MAP_WIDTH_STRING + widthSlider.value.ToString();
    }

    public void SetMapHeight()
    {
        GameManager.gm.mapHeight = (int)(heightSlider.value);
        heightText.text = Constants.MAP_HEIGHT_STRING + heightSlider.value.ToString();
    }

    public void SetSoundVolume()
    {
        GameManager.gm.soundVolume = soundSlider.value * .01f;
        soundText.text = Constants.SOUND_VOLUME_STRING + soundSlider.value.ToString();
    }

    public void SetMusicVolume()
    {
        GameManager.gm.musicVolume = musicSlider.value * .01f;
        musicText.text = Constants.MUSIC_VOLUME_STRING + musicSlider.value.ToString();
    }

    public void QuitGame()
    {
        GameManager.saveMgr.SaveAllValues();
        Application.Quit();
    }

    public void OnApplicationQuit()
    {
        GameManager.saveMgr.SaveAllValues();
    }
}
