using UnityEngine;
using System;

public class EarthBehaviour : MonoBehaviour
{
    private void Start()
    {
        DateTime currentDateTime = AppControl.StartTime;
        float timeOfDayInSeconds = currentDateTime.Hour * 3600f + currentDateTime.Minute * 60f + currentDateTime.Second;
        transform.rotation = Quaternion.Euler(0, timeOfDayInSeconds, 0);
    }
}