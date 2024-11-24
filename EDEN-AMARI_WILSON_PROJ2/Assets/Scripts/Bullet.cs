using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private PlayerMovement playerMovement;

    public float despawnTimer = .2f;  // Set the bullet life to 1 seconds
    void Start()
    {
        playerMovement = FindFirstObjectByType<PlayerMovement>();
    }

    void Awake()
    {
        // Destroy the bullet after 'life' seconds (1.5 in this case)
        Destroy(gameObject, despawnTimer);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has the "Enemy" tag
        if (gameObject.CompareTag("playerBullet") && (collision.gameObject.CompareTag("Enemy")))
        {
            // Destroy the enemy object
            Destroy(collision.gameObject);
        }

        if (gameObject.CompareTag("enemyBullet") && (collision.gameObject.CompareTag("Player")))
        {
            // Destroy the enemy object
            playerMovement.lives--;
            playerMovement.UpdateLifeText();
        }


        // Always destroy the bullet on collision
        Destroy(gameObject);
    }
}
