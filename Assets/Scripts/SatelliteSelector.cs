using UnityEngine;

public class SatelliteSelector : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_output == null) 
            {
                _output = Output.Instance;
            }

            if (_orbitAnimation != null) 
            {
                StopCoroutine(_orbitAnimation);
            }

            ClearLastSelected();
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject.TryGetComponent<SatelliteObject>(out var satellite))
                {
                    _output.Visible = true;
                    _output.Text = satellite.name.ToString();
                    _orbitAnimation = StartCoroutine(satellite.OrbitAnimation(_lineRenderer));
                }
                else if (hit.collider.gameObject.TryGetComponent<MoonBehaviour>(out _))
                {
                    _output.Text = "Moon";
                    _output.Visible = true;
                    _lineRenderer.enabled = false;
                }
            }
            else
            {
                _lineRenderer.enabled = false;                
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

    [SerializeField] private LineRenderer _lineRenderer;
    private Output _output;
    private GameObject _activeOrbit;
    private Coroutine _orbitAnimation;
}
