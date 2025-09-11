using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataSetList : MonoBehaviour
{
    void Start()
    {
        var tle = new TleClient();

        var dataSets = Resources.LoadAll<TextAsset>("TLE");

        foreach (var dataSet in dataSets  /*tle.DataSetUrls*/) 
        {
            var entry = Instantiate(_entryPrefab, _list, false);
            entry.GetComponentInChildren<TextMeshProUGUI>().text = dataSet.name;
            entry.GetComponent<Button>().onClick.AddListener(() =>
            {
                _selectedDataSet.text = dataSet.name;
                _ = _satelliteFactory.GetSattelites(true, Path.Combine("TLE", dataSet.name));
                Open = false;
            });
            //entry.GetComponent<Button>().onClick.AddListener(() => _ = _satelliteFactory.GetSattelites(true));

            _dolly.enabled = false;
        }
    }

    public void ToggleOpen() 
    {
        Open = !gameObject.activeSelf;
    }

    public bool Open 
    {
        get => gameObject.activeSelf;
        set 
        {        
            gameObject.SetActive(value);
            _dolly.enabled = !value;           
        }
    }

    [SerializeField] private Transform _list;
    [SerializeField] private GameObject _entryPrefab;
    [SerializeField] private SatteliteFactory _satelliteFactory;
    [SerializeField] private CameraDolly _dolly;
    [SerializeField] private TextMeshProUGUI _selectedDataSet;
}
