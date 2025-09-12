using SGPdotNET.Observation;
using SGPdotNET.TLE;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class SatteliteFactory : MonoBehaviour
{
    private void Start()
    {
        _output = Output.Instance;
    }

    public async Task GetSattelites(bool fromResources, string source = "")
    {
        foreach (var sat in _satellites)
        {
            Destroy(sat);
        }

        _output.Visible = true;
        _output.Text = "Loading...";
        var satData = string.Empty;

        if (fromResources)
        {
            var mytxtData = (TextAsset)Resources.Load(source);
            satData = mytxtData.text;
        }
        else
        {
            var tleClient = new TleClient();
            satData = await tleClient.DownloadTleAsync(source);
        }

        if (string.IsNullOrWhiteSpace(satData))
        {
            _output.Text = "No satellite data received.";
            _output.Color = Color.red;
            return;
        }

        string[] lines = satData.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < lines.Length - 2; i += 3)
        {
            if (i % 50 == 0)
            {
                //_output.Text = $"Populating... ({i / 3})";
                _output.Visible = true;
                //await Task.Delay(1);
            }

            string name = lines[i].Trim();
            string line1 = lines[i + 1].Trim();
            string line2 = lines[i + 2].Trim();

            if (line1.StartsWith("1 ") && line2.StartsWith("2 "))
            {
                CreateSattelite(name, line1, line2);
            }
            else
            {
                Debug.LogWarning($"Skipping malformed TLE block at line {i}: {name}");
            }
        }

        _output.Text = $"Loaded {lines.Length / 3} satellites.";

        //await Task.Delay(3500);
        //_output.Visible = false;
    }

    void CreateSattelite(string name, string line1, string line2)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(line1) || string.IsNullOrEmpty(line2)) return;

        Tle tle = new Tle(name, line1, line2);
        Satellite satellite = new Satellite(tle);
        SatelliteObject satObj = Instantiate(_satellitePrefab, transform).GetComponent<SatelliteObject>();
        satObj.Setup(satellite);

        _satellites.Add(satObj.gameObject);
    }

    [SerializeField] private GameObject _orbitLinePrefab, _satellitePrefab;
    [SerializeField] private int _orbitSteps = 32;
    [SerializeField] private float _orbitMinutes = 5f;
    private List<GameObject> _satellites = new();
    private Output _output;
}


