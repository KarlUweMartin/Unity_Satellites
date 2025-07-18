using SGPdotNET.Observation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatelliteObject : MonoBehaviour
{
    public void Setup(Satellite satellite)
    {
        name = satellite.Name;
        _satellite = satellite;

        var eciNow = _satellite.Predict(DateTime.UtcNow);
        transform.localPosition = ConvertEciToUnityPosition(eciNow.Position);
    }

    public void DrawOrbit()
    {
        var orbitObj = Instantiate(_orbitLinePrefab, transform);
        orbitObj.name = $"{_satellite.Name}_Orbit";

        var line = orbitObj.GetComponent<LineRenderer>();
        line.positionCount = _orbitSteps;


        var pastSteps = _orbitSteps / 2;
        var futureSteps = _orbitSteps - pastSteps; 

        var stepMinutes = 90 / _orbitSteps;
        var startTime = DataController.StartTime;

        for (int i = 0; i < pastSteps; i++)
        {
            var t = startTime.AddMinutes(-((pastSteps - i) * stepMinutes));
            var eci = _satellite.Predict(t);
            var unityPos = ConvertEciToUnityPosition(eci.Position);
            line.SetPosition(i, unityPos);
        }
        for (int i = 0; i < futureSteps; i++)
        {
            var t = startTime.AddMinutes(i * stepMinutes);
            var eci = _satellite.Predict(t);
            var unityPos = ConvertEciToUnityPosition(eci.Position);
            line.SetPosition(pastSteps + i, unityPos);
        }
    }
    public GameObject DrawFullOrbit()
    {
        var orbitObj = Instantiate(_orbitLinePrefab, transform);
        orbitObj.name = $"{_satellite.Name}_Orbit";
        var line = orbitObj.GetComponent<LineRenderer>();

        var now = DataController.StartTime;
        Vector3 r1 = ConvertEciToUnityPosition(_satellite.Predict(now).Position);
        Vector3 r2 = ConvertEciToUnityPosition(_satellite.Predict(now.AddMinutes(120)).Position);
        Vector3 r3 = ConvertEciToUnityPosition(_satellite.Predict(now.AddMinutes(240)).Position);

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

        StartCoroutine(OrbitAnimation(line, orbitPositions));

        return orbitObj;
    }

    private IEnumerator OrbitAnimation(LineRenderer line, List<Vector3> points) 
    {
        line.positionCount = 1;
        line.SetPosition(0, line.gameObject.transform.position);

        for (int i = 1; i < points.Count; i++) 
        {
            if (line == null) break;

            line.positionCount++;
            line.SetPosition(i, points[i]);
            yield return new WaitForFixedUpdate();
        }
    }

    private Vector3 ConvertEciToUnityPosition(SGPdotNET.Util.Vector3 sgpPosition)
    {
        float x = (float)sgpPosition.X / 1000;
        float y = (float)sgpPosition.Z / 1000;
        float z = (float)sgpPosition.Y / 1000;

        return new Vector3(x, y, z);
    }

    private Satellite _satellite;
    public int _orbitSteps = 32;
    public float _orbitMinutes = 120f;
    public GameObject _orbitLinePrefab;
}
