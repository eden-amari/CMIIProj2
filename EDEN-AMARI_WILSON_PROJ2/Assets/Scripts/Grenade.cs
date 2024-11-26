using UnityEngine;

public class ProjectileCollision : MonoBehaviour
{
    private bool hasCollided = false;  // Flag to track if the projectile has collided

    // Called when the projectile collides with another collider
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the projectile has collided with something
        if (!hasCollided)
        {
            hasCollided = true;  // Set the flag to true so the projectile doesn't destroy again

            // You can handle the collision logic here, like damaging enemies or other effects
            Debug.Log("Projectile collided with " + collision.gameObject.name);

            // Destroy the projectile immediately after collision
            Destroy(gameObject);
        }
    }

}
