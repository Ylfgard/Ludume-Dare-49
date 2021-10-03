using System.Collections.Generic;
using UnityEngine;

public static class GamePauser
{
    private static List<GameObject> openedObjects = new List<GameObject>();
    public static void StopGame(GameObject openObject)
    {
        openedObjects.Add(openObject);
        Time.timeScale = 0;
    }

    public static void ContinueGame(GameObject closeObject)
    {
        openedObjects?.Remove(closeObject);
        if(openedObjects.Count == 0)
           Time.timeScale = 1;
    }
}
