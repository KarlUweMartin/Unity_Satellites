using System;
using UnityEngine.Events;

public static class AppControl
{
    public static DateTime StartTime { get; private set; } = DateTime.Now;


    public static UnityEvent<SatelliteObject> OnSatelliteChanged = new();
    private static SatelliteObject _selectedSatellite;
    public static SatelliteObject SelectedSatellite
    {
        get => _selectedSatellite;
        set
        {
            _selectedSatellite = value;
            OnSatelliteChanged.Invoke(value);
        }
    }

    public static UnityEvent<string> OnDataSetChanged = new();
    private static string _datSetTitle;
    public static string DataSetTitle 
    {
        get => _datSetTitle;
        set 
        {
            _datSetTitle = value;
            OnDataSetChanged.Invoke(value);
        }    
    }
}
