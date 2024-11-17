using UnityEngine;

public class ReticleController : MonoBehaviour
{
    public Camera mainCamera; // Reference to the main camera
    public float maxDistance = 100f; // Maximum distance for the reticle placement

    private void Update()
    {
        UpdateReticlePosition();
    }

    private void UpdateReticlePosition()
    {
        // Create a ray from the camera to where the player is looking
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            // Move the reticle to the hit point
            transform.position = hit.point;
        }
        else
        {
            // If the ray doesn't hit anything, keep it at a default distance
            transform.position = ray.GetPoint(maxDistance);
        }
    }
}
