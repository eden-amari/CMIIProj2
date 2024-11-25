using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private PlayerMovement playerMovement;

    public float despawnTimer = .2f;
    void Start()
    {
        playerMovement = FindFirstObjectByType<PlayerMovement>();
    }

    void Awake()
    {
        Destroy(gameObject, despawnTimer);
    }

    void OnCollisionEnter(Collision collision)
    {
        
        //if (gameObject.CompareTag("playerBullet") && (collision.gameObject.CompareTag("Enemy")))
        //{
        //    // Destroy the enemy object
        //    Destroy(gameObject);
        //}

        if (gameObject.CompareTag("enemyBullet") && (collision.gameObject.CompareTag("Player")))
        {
            playerMovement.lives--;
            playerMovement.UpdateLifeText();
            Destroy(gameObject);
        }


        
    }
}
