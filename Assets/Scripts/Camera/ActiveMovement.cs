using UnityEngine;
using UnityEngine.InputSystem;

public class ActiveMovement : MonoBehaviour
{
    [SerializeField] private float smoothSpeed = 5f; // Smoothing factor
    [SerializeField] private float amount = 10f; // Smoothing factor

    private void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

        // Offset from center, normalized to [-1, 1]
        Vector2 offset = (mousePos - screenCenter);
        offset.x /= Screen.width / 2f;
        offset.y /= Screen.height / 2f;

        // Create a look target in 3D space at z = 10
        Vector3 lookTarget = new Vector3(offset.x, offset.y, amount);

        // Compute target rotation
        Quaternion targetRotation = Quaternion.LookRotation(lookTarget - Camera.main.transform.position);

        // Smoothly rotate towards the target
        Camera.main.transform.rotation = Quaternion.Slerp(
            Camera.main.transform.rotation,
            targetRotation,
            Time.deltaTime * smoothSpeed
        );
    }
}
