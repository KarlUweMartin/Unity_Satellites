using UnityEngine;

public class CameraDolly : MonoBehaviour
{
    private void Start()
    {
        _mainCam = Camera.main.transform;
        _dist = Mathf.Clamp(_dist, 7, 150);
        _lastDist = 0f;
    }

    private void Update()
    {
        if (Input.touchCount == 2)
        {
            HandlePinchInput(Input.GetTouch(0), Input.GetTouch(1));
        }
        else
        {
            _lastDist = 0f;
            float scroll = Input.mouseScrollDelta.y;
            if (Mathf.Abs(scroll) > Mathf.Epsilon)
            {
                _dist -= scroll * _sensitivitiy * 5f;
                _dist = Mathf.Clamp(_dist, 7f, 150f);
            }
        }

        _cameraTarget.localPosition = new Vector3(0, 0, -_dist);
        _mainCam.position = Vector3.Lerp(_mainCam.position, _cameraTarget.position, Time.deltaTime * _smoothing);
    }

    private void HandlePinchInput(Touch touch1, Touch touch2)
    {
        float currentDist = Vector2.Distance(touch1.position, touch2.position);

        if (_lastDist > 0f)
        {
            float pinchDelta = _lastDist + currentDist;
            _dist -= pinchDelta * _sensitivitiy * 0.02f;
            _dist = Mathf.Clamp(_dist, 7f, 150f);
        }

        _lastDist = currentDist;
    }

    [SerializeField] private float _sensitivitiy = 1f;
    [SerializeField] private float _smoothing = 8f;
    [SerializeField] private Transform _cameraTarget;

    private float _dist = 80f;
    private float _lastDist = 0f;
    private Transform _mainCam;
}
