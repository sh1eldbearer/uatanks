using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DT = System.DateTime;

/*
 * This script contains custom functions that will be used universally in this project.
 */

public static class CustomFunctions
{
    // Allows me to seed the random number generator with the current date and time
    public static void InitRandomToNow()
    {
        UnityEngine.Random.InitState(DT.Now.Day + DT.Now.Month + DT.Now.Year + DT.Now.Hour + DT.Now.Minute +
            DT.Now.Second + DT.Now.Millisecond);
    }

    // Allows me to seed the random number generator with the current date
    public static void InitRandomToToday()
    {
        UnityEngine.Random.InitState(DT.Now.Day + DT.Now.Month + DT.Now.Year);
    }

    // Allows me to seed the random number generator with a specific value
    public static void InitRandomWithValue(int seedValue)
    {
        UnityEngine.Random.InitState(seedValue);
    }
}
