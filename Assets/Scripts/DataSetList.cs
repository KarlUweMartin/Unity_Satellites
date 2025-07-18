using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataSetList : MonoBehaviour
{
    void Start()
    {
        var tle = new TleClient();
        foreach (var dataSet in tle.DataSetUrls) 
        {
            var entry = Instantiate(_entryPrefab, _list, false);
            entry.GetComponentInChildren<TextMeshProUGUI>().text = dataSet.Key;
            //entry.GetComponent<Button>().onClick.AddListener(() => _ = _satelliteFactory.GetSattelites(false, dataSet.Value));
            entry.GetComponent<Button>().onClick.AddListener(() => _ = _satelliteFactory.GetSattelites(true));
        }
    }

    [SerializeField] private Transform _list;
    [SerializeField] private GameObject _entryPrefab;
    [SerializeField] private SatteliteFactory _satelliteFactory;
}
