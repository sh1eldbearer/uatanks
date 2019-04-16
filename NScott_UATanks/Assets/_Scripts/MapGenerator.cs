using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    /* Public Variables */

    /* Private Variables */
    private int[,] gameMap; // Each element holds a numerical value representing the room tile that should be spawned

    /// <summary>
    /// Seeds the random number generator with an automatically-determined value, then builds the game map.
    /// </summary>
    /// <param name="mapOfTheDay">If true, the RNG will be seeded with today's date. If false,
    /// the RNG will be seed with the current date and time to the millisecond.</param>
    public void GenerateMap(bool mapOfTheDay)
    {
        if (mapOfTheDay)
        {
            CustomFunctions.InitRandomToToday();
        }
        else
        {
            CustomFunctions.InitRandomToNow();
        }

        MapBuilder();
    }

    /// <summary>
    /// Seeds the random number generator with a given value, then builds the game map.
    /// </summary>
    /// <param name="seedValue">The value with which to seed the RNG.</param>
    public void GenerateMap(int seedValue)
    {
        CustomFunctions.InitRandomWithValue(seedValue);

        MapBuilder();
    }

    /// <summary>
    /// Builds the game map.
    /// </summary>
    private void MapBuilder()
    {
        // Creates an array to hold the type of room to instantiate
        gameMap = new int[GameManager.gm.mapHeight, GameManager.gm.mapWidth];

        for (int heightIndex = 0; heightIndex < GameManager.gm.mapHeight; heightIndex++)
        {
            for (int widthIndex = 0; widthIndex < GameManager.gm.mapWidth; widthIndex++)
            {
                // Determine which room tile to spawn
                gameMap[heightIndex, widthIndex] = UnityEngine.Random.Range(0, GameManager.gm.roomTiles.Count);
                // Determine the new room's position in the game world
                Vector3 spawnPosition = new Vector3(GameManager.gm.roomSpacing * widthIndex, 0f, GameManager.gm.roomSpacing * heightIndex);
                // Spawn the new room
                GameObject newRoom = Instantiate(GameManager.gm.roomTiles[gameMap[heightIndex, widthIndex]], spawnPosition, Quaternion.identity, GameManager.gm.gameContainer.GetComponent<Transform>());
                RoomData roomData = newRoom.GetComponent<RoomData>();

                // Remove the appropriate doors so players can move between rooms
                if (heightIndex != GameManager.gm.mapHeight - 1)
                {
                    roomData.northDoor.SetActive(false);
                }
                if (heightIndex != 0)
                {
                    roomData.southDoor.SetActive(false);
                }
                if (widthIndex != GameManager.gm.mapWidth - 1)
                {
                    roomData.eastDoor.SetActive(false);
                }
                if (widthIndex != 0)
                {
                    roomData.westDoor.SetActive(false);
                }
            }
        }
    }
}
