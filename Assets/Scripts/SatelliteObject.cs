using SGPdotNET.Observation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatelliteObject : MonoBehaviour
{
    public void Setup(Satellite satellite)
    {
        _sat = satellite;
        name = satellite.Name;
        var eciNow = satellite.Predict(_now);
        transform.localPosition = ConvertEciToUnityPositionAt(eciNow.Position, _now);
    }

    public IEnumerator OrbitAnimation(LineRenderer line)
    {
        if (line == null || _sat == null) yield break;

        line.enabled = true;
        line.useWorldSpace = true;
        line.loop = false;

        var orbitPositions = OrbitPositions_Fast();
        if (orbitPositions == null || orbitPositions.Length == 0) yield break;

        line.positionCount = 1;
        line.SetPosition(0, orbitPositions[0]);

        for (int i = 1; i < orbitPositions.Length; i++)
        {
            if (line == null) yield break;

            line.positionCount = i + 1;
            line.SetPosition(i, orbitPositions[i]);
            yield return new WaitForFixedUpdate();
        }

        line.loop = true;
    }

    private Vector3[] OrbitPositions_Fast()
    {
        var now = DataController.StartTime;
        Vector3 r1 = ConvertEciToUnityPositionAt(_sat.Predict(now).Position, _now);
        Vector3 r2 = ConvertEciToUnityPositionAt(_sat.Predict(now.AddMinutes(120)).Position, _now);
        Vector3 r3 = ConvertEciToUnityPositionAt(_sat.Predict(now.AddMinutes(240)).Position, _now);

        Vector3 v1 = r2 - r1;
        Vector3 v2 = r3 - r2;
        var normal = Vector3.Cross(v1, v2).normalized;
        var xAxis = r1.normalized;
        var yAxis = Vector3.Cross(normal, xAxis).normalized;

        float a = (r1.magnitude + r2.magnitude + r3.magnitude) / 3f;
        float b = a * 0.95f;

        int steps = 128;
        List<Vector3> orbitPositions = new();

        for (int i = 0; i <= steps; i++)
        {
            float theta = 2 * Mathf.PI * i / steps;
            var point = a * Mathf.Cos(theta) * xAxis + b * Mathf.Sin(theta) * yAxis;
            orbitPositions.Add(point);
        }

        return orbitPositions.ToArray();
    }

    private Vector3 ConvertEciToUnityPositionAt(SGPdotNET.Util.Vector3 eciKm, DateTime tUtc)
    {
        double gmst = GmstRadians(JulianDate(tUtc));
        double c = Math.Cos(gmst), s = Math.Sin(gmst);
        double xE = c * eciKm.X + s * eciKm.Y;
        double yE = -s * eciKm.X + c * eciKm.Y;
        double zE = eciKm.Z;

        return new Vector3((float)(xE / 1000.0), (float)(zE / 1000.0), (float)(yE / 1000.0));
    }

    private double JulianDate(DateTime utc)
    {
        int Y = utc.Year, M = utc.Month;
        double D = utc.Day + (utc.Hour + (utc.Minute + (utc.Second + utc.Millisecond / 1000.0) / 60.0) / 60.0) / 24.0;
        if (M <= 2) { Y -= 1; M += 12; }
        int A = Y / 100; int B = 2 - A + A / 4;
        return Math.Floor(365.25 * (Y + 4716)) + Math.Floor(30.6001 * (M + 1)) + D + B - 1524.5;
    }

    private double GmstRadians(double jd)
    {
        double T = (jd - 2451545.0) / 36525.0;
        double gmstSec = 67310.54841 + (876600.0 * 3600 + 8640184.812866) * T + 0.093104 * T * T - 6.2e-6 * T * T * T;
        gmstSec = (gmstSec % 86400.0 + 86400.0) % 86400.0;
        return gmstSec * (Math.PI / 43200.0);
    }

    private DateTime _now = DateTime.Now;
    private Satellite _sat;
}
