using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MainBehaviour
{
    public T GetScreen<T>() where T : ScreenBehaviour
    {
        foreach (Transform t in transform)
        {
            var currentScreen = t.GetComponent<T>();

            if (currentScreen != null)
            {
                return currentScreen;
            }
        }

        return null;
    }

    public void SetActiveScreen<T>(bool toActiv) where T : ScreenBehaviour
    {
        foreach (Transform t in transform)
        {
            var currentScreen = t.GetComponent<T>();

            if (currentScreen != null)
            {
                currentScreen.gameObject.SetActive(toActiv);
            }
        }
    }
}