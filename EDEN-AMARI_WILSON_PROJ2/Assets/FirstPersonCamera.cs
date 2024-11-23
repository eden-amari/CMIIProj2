using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public float lookSpeedX = 2f;
    public float lookSpeedY = 2f;
    public float upperLimit = -60f;
    public float lowerLimit = 60f;

    private float rotationX = 0f;

    void Update()
    {
        // Get mouse input for looking around
        float mouseX = Input.GetAxis("Mouse X") * lookSpeedX;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeedY;

        // Rotate the player body (Y-axis) based on mouse input
        transform.Rotate(0, mouseX, 0);

        // Rotate the camera (X-axis) based on mouse input
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, upperLimit, lowerLimit); // Limit vertical rotation
        Camera.main.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }
}
