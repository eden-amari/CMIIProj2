using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float life = 3f;

    void Awake()
    {
        Destroy(gameObject, life);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has the "Enemy" tag
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Destroy the enemy object
            Destroy(collision.gameObject);
        }

        // Always destroy the bullet on collision
        Destroy(gameObject);
    }
}
