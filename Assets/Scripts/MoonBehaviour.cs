using UnityEngine;
using System;

public class MoonBehaviour : MonoBehaviour
{
    void Start()
    {
        DateTime reference = new DateTime(2000, 1, 1, 12, 0, 0, DateTimeKind.Utc);
        TimeSpan elapsed = DataController.StartTime - reference;

        double daysElapsed = elapsed.TotalDays;
        double orbitFraction = daysElapsed / lunarOrbitPeriodDays;
        double angleRad = orbitFraction * 2.0 * Math.PI;

        float x = _scaledMoonDistance * Mathf.Cos((float)angleRad);
        float z = _scaledMoonDistance * Mathf.Sin((float)angleRad);


        transform.position = _earth.position + new Vector3(x, 0f, z);
        transform.LookAt(_earth.position);
    }

    [SerializeField] private Transform _earth;
    private const float _scaledMoonDistance = 392.2f;
    private const double lunarOrbitPeriodDays = 27.321661;
}