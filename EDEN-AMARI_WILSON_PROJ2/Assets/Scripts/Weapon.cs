using System.Collections;
using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    // Gun variables
    public Transform bulletSpawnPoint; // The point where the bullet spawns
    public GameObject bulletPrefab; // The bullet prefab
    public float bulletSpeed = 30f; // Speed of the bullet
    //public int bulletCount; // Initial bullet count

    // Grenade variables
    public Transform launchPoint; // The point from where the grenade will be launched
    public GameObject grenadePrefab; // The grenade prefab
    public float grenadeSpeed = 10f; // Speed of the grenade
    public float maxLaunchHeight = 1.5f; // Maximum height of the arc
    public Camera mainCamera; // Reference to the main camera
    public float launchDuration = 2f; // Duration of the launch
    //public int grenadeCount = 1; // Initial grenade count

    private PlayerInventory playerInventory; // Reference to the player's inventory
    private PlayerMovement playerMovement;
    //private bool isLaunching = false; // To prevent multiple grenade launches

    // TextMesh Pro references
    public AudioClip grenadeSound1;  // Declare grenadeSound1
    public AudioClip grenadeSound2;  // Declare grenadeSound2
 


    public void Start()
    {
        // Get the PlayerInventory component attached to the player
        playerInventory = FindFirstObjectByType<PlayerInventory>();
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        playerMovement.UpdateBulletCountText(); // Initialize the bullet count text
        playerMovement.UpdateGrenadeCountText(); // Initialize the grenade count text

        playerMovement.bulletCount = 6; // Initial bullet count

}

    private void Update()
    {
        if ((playerInventory.collectedWeapons.Contains(this) == false) && (this.tag == "Gun"))
        {
            Vector3 rotationToAdd = new Vector3(0, 0, .5f);
            transform.Rotate(rotationToAdd);
        }

        else if ((playerInventory.collectedWeapons.Contains(this) == false) && (this.tag == "GrenadeLauncher"))
        {
            Vector3 rotationToAdd = new Vector3(0, .5f, 0);
            transform.Rotate(rotationToAdd);
        }

        // Check if the weapon is active and the current weapon in inventory before allowing firing
        else if (playerInventory.currentWeapon == this && gameObject.activeSelf)
        {
            Debug.Log($"Current Weapon: {this.name}, Grenade Count: {playerMovement.grenadeCount}"); // Log the weapon name and grenade count

            if (gameObject.CompareTag("Gun") && Input.GetMouseButtonDown(0)) 
            {
                if (playerMovement.bulletCount > 0) 
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
                if (playerMovement.grenadeCount > 0)
                {
                    Debug.Log("Firing Grenade!");
                    FireProjectile(grenadePrefab, launchPoint, grenadeSpeed); // Correct function call
                    playerMovement.grenadeCount--; // Decrease grenade count
                    playerMovement.UpdateGrenadeCountText(); // Update the text display
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
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play(); // Play the sound
        }
        playerMovement.bulletCount--; // Decrease the bullet count after firing
        playerMovement.UpdateBulletCountText(); // Update the text display
    }

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
        // Instantiate the projectile at the spawn point
        Quaternion rotation = spawnPoint.rotation;
        GameObject projectile = Instantiate(prefab, spawnPoint.position, rotation);

        // Get the Rigidbody component from the instantiated projectile
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Add force to the projectile to launch it forward
            rb.AddForce(rotation * Vector3.forward * speed, ForceMode.VelocityChange);
            rb.velocity = rotation * Vector3.forward * speed;

            // Get the AudioSource component (if attached to the same object)
            AudioSource audioSource = GetComponent<AudioSource>();

            if (audioSource != null)
            {
                // Play grenadeSound1 immediately
                audioSource.clip = grenadeSound1;
                audioSource.Play(); // Play the first sound

                // Start the coroutine to play grenadeSound2 after 0.7 seconds
                StartCoroutine(PlayDelayedSound(audioSource, 1f));
            }
        }
        else
        {
            Debug.LogError($"{prefab.name} prefab does not have a Rigidbody component!");
        }

        // Optionally, destroy the projectile after a delay
        Destroy(projectile, .5f);
    }

    // Coroutine to play the second sound after a delay
    private IEnumerator PlayDelayedSound(AudioSource audioSource, float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay

        // After the delay, play grenadeSound2
        if (audioSource != null)
        {
            audioSource.clip = grenadeSound2; // Set the second sound
            audioSource.Play(); // Play the second sound
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

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


