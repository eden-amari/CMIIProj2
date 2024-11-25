using UnityEngine;
using System.Collections;
public class HealthPack : MonoBehaviour
{
    public float respawnTime = 10f;  // Time in seconds before respawning the health pack
    public Transform respawnLocation;  // Location where the health pack will respawn (can be set in the inspector)

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player picks up the health pack
        if (other.CompareTag("Player"))
        {
            // Pick up the health pack (you can add any additional logic like restoring health here)
            Debug.Log("Health Pack Picked Up!");

            // Disable the health pack (hide it)
            gameObject.SetActive(false);

            // Start the respawn process
            StartCoroutine(RespawnHealthPack());
        }
    }

    private IEnumerator RespawnHealthPack()
    {
        // Wait for the specified respawn time
        yield return new WaitForSeconds(respawnTime);

        // Reactivate the health pack (or instantiate a new one at the respawn location)
        if (respawnLocation != null)
        {
            // If you have a respawn location set, move the health pack back to that position
            transform.position = respawnLocation.position;
        }

        // Activate the health pack again
        gameObject.SetActive(true);
    }
}
