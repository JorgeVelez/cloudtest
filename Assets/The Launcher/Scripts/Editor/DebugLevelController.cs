using System;
using System.Collections;
using UnityEngine;

public class DebugLevelController
{
    public static int DebugLogLevel = 0;
    public static void Log(string message, int level = 0)
    {
        if (DebugLogLevel == -1) return;
        if (level <= DebugLogLevel)
            UnityEngine.Debug.Log(message);
    }
}