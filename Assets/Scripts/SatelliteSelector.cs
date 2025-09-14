using System.Collections.Generic;
using UnityEngine;

public class SatelliteSelector : MonoBehaviour
{

    private void Awake()
    {
        _offsets.Clear();
        var step = _pixelRadius / Mathf.Max(1, GridHalf);
        for (int dy = -GridHalf; dy <= GridHalf; dy++)
        {
            for (int dx = -GridHalf; dx <= GridHalf; dx++)
            {
                var off = new Vector2(dx * step, dy * step);
                if (off.sqrMagnitude <= _pixelRadius * _pixelRadius)
                    _offsets.Add((off, off.sqrMagnitude));
            }
        }
        _offsets.Sort((a, b) => a.r2.CompareTo(b.r2));

        AppControl.OnSatelliteChanged.AddListener((s) => 
        {
            if (s == null) 
            {
                _orbitLine.enabled = false;
            }
        });
    }

    private bool TryPickAroundPointer(Vector2 screenPos, out RaycastHit bestHit)
    {
        bestHit = default;
        float bestDist = float.PositiveInfinity;
        bool gotHit = false;

        if (CastRay(screenPos, out var hitCenter) && hitCenter.distance < bestDist)
        {
            bestHit = hitCenter; bestDist = hitCenter.distance; gotHit = true;
        }

        for (int i = 0; i < _offsets.Count; i++)
        {
            var sp = screenPos + _offsets[i].offset;
            if (CastRay(sp, out var h) && h.distance < bestDist)
            {
                bestHit = h;
                bestDist = h.distance;
                gotHit = true;
            }
        }
        return gotHit;
    }

    private bool CastRay(Vector2 sp, out RaycastHit hit)
    {
        var ray = Camera.main.ScreenPointToRay(sp);
        return Physics.Raycast(ray, out hit, Mathf.Infinity, _hitMask, QueryTriggerInteraction.Ignore);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_output == null) _output = Output.Instance;

            if (TryPickAroundPointer(Input.mousePosition, out var hit))
            {
                if (_orbitAnimation != null) StopCoroutine(_orbitAnimation);
                ClearLastSelected();

                if (hit.collider.gameObject.TryGetComponent<SatelliteObject>(out var satellite))
                {
                    AppControl.SelectedSatellite = satellite;

                    _orbitAnimation = StartCoroutine(satellite.OrbitAnimation(_orbitLine));
                    satellite.Select(true);
                    satellite.transform.localScale = Vector3.one * .02f;
                    satellite.GetComponent<BoxCollider>().enabled = false;
                }
                else if (hit.collider.gameObject.TryGetComponent<MoonBehaviour>(out _))
                {
                    _output.Text = "Moon";
                    _output.Visible = true;
                    _orbitLine.enabled = false;
                }
            }           
        }
    }

    private void ClearLastSelected() 
    {
        var selection = AppControl.SelectedSatellite;

        if (selection != null) 
        {
            selection.Select(false);
            selection.transform.localScale = Vector3.one * .01f;
            selection.GetComponent<BoxCollider>().enabled = true;
        }
    }

    [SerializeField] private LineRenderer _orbitLine;
    [SerializeField] private float _pixelRadius = 8f;
    [SerializeField] private LayerMask _hitMask = ~0;
    readonly List<(Vector2 offset, float r2)> _offsets = new();

    private Output _output;
    private Coroutine _orbitAnimation;
    const int GridHalf = 2;
}
