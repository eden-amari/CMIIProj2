

using System.Collections;
using UnityEngine;

public class LaunchProjectile : MonoBehaviour
{
    public Transform launchPoint; // The point from where the grenade will be launched
    public GameObject projectile; // The grenade prefab
    public float launchVelocity = 30f; // Speed of the grenade
    public float maxLaunchHeight = 1.5f; // Maximum height of the arc
    public Camera mainCamera; // Reference to the main camera
    public float launchDuration = 2f; // Duration of the launch
    private PlayerInventory playerInventory; // Reference to the player's inventory
    private bool isLaunching = false; // To prevent multiple launches

    private void Start()
    {
        // Use FindObjectOfType to get the PlayerInventory component from the scene
        playerInventory = FindFirstObjectByType<PlayerInventory>();
        if (playerInventory == null)
        {
            Debug.LogError("PlayerInventory component not found in the scene!");
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !isLaunching) // Check if not already launching
        {
            // Check if the weapon is in the inventory and has the tag "Grenade"
            if (playerInventory != null && playerInventory.currentWeapon != null && playerInventory.currentWeapon.CompareTag("Grenade"))
            {
                StartCoroutine(LaunchGrenade());
            }
            else
            {
                Debug.Log("Grenade not in inventory or current weapon is null!");
            }
        }
    }

    private IEnumerator LaunchGrenade()
    {
        isLaunching = true; // Set launching to true to prevent further launches

        // Raycast to find the target point in the world
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Instantiate the grenade at the launch point's position
            GameObject grenade = Instantiate(projectile, launchPoint.position, Quaternion.identity);

            // Calculate the launch direction towards the hit point
            Vector3 launchDirection = (hit.point - launchPoint.position).normalized;

            // Calculate the initial position
            Vector3 startPosition = launchPoint.position;
            float startY = startPosition.y;

            // Reduced height for a lower arc
            float launchHeight = Mathf.Min(maxLaunchHeight, (hit.point.y - startY) * 0.5f);
            float elapsedTime = 0f;

            while (elapsedTime < launchDuration)
            {
                elapsedTime += Time.deltaTime;

                // Calculate the time factor (0 to 1)
                float t = elapsedTime / launchDuration;

                // Calculate the position in a parabolic path
                float height = Mathf.Sin(t * Mathf.PI) * launchHeight; // Arc height
                Vector3 newPosition = startPosition + launchDirection * launchVelocity * t + Vector3.up * height;

                // Update the grenade position
                grenade.transform.position = newPosition;

                yield return null; // Wait for the next frame
            }

            // Ensure the grenade ends at the target position
            grenade.transform.position = hit.point;

            // Remove the grenade from the player's inventory
            if (playerInventory.currentWeapon != null)
            {
                playerInventory.currentWeapon.transform.SetParent(null); // Detach from player
                playerInventory.RemoveCurrentWeapon(); // Call the method to remove the weapon
            }

            // Optional: Add any explosion or effect here
            // Destroy(grenade); // Uncomment if you want to destroy after launch
        }

        isLaunching = false; // Reset the launching state
    }
}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class LaunchProjectile : MonoBehaviour
//{
//    public Transform launchPoint;
//    public GameObject projectile;
//    public float launchVelocity = 10f;

//    void Update()
//    {
//        if (Input.GetButtonDown("Fire1"))
//        {
//            var _projectile = Instantiate(projectile, launchPoint.position, launchPoint.rotation);
//            _projectile.GetComponent<Rigidbody>().velocity = launchPoint.up * launchVelocity;
//        }
//    }
//}

//using System.Collections;
//using UnityEngine;

//public class LaunchProjectile : MonoBehaviour
//{
//    public Transform launchPoint;
//    public GameObject projectile;
//    public float launchVelocity = 10f;
//    public float flightDuration = 2f; // Adjust this as needed

//    void Update()
//    {
//        if (Input.GetButtonDown("Fire1"))
//        {
//            LaunchGrenade();
//        }
//    }

//    private void LaunchGrenade()
//    {
//        var _projectile = Instantiate(projectile, launchPoint.position, launchPoint.rotation);
//        StartCoroutine(MoveProjectile(_projectile));
//    }

//    private IEnumerator MoveProjectile(GameObject grenade)
//    {
//        Vector3 startPosition = grenade.transform.position;
//        Vector3 targetPosition = startPosition + launchPoint.up * launchVelocity * flightDuration;
//        float elapsedTime = 0f;

//        while (elapsedTime < flightDuration)
//        {
//            grenade.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / flightDuration);
//            elapsedTime += Time.deltaTime;
//            yield return null; // Wait for the next frame
//        }

//        // Optionally destroy the grenade after it has traveled
//        Destroy(grenade); // Or handle explosion logic here
//    }
//}

//using System.Collections;
//using UnityEngine;

//public class LaunchProjectile : MonoBehaviour
//{
//    public Transform launchPoint; // Should point forward
//    public GameObject projectile; // The grenade prefab
//    public float launchVelocity = 10f;
//    public float flightDuration = 2f;

//    void Update()
//    {
//        if (Input.GetButtonDown("Fire1"))
//        {
//            LaunchGrenade();
//        }
//    }

//    private void LaunchGrenade()
//    {
//        // Instantiate the grenade at the launch point's position and rotation
//        var _projectile = Instantiate(projectile, launchPoint.position, launchPoint.rotation);

//        // Adjust the launch direction to be forward relative to the launch point
//        StartCoroutine(MoveProjectile(_projectile, launchPoint.forward));
//    }

//    private IEnumerator MoveProjectile(GameObject grenade, Vector3 direction)
//    {
//        Vector3 startPosition = grenade.transform.position;
//        Vector3 targetPosition = startPosition + direction * launchVelocity * flightDuration;
//        float elapsedTime = 0f;

//        while (elapsedTime < flightDuration)
//        {
//            grenade.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / flightDuration);
//            elapsedTime += Time.deltaTime;
//            yield return null; // Wait for the next frame
//        }

//        Destroy(grenade); // Destroy after flight
//    }
//}

//using System.Collections;
//using UnityEngine;

//public class LaunchProjectile : MonoBehaviour
//{
//    public Transform launchPoint; // The point from where the grenade will be launched
//    public GameObject projectile; // The grenade prefab
//    public float launchVelocity = 10f; // Initial speed of the grenade
//    public float gravity = -9.81f; // Gravity affecting the grenade
//    public float flightDuration = 8f; // Total flight time before the grenade is destroyed

//    void Update()
//    {
//        if (Input.GetButtonDown("Fire1"))
//        {
//            LaunchGrenade();
//        }
//    }

//    private void LaunchGrenade()
//    {
//        // Instantiate the grenade at the launch point's position and rotation
//        var _projectile = Instantiate(projectile, launchPoint.position, launchPoint.rotation);

//        // Launch the grenade with an initial upward velocity
//        Vector3 initialVelocity = launchPoint.forward * launchVelocity + launchPoint.up * (launchVelocity / 2);

//        StartCoroutine(MoveProjectile(_projectile, initialVelocity));
//    }

//    private IEnumerator MoveProjectile(GameObject grenade, Vector3 initialVelocity)
//    {
//        Vector3 position = grenade.transform.position;
//        float elapsedTime = 0f;

//        while (elapsedTime < flightDuration)
//        {
//            // Update the grenade's position based on its initial velocity and gravity
//            float t = elapsedTime / flightDuration;
//            position += initialVelocity * Time.deltaTime; // Move based on initial velocity
//            position.y += gravity * Time.deltaTime; // Apply gravity

//            grenade.transform.position = position;
//            elapsedTime += Time.deltaTime;
//            yield return null; // Wait for the next frame
//        }

//        Destroy(grenade); // Destroy the grenade after flight
//    }
//}

//using System.Collections;
//using UnityEngine;

//public class LaunchProjectile : MonoBehaviour
//{
//    public Transform launchPoint; // The point from where the grenade will be launched
//    public GameObject projectile; // The grenade prefab
//    public float launchVelocity = 10f; // Base speed of the grenade
//    public float maxLaunchHeight = 2f; // Adjusted maximum height of the arc
//    public Camera mainCamera; // Reference to the main camera
//    public float launchDuration = 2f; // Duration of the launch

//    void Update()
//    {
//        if (Input.GetButtonDown("Fire1"))
//        {
//            StartCoroutine(LaunchGrenade());
//        }
//    }

//    private IEnumerator LaunchGrenade()
//    {
//        // Raycast to find the target point in the world
//        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
//        if (Physics.Raycast(ray, out RaycastHit hit))
//        {
//            // Instantiate the grenade at the launch point's position
//            GameObject grenade = Instantiate(projectile, launchPoint.position, Quaternion.identity);

//            // Calculate the launch direction towards the hit point
//            Vector3 launchDirection = (hit.point - launchPoint.position).normalized;

//            // Calculate the initial position
//            Vector3 startPosition = launchPoint.position;
//            float startY = startPosition.y;

//            // Reduced height for a lower arc
//            float launchHeight = Mathf.Min(maxLaunchHeight, (hit.point.y - startY) * 0.5f); // Reduce height factor
//            float gravity = -9.81f; // Simulated gravity
//            float elapsedTime = 0f;

//            while (elapsedTime < launchDuration)
//            {
//                elapsedTime += Time.deltaTime;

//                // Calculate the time factor (0 to 1)
//                float t = elapsedTime / launchDuration;

//                // Calculate the position in a parabolic path
//                float height = Mathf.Sin(t * Mathf.PI) * launchHeight; // Arc height
//                Vector3 newPosition = startPosition + launchDirection * launchVelocity * t + Vector3.up * height;

//                // Update the grenade position
//                grenade.transform.position = newPosition;

//                yield return null; // Wait for the next frame
//            }

//            // Ensure the grenade ends at the target position
//            grenade.transform.position = hit.point;

//            // Optional: Add any explosion or effect here
//            // Destroy(grenade); // Uncomment if you want to destroy after launch
//        }
//    }
//}
//using System.Collections;
//using UnityEngine;

//public class LaunchProjectile : MonoBehaviour
//{
//    public Transform launchPoint; // The point from where the grenade will be launched
//    public GameObject projectile; // The grenade prefab
//    public float launchVelocity = 30f; // Adjusted speed of the grenade
//    public float maxLaunchHeight = 1.5f; // Reduced maximum height of the arc
//    public Camera mainCamera; // Reference to the main camera
//    public float launchDuration = 2f; // Duration of the launch
//    private PlayerInventory playerInventory; // Reference to the player's inventory

//    private void Start()
//    {
//        playerInventory = FindObjectOfType<PlayerInventory>();
//        if (playerInventory == null)
//        {
//            Debug.LogError("PlayerInventory component not found in the scene!");
//        }
//    }

//    void Update()
//    {
//        if (Input.GetButtonDown("Fire1"))
//        {
//            // Check if the weapon is in the inventory and has the tag "Grenade"
//            if (playerInventory != null && playerInventory.currentWeapon != null && playerInventory.currentWeapon.CompareTag("Grenade"))
//            {
//                StartCoroutine(LaunchGrenade());
//            }
//            else
//            {
//                Debug.Log("Grenade not in inventory or current weapon is null!");
//            }
//        }
//    }

//    private IEnumerator LaunchGrenade()
//    {
//        // Raycast to find the target point in the world
//        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
//        if (Physics.Raycast(ray, out RaycastHit hit))
//        {
//            // Instantiate the grenade at the launch point's position
//            GameObject grenade = Instantiate(projectile, launchPoint.position, Quaternion.identity);

//            // Calculate the launch direction towards the hit point
//            Vector3 launchDirection = (hit.point - launchPoint.position).normalized;

//            // Calculate the initial position
//            Vector3 startPosition = launchPoint.position;
//            float startY = startPosition.y;

//            // Reduced height for a lower arc
//            float launchHeight = Mathf.Min(maxLaunchHeight, (hit.point.y - startY) * 0.5f);
//            float elapsedTime = 0f;

//            while (elapsedTime < launchDuration)
//            {
//                elapsedTime += Time.deltaTime;

//                // Calculate the time factor (0 to 1)
//                float t = elapsedTime / launchDuration;

//                // Calculate the position in a parabolic path
//                float height = Mathf.Sin(t * Mathf.PI) * launchHeight; // Arc height
//                Vector3 newPosition = startPosition + launchDirection * launchVelocity * t + Vector3.up * height;

//                // Update the grenade position
//                grenade.transform.position = newPosition;

//                yield return null; // Wait for the next frame
//            }

//            // Ensure the grenade ends at the target position
//            grenade.transform.position = hit.point;

//            // Remove the grenade from the player's inventory
//            if (playerInventory.currentWeapon != null)
//            {
//                playerInventory.currentWeapon.transform.SetParent(null); // Detach from player
//                playerInventory.RemoveCurrentWeapon(); // Call the method to remove the weapon
//            }
//        }
//    }
//}
