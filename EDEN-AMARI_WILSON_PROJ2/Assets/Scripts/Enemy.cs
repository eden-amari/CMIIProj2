using UnityEngine;

public class Enemy : MonoBehaviour
{

    private PlayerMovement playerMovement;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement = FindFirstObjectByType<PlayerMovement>();

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has the "Enemy" tag
        if (collision.gameObject.CompareTag("Grenade") || collision.gameObject.CompareTag("Bullet"))
        {
            // Destroy the enemy object
            Destroy(gameObject);
            Destroy(collision.gameObject);
            if (playerMovement != null)
            {
                playerMovement.enemiesDefeated++;
                playerMovement.enemiesLeft--;
                playerMovement.UpdateEnemyText();
            }
            else
            {
                Debug.LogWarning("PlayerMovement not found!");
            }

        }

        // Always destroy the bullet on collision
        

    }
}