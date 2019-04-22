using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuFunctions : MonoBehaviour
{
    /* Public Variables */
    [Space] public GameObject mainMenu;
    [Space] public GameObject gameOverMenu;

    [Header("Start Game Menu")]
    public Button startButton;
    public GameObject startPanel;
    [Space]
    public Toggle player1Toggle;
    public Toggle player2Toggle;
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

    [Header("Credits Menu")]
    public Button creditsButton;
    public GameObject creditsPanel;

    [Header("Game Over Menu")]
    public Text p1ScoreText;
    public Text p2ScoreText;
    public Text highScoreText;

    /* Private Variables */
    private Text startText;
    private Text optionsText;
    private Text creditsText;

	// Use this for initialization
	void Start ()
    {
        startText = startButton.GetComponentInChildren<Text>();
        optionsText = optionsButton.GetComponentInChildren<Text>();
        creditsText = creditsButton.GetComponentInChildren<Text>();

        soundText.text = Constants.SOUND_VOLUME_STRING + GameManager.gm.soundVolume * 100;
        soundSlider.value = GameManager.gm.soundVolume * 100;
        musicText.text = Constants.MUSIC_VOLUME_STRING + GameManager.gm.musicVolume * 100;
        musicSlider.value = GameManager.gm.musicVolume * 100;

        if (GameManager.gm.initialNumberOfPlayers == 1)
        {
            player1Toggle.isOn = true;
            player2Toggle.isOn = false;
        }
        else
        {
            player1Toggle.isOn = false;
            player2Toggle.isOn = true;
        }

        ChangeDifficulty();

        switch (GameManager.gm.mapGenerateType)
        {
            case MapGenerateType.RandomMap:
                mapTypeMenu.value = 0;
                break;
            case MapGenerateType.MapOfTheDay:
                mapTypeMenu.value = 1;
                break;
            case MapGenerateType.ProvidedSeed:
                mapTypeMenu.value = 3;
                break;
        }
        seedInputField.text = GameManager.gm.providedSeed.ToString();

        heightSlider.value = GameManager.gm.mapHeight;
        widthSlider.value = GameManager.gm.mapWidth;

        p1ScoreText.text = GameManager.gm.p1Score.ToString();
        p2ScoreText.text = GameManager.gm.p2Score.ToString();
        highScoreText.text = GameManager.gm.highScore.ToString();
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (GameManager.gm.gameOverScreen)
        {
            gameOverMenu.SetActive(true);
            mainMenu.SetActive(false);
        }
        else
        {
            mainMenu.SetActive(true);
            gameOverMenu.SetActive(false);
        }
	}

    public void InitializeGame()
    {
        GameManager.gm.InitializeGame();
    }

    public void ToggleStartPanel()
    {
        if (startPanel.activeInHierarchy)
        {
            startPanel.SetActive(false);
            startText.text = Constants.START_GAME_BUTTON_STRING;
        }
        else
        {
            startPanel.SetActive(true);
            startText.text = Constants.MENU_OPEN_STRING;
            optionsPanel.SetActive(false);
            optionsText.text = Constants.OPTIONS_BUTTON_STRING;
            creditsPanel.SetActive(false);
            creditsText.text = Constants.CREDITS_BUTTON_STRING;
        }
    }

    public void ToggleOptionsPanel()
    {
        if (optionsPanel.activeInHierarchy)
        {
            optionsPanel.SetActive(false);
            optionsText.text = Constants.OPTIONS_BUTTON_STRING;
        }
        else
        {
            startPanel.SetActive(false);
            startText.text = Constants.START_GAME_BUTTON_STRING;
            optionsPanel.SetActive(true);
            optionsText.text = Constants.MENU_OPEN_STRING;
            creditsPanel.SetActive(false);
            creditsText.text = Constants.CREDITS_BUTTON_STRING;
        }
    }

    public void ToggleCreditsPanel()
    {
        if (creditsPanel.activeInHierarchy)
        {
            creditsPanel.SetActive(false);
            creditsText.text = Constants.CREDITS_BUTTON_STRING;
        }
        else
        {
            startPanel.SetActive(false);
            startText.text = Constants.START_GAME_BUTTON_STRING;
            optionsPanel.SetActive(false);
            optionsText.text = Constants.OPTIONS_BUTTON_STRING;
            creditsPanel.SetActive(true);
            creditsText.text = Constants.MENU_OPEN_STRING;
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

    public void SetMapType()
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
        GameManager.gm.soundVolume = soundSlider.value * 0.01f;
        GameManager.soundMgr.AdjustSoundVolume();
        soundText.text = Constants.SOUND_VOLUME_STRING + ((int)(soundSlider.value)).ToString();
    }

    public void SetMusicVolume()
    {
        GameManager.gm.musicVolume = musicSlider.value * 0.01f;
        GameManager.soundMgr.AdjustMusicVolume();
        musicText.text = Constants.MUSIC_VOLUME_STRING + ((int)(musicSlider.value)).ToString();
    }

    public void BackToMainMenu()
    {
        GameManager.gm.gameOverScreen = false;
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
