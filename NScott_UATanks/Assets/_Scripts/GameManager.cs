using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum MapGenerateType
{
    RandomMap,
    MapOfTheDay,
    ProvidedSeed
}

[DisallowMultipleComponent]

/* 
 * GameManager used for configuring game settings by designers, and for disseminating that information
 * to the rest of the game at runtime.
 * 
 * This should be placed as a component on an empty game object within the game scene.
 */

public class GameManager : MonoBehaviour
{
    /* Public Variables */
    [HideInInspector] public static GameManager gm; // Reference to a GameManager object for singleton pattern
    [HideInInspector] public static SaveManager saveMgr;
    [HideInInspector] public static MapGenerator mapGen;
    [HideInInspector] public static SoundManager soundMgr;
    [HideInInspector] public static AudioListenerPositioner listenerPositioner;
    [HideInInspector] public static MultiplayerCameraSetup cameraSetup;
    [HideInInspector] public static Transform gameContainer;

    public bool gameOverScreen = false;

    [Header("Input Controllers")]
    [HideInInspector] public GameObject p1InputController;
    [HideInInspector] public GameObject p2InputController;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float soundVolume;
    [Range(0f, 1f)] public float musicVolume;

    [Header("General Game Settings")]
    [Tooltip("The number of players to spawn at game start.")]
    // [HideInInspector]
    [Range(1, 2)] public int initialNumberOfPlayers = 1;
    [Tooltip("Determines if player tanks can damage each other.")]
    public bool friendlyFire = false;
    [Tooltip("Determines if enemy tanks can damage each other")]
    public bool enemyFriendlyFire = false;

    [Header("General Tank Settings")]
    [Tooltip("The speed at which tanks can move in reverse, represented as a percentage " +
        "of the tank's base movement speed.")]
    [Range(0.1f, 1f)] public float reverseSpeedRate = 0.75f;

    [Header("Player Tank Settings")]
    [Tooltip("The amount lives a player starts with.")]
    [Range(Constants.MIN_PLAYER_LIVES, Constants.MAX_PLAYER_LIVES)] public int playerStartingLives = 3;
    [Tooltip("The amount of HP a player tank starts with.")]
    [Range(1, 5)] public int playerStartingHP = 3;
    [Tooltip("The maximum amount of HP a player can earn after upgrades during a game.")]
    [Range(5, 15)] public int playerHPCap = 10;
    [Tooltip("The starting movement speed of player tanks.")]
    [Range(Constants.MIN_MOVE_SPEED, 10f)] public float playerStartingMoveSpeed = 8f;
    [Tooltip("The maximum movement speed of player tanks.")]
    [Range(10f, Constants.MAX_MOVE_SPEED)] public float playerMoveSpeedCap = 15f;
    [Tooltip("The starting turning speed of player tanks.")]
    [Range(Constants.MIN_TURN_SPEED, 45f)] public float playerStartingTurnSpeed = 45f;
    [Tooltip("The maximum turning speed of player tanks.")]
    [Range(45f, Constants.MAX_TURN_SPEED)] public float playerTurnSpeedCap = 90f;
    [Tooltip("The starting damage of bullets fired by the player.")]
    [Range(Constants.MIN_BULLET_DAMAGE, 3)] public int playerStartingBulletDamage = 1;
    [Tooltip("The maximum damage of bullets fired by the player.")]
    [Range(3, Constants.MAX_BULLET_DAMAGE)] public int playerBulletDamageCap = 3;
    [Tooltip("The starting movement speed of bullets fired by the player.")]
    [Range(Constants.MIN_BULLET_SPEED, 5f)] public float playerStartingBulletSpeed = 5f;
    [Tooltip("The maximum movement speed of bullets fired by the player.")]
    [Range(5f, Constants.MAX_BULLET_SPEED)] public float playerBulletSpeedCap = 10f;
    [Tooltip("The starting length of the player's firing cooldown.")]
    [Range(1.5f, Constants.MAX_FIRING_COOLDOWN)] public float playerStartingFiringCooldown = 1.5f;
    [Tooltip("The minimum length of the player's firing cooldown.")]
    [Range(Constants.MIN_FIRING_COOLDOWN, 1.5f)] public float playerMinimumFiringCooldown = 0.5f;
    [Header("Player Tank Prefabs")]
    public GameObject playerPrefab;

    [Header("Enemy Tank Settings")]
    [Tooltip("The amount of HP an enemy tank starts with.")]
    [Range(1, 5)] public int enemyStartingHP = 3;
    [Tooltip("The maximum amount of HP an enemy can earn after upgrades during a game.")]
    [Range(5, 15)] public int enemyHPCap = 15;
    [Tooltip("The starting movement speed of enemy tanks.")]
    [Range(Constants.MIN_MOVE_SPEED, 10f)] public float enemyStartingMoveSpeed = 8f;
    [Tooltip("The maximum movement speed of enemy tanks.")]
    [Range(10f, 20f)] public float enemyMoveSpeedCap = 15f;
    [Tooltip("The starting turning speed of enemy tanks.")]
    [Range(Constants.MIN_TURN_SPEED, 45f)] public float enemyStartingTurnSpeed = 45f;
    [Tooltip("The maximum turning speed of enemy tanks.")]
    [Range(45f, 90f)] public float enemyTurnSpeedCap = 90f;
    [Tooltip("The starting damage of bullets fired by the enemy.")]
    [Range(Constants.MIN_BULLET_DAMAGE, 3)] public int enemyStartingBulletDamage = 1;
    [Tooltip("The maximum damage of bullets fired by the enemy.")]
    [Range(3, 5)] public int enemyBulletDamageCap = 3;
    [Tooltip("The starting movement speed of bullets fired by the enemy.")]
    [Range(Constants.MIN_BULLET_SPEED, 5f)] public float enemyStartingBulletSpeed = 5f;
    [Tooltip("The maximum movement speed of bullets fired by the enemy.")]
    [Range(5f, 10f)] public float enemyBulletSpeedCap = 10f;
    [Tooltip("The starting length of the enemy's firing cooldown.")]
    [Range(1.5f, 3f)] public float enemyStartingFiringCooldown = 1.5f;
    [Tooltip("The minimum length of the enemy's firing cooldown.")]
    [Range(Constants.MIN_FIRING_COOLDOWN, 1.5f)] public float enemyMinimumFiringCooldown = 0.5f;
    [Tooltip("The distance at which an enemy tank considers itself \"close enough\" to a waypoint.")]
    [Range(1f, 10f)] public float waypointCloseEnoughDistance = 2f;
    [Tooltip("The distance at which an enemy tank considers itself \"close enough\" to a target tank.")]
    [Range(1f, 25f)] public float tankCloseEnoughDistance = 15f;
    [Tooltip("The maximum length of time an enemy tank will pursue a player without seeing them, before " +
        "returning to their default behavior.\nNOTE: Reaper tanks will ignore this value!")]
    [Range(1f, 10f)] public float maxPursuitTime = 3f;

    [Header("Enemy Tank Prefabs")]
    public GameObject standardEnemyPrefab;
    public GameObject cowardEnemyPrefab;
    public GameObject reaperEnemyPrefab;
    public GameObject captainEnemyPrefab;

    [Header("Enemy Tank Spawn Settings")]
    [Range(0, 100)] public int standardSpawnRate = 6;
    [Range(0, 100)] public int cowardSpawnRate = 2;
    [Range(0, 100)] public int reaperSpawnRate = 1;
    [Range(0, 100)] public int captainSpawnRate = 1;
    [HideInInspector] public List<GameObject> enemySpawnTable;


    [Header("Bullet Settings")]
    [Tooltip("The bullet prefab object instantiated by a tank when it fires.")]
    public GameObject bulletPrefab;
    [Tooltip("The length of time a bullet should exist in the game world before destroying itself.")]
    [Range(1f, 5f)] public float bulletLifeSpan = 5f;
    [Tooltip("Determines whether or not bullets should destroy each other when they collide.")]
    public bool bulletCollisions = false;

    [Header("Powerup Settings")]
    [Tooltip("The initial delay before a powerup spawns into the game world.")]
    [Range(0f, 45f)] public float initialPowerupSpawnDelay = 10f;
    [Tooltip("The interval of time between powerups spawning in the game world.")]
    [Range(15f, 90f)] public float powerupSpawnDelay = 15f;
    [Tooltip("The time before an unclaimed powerup despawns from the game world.")]
    [Range(10f, 30f)] public float powerupLifespan = 20f;
    [Tooltip("The rotation speed of powerups in the game world.")]
    [Range(0f, 360f)] public float powerupRotateSpeed = 45f;
    [Tooltip("The bounce speed of powerups in the game world.")]
    [Range(0f, 25f)] public float powerupBounceSpeed = 5f;
    [Tooltip("The bounce amplitude of powerups in the game world.")]
    [Range(0f, 2f)] public float powerupBounceAmplitude = 1f;
    [Tooltip("The list of powerup objects that can be spawned into the world.")]

    [Header("Powerup Prefabs")]
    public List<GameObject> powerupPrefabs;

    [Header("Map Generation Settings")]
    public MapGenerateType mapGenerateType;
    public int providedSeed = 0;
    [Range(1, 10)] public int mapWidth = 3;
    [Range(1, 10)] public int mapHeight = 3;
    public List<GameObject> roomTiles;
    public float roomSpacing = 64f;


    [Header("Game Cameras & Camera Settings")]
    [Tooltip("The camera used to render player 1's display")]
    public Camera player1Camera;
    [Tooltip("Determines if player 1's camera can rotate with the tank at it moves.")]
    public bool p1RotateCamera = false;
    [Space]
    [Tooltip("The camera used to render player 2's display")]
    public Camera player2Camera;
    [Tooltip("Determines if player 2's camera can rotate with the tank as it moves.")]
    public bool p2RotateCamera = false;

    [Header("Layers & LayerMasks")]
    [Tooltip("The layer for tank objects.")]
    public LayerMask tankLayer;
    [Tooltip("A LayerMask that ignores powerups.")]
    public LayerMask ignorePowerupLayer;
    [Tooltip("The layer for showing player 1's UI")]
    public LayerMask p1UILayer;
    [Tooltip("The layer for showing player 2's UI")]
    public LayerMask p2UILayer;

    [Header("UI Settings")]
    [Tooltip("The length of time it takes for any cooldown bars to fade out when " +
        "their countdowns have completed.")]
    [Range(0.1f, 2f)] public float barFadeTime = 0.25f;

    [Header("Tank Lists")]
    [Tooltip("The list of currently active player tanks. The game is over if there are " +
        "no active players remaining in the game.")]
    public List<TankData> players;
    [Tooltip("The list of currently active enemy tanks.")]
    public List<TankData> enemies;

    [Header("Spawn Point Lists")]
    [Tooltip("The list of player spawn points active in the game.")]
    [HideInInspector] public List<Transform> playerSpawnPoints;
    [Tooltip("The list of enemy spawn points active in the game.")]
    [HideInInspector] public List<Transform> enemySpawnPoints;
    [Tooltip("The list of power-up spawn points active in the game.")]
    [HideInInspector] public List<Transform> powerupSpawnPoints;

    [Header("Game Status")]
    [Tooltip("Player 1's score.")]
    public int p1Score = 0;
    [Tooltip("Player 2's score.")]
    public int p2Score = 0;

    [Header("Point Values")]
    public int highScore = 0;
    [Tooltip("The base value an enemy tank is worth when it's destroyed.")]
    [Range(10,10000)] public int baseTankValue = 100;
    [Tooltip("For every hit point over the starting value, an enemy tank will be worth " +
        "this percentage more points. (Example: if this value is set to 1.5, an enemy tank " +
        "with one more hit point than the starting value would be worth <Base Value> + " +
        "(<Bonus Value> X <Base Value>) X <Number of HP over the starting value>).)")]
    [Range(0f, 3f)] public float bonusModifer = 0.5f;

    /* Private Variables */
    private bool isGameRunning = false;
    private float powerupSpawnTimer;

    private void Awake()
    {
        // Singleton pattern for game manager preservation
        if (gm == null)
        {
            gm = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Initializes the game conditions for play.
    /// </summary>
    public void InitializeGame()
    {
        // Initializes game values
        p1Score = 0;
        p2Score = 0;
        players.Clear();
        enemies.Clear();
        enemySpawnTable.Clear();
        playerSpawnPoints.Clear();
        enemySpawnPoints.Clear();
        powerupSpawnPoints.Clear();

        // Position the audio listener
        listenerPositioner.PositionListener(); // This line is a tongue twister

        // Loads the game scene
        SceneManager.LoadScene(1);

        // Generates the game map
        switch (mapGenerateType)
        {
            case MapGenerateType.RandomMap:
                mapGen.GenerateMap(false);
                break;
            case MapGenerateType.MapOfTheDay:
                mapGen.GenerateMap(true);
                break;
            case MapGenerateType.ProvidedSeed:
                mapGen.GenerateMap(providedSeed);
                break;
        }

        // Spawns players
        for (int index = 1; index <= initialNumberOfPlayers; index++)
        {
            if (index > 2)
            {
                break;
            }

            SpawnNewPlayer();

            switch (index)
            {
                case 1:
                    p1InputController.SetActive(true);
                    InputController.p1Input.Initialize();
                    break;
                case 2:
                    p2InputController.SetActive(true);
                    InputController.p2Input.Initialize();
                    break;
            }
        }

        // Populates spawn table with enemy spawn rates
        for (int index = 1; index < 100; index++)
        {
            bool addedSomething = false;

            if (index <= standardSpawnRate)
            {
                enemySpawnTable.Add(standardEnemyPrefab);
                addedSomething = true;
            }
            if (index <= cowardSpawnRate)
            {
                enemySpawnTable.Add(cowardEnemyPrefab);
                addedSomething = true;
            }
            if (index <= captainSpawnRate)
            {
                enemySpawnTable.Add(captainEnemyPrefab);
                addedSomething = true;
            }
            if (index <= reaperSpawnRate)
            {
                enemySpawnTable.Add(reaperEnemyPrefab);
                addedSomething = true;
            }

            // If nothing was added this pass, exit the loop
            if (!addedSomething)
            {
                break;
            }
        }

        // Spawns enemies
        int range = UnityEngine.Random.Range((int)(enemySpawnPoints.Count / 2), enemySpawnPoints.Count);
        for (int index = 0; index < range; index++)
        {
            SpawnNewEnemy();
        }

        // Re-adds all used spawn points to the spawn point lists
        gameContainer.BroadcastMessage("RegisterThisSpawnPoint");

        // Sets the spawn time of the first powerup
        powerupSpawnTimer = initialPowerupSpawnDelay;

        // Changes from menu music to game music
        soundMgr.PlayGameMusic();

        isGameRunning = true;
    }

    // Update is called once per frame
    private void Update()
    {
        // Ensures certain values can't be accidentally set too high or low in the inspector
        playerStartingHP = Mathf.Clamp(playerStartingHP, Constants.MIN_HP, Constants.MAX_HP);
        playerHPCap = Mathf.Clamp(playerHPCap, playerStartingHP, Constants.MAX_HP);
        enemyStartingHP = Mathf.Clamp(enemyStartingHP, Constants.MIN_HP, Constants.MAX_HP);
        enemyHPCap = Mathf.Clamp(enemyHPCap, enemyStartingHP, Constants.MAX_HP);
        barFadeTime = Mathf.Clamp(barFadeTime, 0.1f, 2f);
        baseTankValue = Mathf.Clamp(baseTankValue, 10, 10000);
        bonusModifer = Mathf.Clamp(bonusModifer, 0f, 3f);

        if (isGameRunning)
        {
            if (players.Count == 0)
            {
                // No players left - the game is over
                    EndGame();
            }

            // Updates player's scores
            try
            {
                p1Score = InputController.p1Input.tankData.tankScorer.score;
            }
            catch { }

            try
            {
                p2Score = InputController.p2Input.tankData.tankScorer.score;
            }
            catch { }
            // Updates the score values stored in the gameManager
            CompareScores();

            powerupSpawnTimer -= Time.deltaTime;
            if (powerupSpawnTimer <= 0f)
            {
                SpawnPowerup();
                powerupSpawnTimer = powerupSpawnDelay;
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                Transform closestSpawnpoint = null;
                float closestDistance = float.MaxValue;

                foreach (var spawnPoint in powerupSpawnPoints)
                {
                    float currentDistance = Vector3.Distance(players[0].tankTf.position, spawnPoint.position);
                    if (currentDistance < closestDistance)
                    {
                        closestDistance = currentDistance;
                        closestSpawnpoint = spawnPoint;
                    }
                }
                SpawnPowerup(closestSpawnpoint);
            }

            foreach (var enemy in enemies)
            {
                if (enemy == null)
                {
                    enemies.Remove(enemy);
                    SpawnNewEnemy();
                    break;
                }
            }

            // Developer commands for demos and testing
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EndGame();
            }
        }
    }

    private void FixedUpdate()
    {
        // Developer commands for demos and testing
        if (Input.GetKey(KeyCode.Minus))
        {
            soundVolume = Mathf.Clamp(soundVolume -= 0.01f, 0f, 1f);
            soundMgr.AdjustSoundVolume();
        }
        else if (Input.GetKey(KeyCode.Equals))
        {
            soundVolume = Mathf.Clamp(soundVolume += 0.01f, 0f, 1f);
            soundMgr.AdjustSoundVolume();
        }
        if (Input.GetKey(KeyCode.LeftBracket))
        {
            musicVolume = Mathf.Clamp(musicVolume -= 0.01f, 0f, 1f);
            soundMgr.AdjustMusicVolume();
        }
        else if (Input.GetKey(KeyCode.RightBracket))
        {
            musicVolume = Mathf.Clamp(musicVolume += 0.01f, 0f, 1f);
            soundMgr.AdjustMusicVolume();
        }
    }

    /// <summary>
    /// Spawns a player into the game world.
    /// </summary>
    private void SpawnNewPlayer()
    {
        // Determines which spawn point to spawn the player at
        int spawnIndex = UnityEngine.Random.Range(0, playerSpawnPoints.Count);
        GameObject newTank = Instantiate(playerPrefab, playerSpawnPoints[spawnIndex].position, playerSpawnPoints[spawnIndex].rotation, playerSpawnPoints[spawnIndex].parent.parent.parent);
        players.Add(newTank.GetComponent<TankData>());
        // Removes the used spawn point from the spawn point list (prevents overlaps)
        playerSpawnPoints.RemoveAt(spawnIndex);
    }

    /// <summary>
    /// Creates a new player tank being controlled by the InputController that calls this function
    /// </summary>
    /// <param name="controller">The InputController requesting a new tank.</param>
    public void RespawnPlayer(TankData tankData)
    {
        // Determines which spawn point to spawn the player at
        int spawnIndex = UnityEngine.Random.Range(0, playerSpawnPoints.Count);
        // Moves the tank to the spawn point
        tankData.tankTf.position = playerSpawnPoints[spawnIndex].position;
        // Resets the tank's health
        tankData.currentHP = tankData.maxHP;
    }

    public void DestroyPlayer(TankData player)
    {
        if (player == InputController.p1Input.tankData)
        {
            InputController.p1Input.gameObject.SetActive(false);
        }
        else if (player == InputController.p2Input.tankData)
        {
            InputController.p2Input.gameObject.SetActive(false);
        }

        // Destroys the player tank
        Destroy(player.gameObject);
        if (GameManager.gm.players.Count > 0)
        {
            // Adjust the camera viewports
            GameManager.cameraSetup.ConfigureViewports();
        }
    }

    /// <summary>
    /// Spawns an enemy into the game world.
    /// </summary>
    private void SpawnNewEnemy()
    {
        // Determines which enemy type to spawn
        int enemyIndex = UnityEngine.Random.Range(0, enemySpawnTable.Count);
        GameObject enemyToSpawn = enemySpawnTable[enemyIndex];
        // Determines which spawn point to spawn the enemy at
        int spawnIndex = UnityEngine.Random.Range(0, enemySpawnPoints.Count);
        Instantiate(enemyToSpawn, enemySpawnPoints[spawnIndex].position, enemySpawnPoints[spawnIndex].rotation, enemySpawnPoints[spawnIndex].parent.parent.Find("Tanks").transform);
        // Removes the used spawn point from the spawn point list (prevents overlaps)
        enemySpawnPoints.RemoveAt(spawnIndex);
    }

    /// <summary>
    /// Spawns a powerup into the game world.
    /// </summary>
    private void SpawnPowerup()
    {
        // Determines which powerup to spawn
        int powerupIndex = UnityEngine.Random.Range(0, powerupPrefabs.Count);
        GameObject powerupToSpawn = powerupPrefabs[powerupIndex];
        // Determines which spawn point to spawn the enemy at
        int spawnIndex = UnityEngine.Random.Range(0, powerupSpawnPoints.Count);
        Instantiate(powerupToSpawn, powerupSpawnPoints[spawnIndex].position, powerupSpawnPoints[spawnIndex].rotation, powerupSpawnPoints[spawnIndex].parent.parent.transform);
    }

    /// <summary>
    /// Spawns a powerup into the game world at the specified location.
    /// </summary>
    private void SpawnPowerup(Transform closestWaypoint)
    {
        // Determines which powerup to spawn
        int powerupIndex = UnityEngine.Random.Range(0, powerupPrefabs.Count);
        GameObject powerupToSpawn = powerupPrefabs[powerupIndex];
        // Determines which spawn point to spawn the enemy at
        Instantiate(powerupToSpawn, closestWaypoint.position, closestWaypoint.rotation, closestWaypoint.parent.parent.transform);
    }

    /// <summary>
    /// Changes the spawn rates of each type of enemy tank. Only has an effect before the game is initialized.
    /// </summary>
    /// <param name="standardRate"></param>
    /// <param name="cowardRate"></param>
    /// <param name="captainRate"></param>
    /// <param name="reaperRate"></param>
    public void SetSpawnRates(int standardRate, int cowardRate, int captainRate, int reaperRate)
    {
        standardSpawnRate = standardRate;
        cowardSpawnRate = cowardRate;
        captainSpawnRate = captainRate;
        reaperSpawnRate = reaperRate;
    }

    public void EndGame()
    {
        isGameRunning = false;
        gameOverScreen = true;
        Destroy(gameContainer.gameObject);
        gameContainer = null;
        soundMgr.PlayMenuMusic();
        SceneManager.LoadScene(0);
    }

    public void CompareScores()
    {
        if (p1Score > highScore)
        {
            highScore = p1Score;
            saveMgr.SaveHighScore();
        }
        else if (p2Score > highScore)
        {
            highScore = p2Score;
            saveMgr.SaveHighScore();
        }
    }
}