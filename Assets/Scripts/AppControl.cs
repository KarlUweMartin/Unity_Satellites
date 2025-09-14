using System;
using UnityEngine.Events;

public static class AppControl
{
    public static DateTime StartTime { get; private set; } = DateTime.Now;

    public static UnityEvent<string> OnTitleChanged = new();

    private static string _activeTitle;
    public static string ActiveTitle 
    {
        get => _activeTitle;
        set 
        {
            _activeTitle = value;
            OnTitleChanged.Invoke(value);
        }    
    }
}
