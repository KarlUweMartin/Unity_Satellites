using UnityEngine;

public class Rotate : MonoBehaviour
{
    void Update()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + _rotationSpeed, transform.eulerAngles.z);
    }

    [SerializeField] private float _rotationSpeed;
}
