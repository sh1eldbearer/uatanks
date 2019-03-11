using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DT = System.DateTime;

/*
 * This script contains custom functions that will be used universally in this project.
 */

public class CustomFunctions
{
    // Allows me to seed the random number generator with the current date and time
    public static void InitRandomToNow()
    {
        Random.InitState(DT.Now.Day + DT.Now.Month + DT.Now.Year + DT.Now.Hour + DT.Now.Minute +
            DT.Now.Second + DT.Now.Millisecond);
    }
}
