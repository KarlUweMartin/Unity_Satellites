using TMPro;
using UnityEngine;

public class SatelliteSelector : MonoBehaviour
{

    private void Start()
    {
        _output = Output.Instance;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClearLastSelected();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject.TryGetComponent<SatelliteObject>(out var satellite))
                {
                    var orbitObj = satellite.DrawFullOrbit();
                    _output.Text = satellite.name.ToString();
                    _activeOrbit = orbitObj;
                    _output.Visible = true;
                }
                else if (hit.collider.gameObject.TryGetComponent<MoonBehaviour>(out var moon))
                {
                    _output.Text = "Moon";
                    _output.Visible = true;
                }
            }
            else
            {
                _output.Visible = false;
            }
        }
    }

    private void ClearLastSelected() 
    {
        if(_activeOrbit != null) 
        {
            Destroy(_activeOrbit);
        }
    }

    private Output _output;
    private GameObject _activeOrbit;
}
