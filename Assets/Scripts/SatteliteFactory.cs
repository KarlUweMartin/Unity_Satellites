using SGPdotNET.Observation;
using SGPdotNET.TLE;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class SatteliteFactory : MonoBehaviour
{
    private void Start()
    {
        _output = Output.Instance;
    }

    public async Task GetSattelites(string source = "")
    {
        ClearOldSatellites();

        if (_populationCoroutine != null) 
        {
            StopCoroutine(_populationCoroutine);
        }

        _output.Visible = true;
        _output.Text = "Loading...";

        var tleClient = new TleClient();
        var satData = await tleClient.DownloadTleAsync(source);

        if (string.IsNullOrWhiteSpace(satData))
        {
            _output.Text = "No satellite data received.";
            return;
        }

        _populationCoroutine = StartCoroutine(PopulateSatellites(satData));
    }

    private void ClearOldSatellites()
    {
        foreach (var sat in _satellites)
        {
            Destroy(sat);           
        }
        _satellites.Clear();
    }

    private IEnumerator PopulateSatellites(string satData)
    {
        var lines = satData.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        int created = 0;
        const int batch = 20;

        for (int i = 0; i < lines.Length - 2; i += 3)
        {
            var name = lines[i].Trim();
            var line1 = lines[i + 1].Trim();
            var line2 = lines[i + 2].Trim();

            if (line1.StartsWith("1 ") && line2.StartsWith("2 "))
            {
                CreateSattelite(name, line1, line2);
                created++;

                if (created % batch == 0)
                {
                    _output.Text = $"Populating... ({created})";
                    _output.Visible = true;
                    yield return null;
                }
            }
            else
            {
                Debug.LogWarning($"Skipping malformed TLE block at line {i}: {name}");
            }
        }

        _output.Text = $"Loaded {created} satellites.";
    }


    private void CreateSattelite(string name, string line1, string line2)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(line1) || string.IsNullOrEmpty(line2)) return;

        Tle tle = new(name, line1, line2);
        Satellite satellite = new(tle);
        SatelliteObject satObj = Instantiate(_satellitePrefab, transform).GetComponent<SatelliteObject>();
        satObj.Setup(satellite);

        _satellites.Add(satObj.gameObject);
    }

    [SerializeField] private GameObject _orbitLinePrefab, _satellitePrefab;
    [SerializeField] private int _orbitSteps = 32;
    [SerializeField] private float _orbitMinutes = 5f;
    private List<GameObject> _satellites = new();
    private Output _output;
    private Coroutine _populationCoroutine;
}


