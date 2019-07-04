using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MainBehaviour
{
    public delegate void OnDataDownloadedHandler();

    public event OnDataDownloadedHandler OnDataDownloaded;

    public void OnDefinitionsDownloaded()
    {
        OnDataDownloaded();
    }
}