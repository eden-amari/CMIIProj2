using System.Collections;
using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    // Gun variables
    public Transform bulletSpawnPoint; // The point where the bullet spawns
    public GameObject bulletPrefab; // The bullet prefab
    public float bulletSpeed = 30f; // Speed of the bullet
    public int bulletCount = 6; // Initial bullet count

    // Grenade variables
    public Transform launchPoint; // The point from where the grenade will be launched
    public GameObject grenadePrefab; // The grenade prefab
    public float grenadeSpeed = 10f; // Speed of the grenade
    public float maxLaunchHeight = 1.5f; // Maximum height of the arc
    public Camera mainCamera; // Reference to the main camera
    public float launchDuration = 2f; // Duration of the launch
    public int grenadeCount = 0; // Initial grenade count

    private PlayerInventory playerInventory; // Reference to the player's inventory
    //private bool isLaunching = false; // To prevent multiple grenade launches

    // TextMesh Pro references
    public TextMeshProUGUI bulletCountText; // Reference to the TextMeshProUGUI component
    public TextMeshProUGUI grenadeCountText; // Reference to the grenade count text

    private void Start()
    {
        // Get the PlayerInventory component attached to the player
        playerInventory = FindFirstObjectByType<PlayerInventory>();
        UpdateBulletCountText(); // Initialize the bullet count text
        UpdateGrenadeCountText(); // Initialize the grenade count text
    }

    private void Update()
    {
        if ((playerInventory.currentWeapon != this) && (this.tag == "Gun"))
        {
            Vector3 rotationToAdd = new Vector3(0, 0, .5f);
            transform.Rotate(rotationToAdd);
        }

        else if ((playerInventory.currentWeapon != this) && (this.tag == "GrenadeLauncher"))
        {
            Vector3 rotationToAdd = new Vector3(0, .5f, 0);
            transform.Rotate(rotationToAdd);
        }

        // Check if the weapon is active and the current weapon in inventory before allowing firing
        else if (playerInventory.currentWeapon == this && gameObject.activeSelf)
        {
            Debug.Log($"Current Weapon: {this.name}, Grenade Count: {grenadeCount}"); // Log the weapon name and grenade count

            if (gameObject.CompareTag("Gun") && Input.GetMouseButtonDown(0)) 
            {
                if (bulletCount > 0) 
                {
                    Debug.Log("Firing Gun!");
                    FireGun();
                }
                else
                {
                    Debug.Log("No bullets left!");
                }
            }
            else if (gameObject.CompareTag("GrenadeLauncher") && Input.GetMouseButtonDown(0))
            {
                if (grenadeCount > 0)
                {
                    Debug.Log("Firing Grenade!");
                    FireProjectile(grenadePrefab, launchPoint, grenadeSpeed); // Correct function call
                    grenadeCount--; // Decrease grenade count
                    UpdateGrenadeCountText(); // Update the text display
                }
                else
                {
                    Debug.Log("No grenades left!");
                }
            }
        }
    }

    // Fire the gun projectile
    private void FireGun()
    {
        ShootGun(bulletPrefab, bulletSpawnPoint, bulletSpeed);
        bulletCount--; // Decrease the bullet count after firing
        UpdateBulletCountText(); // Update the text display
    }

    // Fire the grenade projectile with a parabolic arc
    //private IEnumerator LaunchGrenade()
    //{
    //    isLaunching = true; // Set launching to true to prevent further launches

    //    // Raycast to find the target point in the world
    //    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
    //    if (Physics.Raycast(ray, out RaycastHit hit))
    //    {
    //        // Instantiate the grenade at the launch point's position
    //        GameObject grenade = Instantiate(grenadePrefab, launchPoint.position, Quaternion.identity);

    //        // Calculate the launch direction towards the hit point
    //        Vector3 launchDirection = (hit.point - launchPoint.position).normalized;

    //        // Calculate the initial position
    //        Vector3 startPosition = launchPoint.position;
    //        float startY = startPosition.y;

    //        // Reduced height for a lower arc
    //        float launchHeight = Mathf.Min(maxLaunchHeight, (hit.point.y - startY) * 0.5f);
    //        float elapsedTime = 0f;

    //        while (elapsedTime < launchDuration)
    //        {
    //            elapsedTime += Time.deltaTime;

    //            // Calculate the time factor (0 to 1)
    //            float t = elapsedTime / launchDuration;

    //            // Calculate the position in a parabolic path
    //            float height = Mathf.Sin(t * Mathf.PI) * launchHeight; // Arc height
    //            Vector3 newPosition = startPosition + launchDirection * grenadeSpeed * t + Vector3.up * height;

    //            // Update the grenade position
    //            grenade.transform.position = newPosition;

    //            yield return null; // Wait for the next frame
    //        }

    //        // Ensure the grenade ends at the target position
    //        grenade.transform.position = hit.point;

    //        // Optional: Add any explosion or effect here
    //        // Destroy(grenade); // Uncomment if you want to destroy after launch
    //    }

    //    isLaunching = false; // Reset the launching state
    //    grenadeCount--; // Decrease the grenade count after firing
    //    UpdateGrenadeCountText(); // Update the text display
    //}

    // Fire a projectile (bullet or grenade) with basic force
    private GameObject ShootGun(GameObject prefab, Transform spawnPoint, float speed)
    {
        Quaternion rotation = spawnPoint.rotation;
        GameObject projectile = Instantiate(prefab, spawnPoint.position, rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(rotation * Vector3.forward * speed, ForceMode.VelocityChange);
        }
        else
        {
            Debug.LogError($"{prefab.name} prefab does not have a Rigidbody component!");
        }

        return projectile;
    }

    private void FireProjectile(GameObject prefab, Transform spawnPoint, float speed)
    {
        // Instantiate the projectile at the spawn point with no rotation, or you can use spawnPoint.rotation if you want to inherit rotation
        Quaternion rotation = spawnPoint.rotation;
        GameObject projectile = Instantiate(prefab, spawnPoint.position, rotation);

        // Get the Rigidbody component from the instantiated projectile
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Add force to the projectile to launch it forward
            rb.AddForce(rotation * Vector3.forward * speed, ForceMode.VelocityChange);
        }
        else
        {
            Debug.LogError($"{prefab.name} prefab does not have a Rigidbody component!");
        }

        // Optionally, destroy the projectile after a delay, for example, after 5 seconds
        Destroy(projectile, 1f); // Destroy the projectile after 5 seconds

        // No need for a return here, since the method is void.
    }


    //private GameObject FireProjectile(GameObject prefab, Transform spawnPoint, float speed)
    //{
    //    // Instantiate the projectile at the spawn point with no rotation, or you can use spawnPoint.rotation if you want to inherit rotation
    //    GameObject projectile = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

    //    // Get the Rigidbody of the projectile
    //    Rigidbody rb = projectile.GetComponent<Rigidbody>();

    //    if (rb != null)
    //    {
    //        // Apply force to the grenade in the direction the spawn point is facing
    //        Vector3 launchDirection = spawnPoint.forward; // Use the forward direction of the spawn point

    //        // Optionally, modify launch direction towards mouse or aim, but start with spawn point forward direction
    //        // Vector3 launchDirection = (hit.point - spawnPoint.position).normalized; // Uncomment if you want to aim at the hit point instead of spawn point's forward direction

    //        // Add force in the forward direction of the spawn point
    //        rb.AddForce(launchDirection * speed, ForceMode.VelocityChange);
    //    }
    //    else
    //    {
    //        Debug.LogError($"{prefab.name} prefab does not have a Rigidbody component!");
    //    }

    //    return projectile;
    //}

    //private GameObject FireProjectile(GameObject prefab, Transform spawnPoint, float speed)
    //{
    //    // Calculate the direction from the spawn point to the mouse position
    //    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition); // Use the camera's screen point to raycast to the world
    //    RaycastHit hit;

    //    if (Physics.Raycast(ray, out hit)) // If the ray hits something
    //    {
    //        // Calculate the direction from the spawn point to the hit point
    //        Vector3 launchDirection = (hit.point - spawnPoint.position).normalized;

    //        // Instantiate the projectile
    //        GameObject projectile = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

    //        // Get the Rigidbody of the projectile
    //        Rigidbody rb = projectile.GetComponent<Rigidbody>();

    //        if (rb != null)
    //        {
    //            // Add force to the grenade in the direction of the target, multiplied by the speed
    //            rb.AddForce(launchDirection * speed, ForceMode.VelocityChange);
    //        }
    //        else
    //        {
    //            Debug.LogError($"{prefab.name} prefab does not have a Rigidbody component!");
    //        }

    //        return projectile;
    //    }

    //    return null; // If no hit, don't launch the grenade
    //}


    //private GameObject LaunchProjectile(GameObject prefab, Transform spawnPoint, float speed)
    //{
    //    // Calculate the direction from the spawn point to the mouse position
    //    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition); // Use the camera's screen point to raycast to the world
    //    RaycastHit hit;

    //    if (Physics.Raycast(ray, out hit)) // If the ray hits something
    //    {
    //        // Calculate the direction from the spawn point to the hit point
    //        Vector3 launchDirection = (hit.point - spawnPoint.position).normalized;

    //        // Instantiate the projectile
    //        GameObject projectile = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

    //        // Get the Rigidbody of the projectile
    //        Rigidbody rb = projectile.GetComponent<Rigidbody>();

    //        if (rb != null)
    //        {
    //            // Add force to the grenade in the direction of the target, multiplied by the speed
    //            rb.AddForce(launchDirection * speed, ForceMode.VelocityChange);
    //        }
    //        else
    //        {
    //            Debug.LogError($"{prefab.name} prefab does not have a Rigidbody component!");
    //        }

    //        return projectile;
    //    }

    //    return null; // If no hit, don't launch the grenade
    //}

    // Update the bullet count UI text
    public void UpdateBulletCountText()
    {
        if (bulletCountText != null)
        {
            bulletCountText.text = "Bullets: " + bulletCount; // Update the TextMeshPro text
        }
    }

    // Update the grenade count UI text
    public void UpdateGrenadeCountText()
    {
        if (grenadeCountText != null)
        {
            grenadeCountText.text = "Grenades in the launcher: " + grenadeCount;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If the player collides with the grenade, add to the grenade count and destroy the grenade object
        if (collision.gameObject.CompareTag("Grenade"))
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                grenadeCount++; // Add one grenade to the player's inventory
                UpdateGrenadeCountText(); // Update the displayed grenade count
                Destroy(collision.gameObject); // Destroy the grenade, not the weapon
                Debug.Log("Grenade collected! Total grenades: " + grenadeCount);
            }
        }

        // If the weapon collides with the player, equip the weapon (do not destroy it)
        if (collision.gameObject.CompareTag("Player") && gameObject.CompareTag("Weapon"))
        {
            Equip(); // Equip the weapon if it's the right one
            Debug.Log("Weapon Equipped: " + gameObject.name);
        }
    }

    // Equip the weapon to the player
    public void Equip()
    {
        gameObject.SetActive(true); // Activate the weapon when equipped
    }

    // Unequip the weapon (if needed)
    public void Unequip()
    {
        gameObject.SetActive(false); // Deactivate the weapon when unequipped
    }
}


//using UnityEngine;
//using TMPro;

//public class Weapon : MonoBehaviour
//{
//    // Gun variables
//    public Transform bulletSpawnPoint; // The point where the bullet spawns
//    public GameObject bulletPrefab; // The bullet prefab
//    public float bulletSpeed = 10f; // Speed of the bullet
//    public int bulletCount = 6; // Initial bullet count

//    // Grenade variables
//    public Transform launchPoint; // The point where the grenade is launched from
//    public GameObject projectile; // The grenade prefab
//    public float grenadeSpeed = 10f; // Speed of the grenade
//    private PlayerInventory playerInventory; // Reference to the player's inventory
//    public int grenadeCount = 0; // Initial grenade count

//    // TextMesh Pro reference
//    public TextMeshProUGUI bulletCountText; // Reference to the TextMeshProUGUI component
//    public TextMeshProUGUI grenadeCountText; // Reference to the grenade count text

//    private void Start()
//    {

//        // Get the PlayerInventory component attached to the player
//        playerInventory = FindObjectOfType<PlayerInventory>();
//        UpdateBulletCountText(); // Initialize the bullet count text
//        UpdateGrenadeCountText(); // Initialize the grenade count text
//    }

//    public void UpdateBulletCountText()
//    {
//        if (bulletCountText != null)
//        {
//            bulletCountText.text = "Bullets: " + bulletCount; // Update the TextMeshPro text
//        }
//    }

//    public void UpdateGrenadeCountText()
//    {
//        if (grenadeCountText != null)
//        {
//            grenadeCountText.text = "Grenades in the launcher: " + grenadeCount;
//            // Add a log to confirm the text update
//            Debug.Log("Grenade count text updated to: " + grenadeCount);
//        }
//    }


//    private void Update()
//    {
//        // Check if the weapon is active and the current weapon in inventory before allowing firing
//        if (playerInventory.currentWeapon == this && gameObject.activeSelf)
//        {
//            Debug.Log($"Current Weapon: {this.name}, Grenade Count: {grenadeCount}"); // Log the weapon name and grenade count

//            if (gameObject.CompareTag("Gun") && Input.GetMouseButtonDown(0)) // Use GetMouseButtonDown for single shot
//            {
//                if (bulletCount > 0) // Check if there are bullets left
//                {
//                    Debug.Log("Firing Gun!");
//                    FireGun();
//                }
//                else
//                {
//                    Debug.Log("No bullets left!");
//                }
//            }
//            else if (gameObject.CompareTag("GrenadeLauncher") && Input.GetMouseButtonDown(0))
//            {
//                if (grenadeCount > 0)
//                {
//                    Debug.Log("Firing Grenade!");
//                    FireGrenade();
//                }
//                else
//                {
//                    Debug.Log("No grenades left!");
//                }
//            }
//        }
//    }

//    private void FireGun()
//    {
//        FireProjectile(bulletPrefab, bulletSpawnPoint, bulletSpeed);
//        bulletCount--; // Decrease the bullet count after firing
//        UpdateBulletCountText(); // Update the text display
//    }

//    private void FireGrenade()
//    {
//        FireProjectile(projectile, launchPoint, grenadeSpeed);
//        grenadeCount--; // Decrease the grenade count after firing
//        UpdateGrenadeCountText(); // Update the text display
//    }

//    //private GameObject FireProjectile(GameObject prefab, Transform spawnPoint, float speed)
//    //{
//    //    Quaternion rotation = spawnPoint.rotation * Quaternion.Euler(0, 0, 90);
//    //    GameObject projectile = Instantiate(prefab, spawnPoint.position, rotation);
//    //    Rigidbody rb = projectile.GetComponent<Rigidbody>();

//    //    if (rb != null)
//    //    {
//    //        rb.linearVelocity = rotation * Vector3.forward * speed;
//    //    }
//    //    else
//    //    {
//    //        Debug.LogError($"{prefab.name} prefab does not have a Rigidbody component!");
//    //    }

//    //    return projectile;
//    //}

//    private GameObject FireProjectile(GameObject prefab, Transform spawnPoint, float speed)
//    {
//        // Apply a slight upward angle for the grenade to travel upwards at a reasonable arc
//        Quaternion rotation = spawnPoint.rotation * Quaternion.Euler(0, 0, 90);
//        GameObject projectile = Instantiate(prefab, spawnPoint.position, rotation);
//        Rigidbody rb = projectile.GetComponent<Rigidbody>();

//        if (rb != null)
//        {
//            // Apply force in the forward direction
//            rb.AddForce(rotation * Vector3.forward * speed, ForceMode.VelocityChange);
//        }
//        else
//        {
//            Debug.LogError($"{prefab.name} prefab does not have a Rigidbody component!");
//        }

//        return projectile;
//    }


//    private void OnCollisionEnter(Collision collision)
//    {
//        if (gameObject.CompareTag("Grenade"))
//        {
//            if (collision.gameObject.CompareTag("Player"))
//            {
//                grenadeCount++;
//                UpdateGrenadeCountText(); // Update the displayed grenade count
//                Destroy(gameObject);
//                Debug.Log("Grenade collected! Total grenades: " + grenadeCount);
//            }
//        }
//    }

//    public void Equip()
//    {
//        gameObject.SetActive(true); // Activate the weapon when equipped
//    }

//    public void Unequip()
//    {
//        gameObject.SetActive(false); // Deactivate the weapon when unequipped
//    }
//}

//------------------
//using UnityEngine;
//using TMPro;

//public class Weapon : MonoBehaviour
//{
//    // Gun variables
//    public Transform bulletSpawnPoint; // The point where the bullet spawns
//    public GameObject bulletPrefab; // The bullet prefab
//    public float bulletSpeed = 10f; // Speed of the bullet
//    public int bulletCount = 6; // Initial bullet count



//    // Grenade variables
//    public Transform launchPoint; // The point where the grenade is launched from
//    public GameObject projectile; // The grenade prefab
//    public float grenadeSpeed = 10f; // Speed of the grenade
//    private PlayerInventory playerInventory; // Reference to the player's inventory
//    public int grenadeCount = 0;
//    // TextMesh Pro reference
//    public TextMeshProUGUI bulletCountText; // Reference to the TextMeshProUGUI component
//    public TextMeshProUGUI grenadeCountText; // Reference to the grenade count text

//    private void Start()
//    {
//        // Get the PlayerInventory component attached to the player
//        playerInventory = FindObjectOfType<PlayerInventory>();
//        UpdateBulletCountText(); // Initialize the bullet count text
//        UpdateGrenadeCountText(); // Initialize the grenade count text
//    }

//    public void UpdateBulletCountText()
//    {
//        if (bulletCountText != null)
//        {
//            bulletCountText.text = "Bullets: " + bulletCount; // Update the TextMeshPro text
//        }
//    }

//    public void UpdateGrenadeCountText()
//    {
//        if (grenadeCountText != null)
//        {
//            grenadeCountText.text = "Grenades in the launcher: " + grenadeCount; // Update the TextMeshPro text
//        }
//    }

//    private void Update()
//    {
//        // Check if the weapon is active and the current weapon in inventory before allowing firing
//        if (playerInventory.currentWeapon == this && gameObject.activeSelf)
//        {
//            Debug.Log($"Current Weapon: {playerInventory.currentWeapon}, Grenade Count: {grenadeCount}");

//            if (gameObject.CompareTag("Gun") && Input.GetMouseButtonDown(0)) // Use GetMouseButtonDown for single shot
//            {
//                if (bulletCount > 0) // Check if there are bullets left
//                {
//                    Debug.Log("Firing Gun!");
//                    FireGun();
//                }
//                else
//                {
//                    Debug.Log("No bullets left!");
//                }
//            }
//            else if (gameObject.CompareTag("GrenadeLauncher") && Input.GetMouseButtonDown(0))
//            {   

//                if (grenadeCount > 0)
//                {
//                    Debug.Log("Firing Grenade!");
//                    FireGrenade();
//                }
//                else
//                {
//                    Debug.Log("No grenades left!");
//                }
//            }
//        }
//    }

//    private void FireGun()
//    {
//        FireProjectile(bulletPrefab, bulletSpawnPoint, bulletSpeed);
//        bulletCount--; // Decrease the bullet count after firing
//        UpdateBulletCountText(); // Update the text display
//    }

//    private void FireGrenade()
//    {
//        FireProjectile(projectile, launchPoint, grenadeSpeed);
//        grenadeCount--; // Decrease the grenade count after firing
//        UpdateGrenadeCountText(); // Update the text display
//    }

//    private GameObject FireProjectile(GameObject prefab, Transform spawnPoint, float speed)
//    {
//        Quaternion rotation = spawnPoint.rotation * Quaternion.Euler(0, 0, 90);
//        GameObject projectile = Instantiate(prefab, spawnPoint.position, rotation);
//        Rigidbody rb = projectile.GetComponent<Rigidbody>();

//        if (rb != null)
//        {
//            rb.velocity = rotation * Vector3.forward * speed;
//        }
//        else
//        {
//            Debug.LogError($"{prefab.name} prefab does not have a Rigidbody component!");
//        }

//        return projectile;
//    }

//    private void OnCollisionEnter(Collision collision)
//    {
//        // Check if this object is a grenade
//        if (gameObject.CompareTag("Grenade"))
//        {
//            // Check if the other object is the player
//            if (collision.gameObject.CompareTag("Player"))
//            {
//                // Increment grenade count
//                grenadeCount++; // Use the property to increment grenade count
//                Destroy(gameObject); // Destroy the grenade object
//                Debug.Log("Grenade collected! Total grenades: " + grenadeCount);
//            }
//        }
//    }

//    public void Equip()
//    {
//        gameObject.SetActive(true); // Activate the weapon when equipped
//    }

//    public void Unequip()
//    {
//        gameObject.SetActive(false); // Deactivate the weapon when unequipped
//    }
////}
//using UnityEngine;
//using TMPro;

//public class Weapon : MonoBehaviour
//{
//    // Gun variables
//    public Transform bulletSpawnPoint; // The point where the bullet spawns
//    public GameObject bulletPrefab; // The bullet prefab
//    public float bulletSpeed = 10f; // Speed of the bullet
//    public int bulletCount = 6; // Initial bullet count

//    // Grenade variables
//    public Transform launchPoint; // The point where the grenade is launched from
//    public GameObject projectile; // The grenade prefab
//    public float grenadeSpeed = 10f; // Speed of the grenade
//    private PlayerInventory playerInventory; // Reference to the player's inventory
//    public int grenadeCount = 0; // Initial grenade count

//    // TextMesh Pro reference
//    public TextMeshProUGUI bulletCountText; // Reference to the TextMeshProUGUI component
//    public TextMeshProUGUI grenadeCountText; // Reference to the grenade count text

//    private void Start()
//    {
//        // Get the PlayerInventory component attached to the player
//        playerInventory = FindObjectOfType<PlayerInventory>();
//        UpdateBulletCountText(); // Initialize the bullet count text
//        UpdateGrenadeCountText(); // Initialize the grenade count text
//    }

//    public void UpdateBulletCountText()
//    {
//        if (bulletCountText != null)
//        {
//            bulletCountText.text = "Bullets: " + bulletCount; // Update the TextMeshPro text
//        }
//    }

//    public void UpdateGrenadeCountText()
//    {
//        if (grenadeCountText != null)
//        {
//            grenadeCountText.text = "Grenades in the launcher: " + grenadeCount; // Update the TextMeshPro text
//        }
//    }

//    private void Update()
//    {
//        // Check if the weapon is active and the current weapon in inventory before allowing firing
//        if (playerInventory.currentWeapon == this && gameObject.activeSelf)
//        {
//            Debug.Log($"Current Weapon: {playerInventory.currentWeapon}, Grenade Count: {grenadeCount}");

//            if (gameObject.CompareTag("Gun") && Input.GetMouseButtonDown(0)) // Use GetMouseButtonDown for single shot
//            {
//                if (bulletCount > 0) // Check if there are bullets left
//                {
//                    Debug.Log("Firing Gun!");
//                    FireGun();
//                }
//                else
//                {
//                    Debug.Log("No bullets left!");
//                }
//            }
//            else if (gameObject.CompareTag("GrenadeLauncher") && Input.GetMouseButtonDown(0))
//            {
//                if (grenadeCount > 0)
//                {
//                    Debug.Log("Firing Grenade!");
//                    FireGrenade();
//                }
//                else
//                {
//                    Debug.Log("No grenades left!");
//                }
//            }
//        }
//    }

//    private void FireGun()
//    {
//        FireProjectile(bulletPrefab, bulletSpawnPoint, bulletSpeed);
//        bulletCount--; // Decrease the bullet count after firing
//        UpdateBulletCountText(); // Update the text display
//    }

//    private void FireGrenade()
//    {
//        FireProjectile(projectile, launchPoint, grenadeSpeed);
//        grenadeCount--; // Decrease the grenade count after firing
//        UpdateGrenadeCountText(); // Update the text display
//    }

//    private GameObject FireProjectile(GameObject prefab, Transform spawnPoint, float speed)
//    {
//        Quaternion rotation = spawnPoint.rotation * Quaternion.Euler(0, 0, 90);
//        GameObject projectile = Instantiate(prefab, spawnPoint.position, rotation);
//        Rigidbody rb = projectile.GetComponent<Rigidbody>();

//        if (rb != null)
//        {
//            rb.velocity = rotation * Vector3.forward * speed;
//        }
//        else
//        {
//            Debug.LogError($"{prefab.name} prefab does not have a Rigidbody component!");
//        }

//        return projectile;
//    }

//    private void OnCollisionEnter(Collision collision)
//    {
//        // Check if this object is a grenade
//        if (gameObject.CompareTag("Grenade"))
//        {
//            // Check if the other object is the player
//            if (collision.gameObject.CompareTag("Player"))
//            {
//                // Increment grenade count
//                grenadeCount++; // Increment grenade count
//                UpdateGrenadeCountText(); // Update the displayed grenade count
//                Destroy(gameObject); // Destroy the grenade object
//                Debug.Log("Grenade collected! Total grenades: " + grenadeCount);
//            }
//        }
//    }

//    public void Equip()
//    {
//        gameObject.SetActive(true); // Activate the weapon when equipped
//    }

//    public void Unequip()
//    {
//        gameObject.SetActive(false); // Deactivate the weapon when unequipped
//    }
//}


//public class Weapon : MonoBehaviour // Inherits from MonoBehaviour
//{
//    // Gun variables
//    public Transform bulletSpawnPoint; // The point where the bullet spawns
//    public GameObject bulletPrefab; // The bullet prefab
//    public float bulletSpeed = 10f; // Speed of the bullet
//    public int bulletCount = 6; // Initial bullet count

//    [SerializeField] private int grenadeCount = 1; // Initial grenade count

//    // Grenade variables
//    public Transform launchPoint; // The point where the grenade is launched from
//    public GameObject projectile; // The grenade prefab
//    public float grenadeSpeed = 10f; // Speed of the grenade
//    private PlayerInventory playerInventory; // Reference to the player's inventory

//    // TextMesh Pro reference
//    public TextMeshProUGUI bulletCountText; // Reference to the TextMeshProUGUI component
//    public TextMeshProUGUI grenadeCountText;

//    private void Start()
//    {
//        // Get the PlayerInventory component attached to the player
//        playerInventory = FindObjectOfType<PlayerInventory>();
//        UpdateBulletCountText(); // Initialize the bullet count text
//        UpdateGrenadeCountText(); // Initialize the grenade count text
//    }

//    public int GrenadeCount
//    {
//        get { return grenadeCount; }
//        set
//        {
//            grenadeCount = value;
//            UpdateGrenadeCountText(); // Update the UI whenever the value is set
//        }
//    }

//    public void UpdateBulletCountText()
//    {
//        if (bulletCountText != null)
//        {
//            bulletCountText.text = "Bullets: " + bulletCount; // Update the TextMeshPro text
//        }
//    }

//    public void UpdateGrenadeCountText()
//    {
//        if (grenadeCountText != null)
//        {
//            grenadeCountText.text = "Grenades in the launcher: " + grenadeCount; // Update the TextMeshPro text
//        }
//    }

//    private void Update()
//    {
//        // Check if the weapon is active and the current weapon in inventory before allowing firing
//        if (playerInventory.currentWeapon == this && gameObject.activeSelf)
//        {
//            Debug.Log($"Current Weapon: {playerInventory.currentWeapon}, Grenade Count: {grenadeCount}");

//            if (gameObject.CompareTag("Gun") && Input.GetMouseButtonDown(0)) // Use GetMouseButtonDown for single shot
//            {
//                if (bulletCount > 0) // Check if there are bullets left
//                {
//                    Debug.Log("Firing Gun!");
//                    FireGun();
//                }
//                else
//                {
//                    Debug.Log("No bullets left!");
//                }
//            }
//            else if (gameObject.CompareTag("GrenadeLauncher") && Input.GetMouseButtonDown(0))
//            {
//                if (grenadeCount > 0)
//                {
//                    Debug.Log("Firing Grenade!");
//                    FireGrenade();
//                }
//                else
//                {
//                    Debug.Log("No grenades left!");
//                }
//            }
//        }
//    }

//    private void FireGun()
//    {
//        FireProjectile(bulletPrefab, bulletSpawnPoint, bulletSpeed);
//        bulletCount--; // Decrease the bullet count after firing
//        UpdateBulletCountText(); // Update the text display
//    }

//    private void FireGrenade()
//    {
//        FireProjectile(projectile, launchPoint, grenadeSpeed);
//        grenadeCount--; // Decrease the grenade count after firing
//        UpdateGrenadeCountText(); // Update the text display
//    }

//    private GameObject FireProjectile(GameObject prefab, Transform spawnPoint, float speed)
//    {
//        Quaternion rotation = spawnPoint.rotation * Quaternion.Euler(0, 0, 90);
//        GameObject projectile = Instantiate(prefab, spawnPoint.position, rotation);
//        Rigidbody rb = projectile.GetComponent<Rigidbody>();

//        if (rb != null)
//        {
//            rb.velocity = rotation * Vector3.forward * speed;
//        }
//        else
//        {
//            Debug.LogError($"{prefab.name} prefab does not have a Rigidbody component!");
//        }

//        return projectile;
//    }

//    private void OnCollisionEnter(Collision collision)
//    {
//        // Check if this object is a grenade
//        if (gameObject.CompareTag("Grenade"))
//        {
//            // Check if the other object is the player
//            if (collision.gameObject.CompareTag("Player"))
//            {
//                // Increment grenade count
//                GrenadeCount++; // Use the property to increment grenade count

//                // Destroy the grenade object
//                Destroy(gameObject);

//                Debug.Log("Grenade collected! Total grenades: " + GrenadeCount);
//            }
//        }
//    }

//    public void Equip()
//    {
//        gameObject.SetActive(true); // Activate the weapon when equipped
//    }

//    public void Unequip()
//    {
//        gameObject.SetActive(false); // Deactivate the weapon when unequipped
//    }
//}

//public class Weapon : MonoBehaviour // Inherits from MonoBehaviour
//{
//    // Gun variables
//    public Transform bulletSpawnPoint; // The point where the bullet spawns
//    public GameObject bulletPrefab; // The bullet prefab
//    public float bulletSpeed = 10f; // Speed of the bullet
//    public int bulletCount = 6; // Initial bullet count

//    [SerializeField] public int grenadeCount = 1; // Initial grenade count (private)

//    // Grenade variables
//    public Transform launchPoint; // The point where the grenade is launched from
//    public GameObject projectile; // The grenade prefab
//    public float grenadeSpeed = 10f; // Speed of the grenade
//    private PlayerInventory playerInventory; // Reference to the player's inventory

//    // TextMesh Pro reference
//    public TextMeshProUGUI bulletCountText; // Reference to the TextMeshProUGUI component
//    public TextMeshProUGUI grenadeCountText;

//    private void Start()
//    {
//        // Get the PlayerInventory component attached to the player
//        playerInventory = FindObjectOfType<PlayerInventory>();
//        UpdateBulletCountText(); // Initialize the bullet count text
//        UpdateGrenadeCountText(); // Initialize the grenade count text
//    }

//    // Public property to access grenadeCount
//    public int GrenadeCount
//    {
//        get { return grenadeCount; }
//        set
//        {
//            grenadeCount = 1;
//            UpdateGrenadeCountText(); // Update the UI whenever the value is set
//        }
//    }

//    public void UpdateBulletCountText()
//    {
//        if (bulletCountText != null)
//        {
//            bulletCountText.text = "Bullets: " + bulletCount; // Update the TextMeshPro text
//        }
//    }

//    public void UpdateGrenadeCountText()
//    {
//        if (grenadeCountText != null)
//        {
//            grenadeCountText.text = "Grenades in the launcher: " + grenadeCount; // Update the TextMeshPro text
//        }
//    }


//    private void Update()
//    {
//        // Check if the weapon is active and the current weapon in inventory before allowing firing
//        if (playerInventory.currentWeapon == this && gameObject.activeSelf)
//        {
//            Debug.Log($"Current Weapon: {playerInventory.currentWeapon}, Grenade Count: {grenadeCount}");

//            if (gameObject.CompareTag("Gun") && Input.GetMouseButtonDown(0)) // Use GetMouseButtonDown for single shot
//            {
//                if (bulletCount > 0) // Check if there are bullets left
//                {
//                    Debug.Log("Firing Gun!");
//                    FireGun();
//                }
//                else
//                {
//                    Debug.Log("No bullets left!");
//                }
//            }
//            else if (gameObject.CompareTag("GrenadeLauncher") && Input.GetMouseButtonDown(0))
//            {
//                if (grenadeCount > 0)
//                {
//                    Debug.Log("Firing Grenade!");
//                    FireGrenade();
//                }
//                else
//                {
//                    Debug.Log("No grenades left!");
//                }
//            }
//        }
//    }

//    private void FireGun()
//    {
//        FireProjectile(bulletPrefab, bulletSpawnPoint, bulletSpeed);
//        bulletCount--; // Decrease the bullet count after firing
//        UpdateBulletCountText(); // Update the text display
//    }

//    private void FireGrenade()
//    {
//        FireProjectile(projectile, launchPoint, grenadeSpeed);
//        grenadeCount--; // Decrease the grenade count after firing
//        UpdateGrenadeCountText(); // Update the text display
//    }

//    private GameObject FireProjectile(GameObject prefab, Transform spawnPoint, float speed)
//    {
//        Quaternion rotation = spawnPoint.rotation * Quaternion.Euler(0, 0, 90);
//        GameObject projectile = Instantiate(prefab, spawnPoint.position, rotation);
//        Rigidbody rb = projectile.GetComponent<Rigidbody>();

//        if (rb != null)
//        {
//            rb.velocity = rotation * Vector3.forward * speed;
//        }
//        else
//        {
//            Debug.LogError($"{prefab.name} prefab does not have a Rigidbody component!");
//        }

//        return projectile;
//    }



//    private void OnCollisionEnter(Collision collision)
//    {
//        // Check if this object is a grenade
//        if (gameObject.CompareTag("Grenade"))
//        {
//            // Check if the other object is the player
//            if (collision.gameObject.CompareTag("Player"))
//            {
//                // Increment grenade count
//                grenadeCount++;
//                UpdateGrenadeCountText(); // Update the displayed grenade count

//                // Optionally destroy the grenade object
//                Destroy(gameObject); // Use 'gameObject' instead of 'Collider.gameObject'

//                Debug.Log("Grenade collected! Total grenades: " + GrenadeCount);
//            }
//        }
//    }


//    //public void AddGrenade()
//    //{
//    //    grenadeCount++; // Increment grenade count
//    //    UpdateGrenadeCountText(); // Update the displayed grenade count
//    //}

//    public void Equip()
//    {
//        gameObject.SetActive(true); // Activate the weapon when equipped
//    }

//    public void Unequip()
//    {
//        gameObject.SetActive(false); // Deactivate the weapon when unequipped
//    }
//}

//public class Weapon : MonoBehaviour // Inherits from MonoBehaviour
//{
//    // Gun variables
//    public Transform bulletSpawnPoint; // The point where the bullet spawns
//    public GameObject bulletPrefab; // The bullet prefab
//    public float bulletSpeed = 10f; // Speed of the bullet
//    public int bulletCount = 6; // Initial bullet count
//    public int grenadeCount = 0;

//    // Grenade variables
//    public Transform launchPoint; // The point where the grenade is launched from
//    public GameObject projectile; // The grenade prefab
//    public float grenadeSpeed = 10f; // Speed of the grenade
//    private PlayerInventory playerInventory; // Reference to the player's inventory

//    // TextMesh Pro reference
//    public TextMeshProUGUI bulletCountText; // Reference to the TextMeshProUGUI component
//    public TextMeshProUGUI grenadeCountText;

//    private void Start()
//    {
//        // Get the PlayerInventory component attached to the player
//        playerInventory = FindObjectOfType<PlayerInventory>();
//        UpdateBulletCountText(); // Initialize the bullet count text
//        UpdateGrenadeCountText();
//    }

//    private void Update()
//    {
//        // Check if the weapon is active and the current weapon in inventory before allowing firing
//        if (playerInventory.currentWeapon == this && gameObject.activeSelf)
//        {
//            if (gameObject.CompareTag("Gun") && Input.GetMouseButtonDown(0)) // Use GetMouseButtonDown for single shot
//            {
//                if (bulletCount > 0) // Check if there are bullets left
//                {
//                    FireGun();
//                }
//                else
//                {
//                    Debug.Log("No bullets left!");
//                }
//            }
//            else if (gameObject.CompareTag("Grenade") && Input.GetMouseButtonDown(0))
//            {
//                if (playerInventory != null && playerInventory.currentWeapon != null && playerInventory.currentWeapon.CompareTag("Grenade"))
//                {
//                    if (grenadeCount > 0)
//                    {
//                        FireGrenade();
//                    }
//                    else
//                    {
//                        Debug.Log("No grenades left!");
//                    }
//                }
//            }
//        }
//    }

//    private void FireGun()
//    {
//        // Create a rotation that adds 90 degrees on the Z-axis to the spawn point's rotation
//        Quaternion bulletRotation = bulletSpawnPoint.rotation * Quaternion.Euler(0, 0, 90);

//        // Instantiate the bullet at the spawn point's position and the modified rotation
//        var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletRotation);

//        // Get the Rigidbody and set its velocity
//        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
//        if (bulletRigidbody != null)
//        {
//            // Set the bullet's velocity based on the forward direction of the new rotation
//            bulletRigidbody.velocity = bulletRotation * Vector3.forward * bulletSpeed;
//        }
//        else
//        {
//            Debug.LogError("Bullet prefab does not have a Rigidbody component!");
//        }

//        bulletCount--; // Decrease the bullet count after firing
//        UpdateBulletCountText(); // Update the text display
//    }

//    private void FireGrenade()
//    {
//        // Create a rotation that adds 90 degrees on the Z-axis to the launch point's rotation
//        Quaternion grenadeRotation = launchPoint.rotation * Quaternion.Euler(0, 0, 90);

//        // Instantiate the grenade at the launch point's position and the modified rotation
//        var grenade = Instantiate(projectile, launchPoint.position, grenadeRotation);

//        // Get the Rigidbody and set its velocity
//        Rigidbody grenadeRigidbody = grenade.GetComponent<Rigidbody>();
//        if (grenadeRigidbody != null)
//        {
//            // Set the grenade's velocity based on the forward direction of the launch point
//            grenadeRigidbody.velocity = grenadeRotation * Vector3.forward * grenadeSpeed;
//        }
//        else
//        {
//            Debug.LogError("Grenade prefab does not have a Rigidbody component!");
//        }
//        grenadeCount--; // Decrease the bullet count after firing
//        UpdateGrenadeCountText(); // Update the text display
//    }

//    public void UpdateBulletCountText()
//    {
//        if (bulletCountText != null)
//        {
//            bulletCountText.text = "Bullets: " + bulletCount; // Update the TextMeshPro text
//        }
//    }

//    public void UpdateGrenadeCountText()
//    {
//        if (grenadeCountText != null)
//        {
//            grenadeCountText.text = "Grenades in the launcher: " + grenadeCount; // Update the TextMeshPro text
//        }
//    }

//    public void Equip()
//    {
//        gameObject.SetActive(true); // Activate the weapon when equipped
//    }

//    public void Unequip()
//    {
//        gameObject.SetActive(false); // Deactivate the weapon when unequipped
//    }
//}


//using UnityEngine;
//using System.Collections;

//public class Weapon : MonoBehaviour // Inherits from MonoBehaviour
//{
//    // Gun variables
//    public Transform bulletSpawnPoint; // The point where the bullet spawns
//    public GameObject bulletPrefab; // The bullet prefab
//    public float bulletSpeed = 10f; // Speed of the bullet

//    // Grenade variables
//    public Transform launchPoint; // The point where the grenade is launched from
//    public GameObject projectile; // The grenade prefab
//    public float grenadeSpeed = 10f; // Speed of the grenade
//    private PlayerInventory playerInventory; // Reference to the player's inventory

//    private void Start()
//    {
//        // Get the PlayerInventory component attached to the player
//        playerInventory = FindObjectOfType<PlayerInventory>();
//    }

//    private void Update()
//    {
//        // Check if the weapon is active and the current weapon in inventory before allowing firing
//        if (playerInventory.currentWeapon == this && gameObject.activeSelf)
//        {
//            if (gameObject.CompareTag("Gun") && Input.GetMouseButtonDown(0)) // Use GetMouseButtonDown for single shot
//            {
//                FireGun();
//            }
//            else if (gameObject.CompareTag("Grenade") && Input.GetMouseButtonDown(0))
//            {
//                if (playerInventory != null && playerInventory.currentWeapon != null && playerInventory.currentWeapon.CompareTag("Grenade"))
//                {
//                    FireGrenade();
//                }
//            }
//        }
//    }

//    private void FireGun()
//    {
//        // Create a rotation that adds 90 degrees on the Z-axis to the spawn point's rotation
//        Quaternion bulletRotation = bulletSpawnPoint.rotation * Quaternion.Euler(0, 0, 90);

//        // Instantiate the bullet at the spawn point's position and the modified rotation
//        var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletRotation);

//        // Get the Rigidbody and set its velocity
//        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
//        if (bulletRigidbody != null)
//        {
//            // Set the bullet's velocity based on the forward direction of the new rotation
//            bulletRigidbody.velocity = bulletRotation * Vector3.forward * bulletSpeed;
//        }
//        else
//        {
//            Debug.LogError("Bullet prefab does not have a Rigidbody component!");
//        }
//    }

//    private void FireGrenade()
//    {
//        // Create a rotation that adds 90 degrees on the Z-axis to the launch point's rotation
//        Quaternion grenadeRotation = launchPoint.rotation * Quaternion.Euler(0, 0, 90);

//        // Instantiate the grenade at the launch point's position and the modified rotation
//        var grenade = Instantiate(projectile, launchPoint.position, grenadeRotation);

//        // Get the Rigidbody and set its velocity
//        Rigidbody grenadeRigidbody = grenade.GetComponent<Rigidbody>();
//        if (grenadeRigidbody != null)
//        {
//            // Set the grenade's velocity based on the forward direction of the launch point
//            grenadeRigidbody.velocity = grenadeRotation * Vector3.forward * grenadeSpeed;
//        }
//        else
//        {
//            Debug.LogError("Grenade prefab does not have a Rigidbody component!");
//        }
//    }

//    public void Equip()
//    {
//        gameObject.SetActive(true); // Activate the weapon when equipped
//    }

//    public void Unequip()
//    {
//        gameObject.SetActive(false); // Deactivate the weapon when unequipped
//    }
//}