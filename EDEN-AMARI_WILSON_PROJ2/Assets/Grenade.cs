//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float explosionDelay = 3f;       // Time until the grenade explodes
    public float explosionRadius = 5f;      // Radius of the explosion
    public float explosionForce = 100f;     // Force applied on objects when it explodes
    public GameObject explosionEffect;     // The visual effect of the explosion

    private bool hasExploded = false;

    void Start()
    {
        // Start the explosion timer
        Invoke("Explode", explosionDelay);
    }

    void Explode()
    {
        if (hasExploded)
            return;

        hasExploded = true;


        // Show explosion effect
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }

        // Apply explosion force to surrounding objects
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }

        // Destroy grenade object after explosion
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        // Draw the explosion radius in the editor for debugging
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}

//using UnityEngine;

//public class Grenade : MonoBehaviour
//{
//    // Reference to the weapon component
//    private Weapon weapon;

//    private void Start()
//    {
//        weapon = FindObjectOfType<Weapon>(); // Get the Weapon component in the scene
//    }

//    private void OnCollisionEnter(Collision collision)
//    {
//        // Check if the object that collided is the player
//        if (collision.gameObject.CompareTag("Player"))
//        {
//            if (weapon != null)
//            {
//                // Incrgment grenade count using the property
//                weapon.grenadeCount++; // Use the public property to ensure the UI updates
//                Destroy(gameObject); // Destroy the grenade object
//                Debug.Log("Grenade collected! Total grenades: " + weapon.grenadeCount);
//            }
//        }
//    }
//}





//using UnityEngine;

//public class Grenade : MonoBehaviour
//{
//    // Reference to the weapon component
//    private Weapon weapon;

//    private void Start()
//    {
//        weapon = FindObjectOfType<Weapon>(); // Get the Weapon component in the scene
//    }

//    private void OnCollisionEnter(Collision collision)
//    {
//        // Example of how to access grenadeCount when the grenade hits the player
//        if (collision.gameObject.CompareTag("Player"))
//        {
//            if (weapon != null)
//            {
//                weapon.grenadeCount++; // Access grenadeCount through the property
//                Destroy(gameObject); // Destroy the grenade object
//                Debug.Log("Grenade collected! Total grenades: " + weapon.grenadeCount);
//            }
//        }
//    }
//}

//public class Grenade : MonoBehaviour
//{
//    private Weapon weapon; // Reference to the Weapon component

//    private void Start()
//    {
//        // Find the Weapon component attached to the player or the weapon holder
//        weapon = FindObjectOfType<Weapon>();
//    }

//    void OnCollisionEnter(Collision collision)
//    {
//        // Check if the collided object has the "Enemy" tag
//        if (collision.gameObject.CompareTag("Enemy"))
//        {
//            // Destroy the enemy object
//            Destroy(collision.gameObject);
//        }
//        // Check if the collided object has the "Player" tag
//        else if (collision.gameObject.CompareTag("Player"))
//        {
//            // Increment grenade count if the reference is valid

//            weapon.grenadeCount++; // Increment grenade count
//            weapon.UpdateGrenadeCountText(); // Update the displayed grenade count


//            // Always destroy the grenade on collision with the player
//            Destroy(gameObject);
//        }

//    }
//}



//public class Grenade : MonoBehaviour
//{
//    private Weapon weapon; // Reference to the Weapon component

//    private void Start()
//    {
//        // Find the Weapon component attached to the player or the weapon holder
//        weapon = FindObjectOfType<Weapon>();
//    }

//    void OnCollisionEnter(Collision collision)
//    {
//        // Check if the collided object has the "Enemy" tag
//        if (collision.gameObject.CompareTag("Enemy"))
//        {
//            // Destroy the enemy object
//            Destroy(collision.gameObject);
//        }
//        // Check if the collided object has the "Player" tag
//        else if (collision.gameObject.CompareTag("Player"))
//        {
//            // Do nothing, don't destroy the grenade
//            return;
//        }
//        else
//        {
//            // Increment grenade count directly if the reference is valid
//            if (weapon != null)
//            {
//                weapon.grenadeCount++; // Directly increment grenade count
//            }

//            // Always destroy the grenade on collision with other objects
//            Destroy(gameObject);
//        }
//    }
//}



//public class Grenade : MonoBehaviour
//{
//    //// Start is called before the first frame update
//    //void Start()
//    //{

//    //}

//    //// Update is called once per frame
//    //void Update()
//    //{

//    //}



//    void OnCollisionEnter(Collision collision)
//    {
//        // Check if the collided object has the "Enemy" tag
//        if (collision.gameObject.CompareTag("Enemy"))
//        {
//            // Destroy the enemy object
//            Destroy(collision.gameObject);
//        }
//        // Check if the collided object has the "Player" tag
//        else if (collision.gameObject.CompareTag("Player"))
//        {

//            return;
//        }

//        else
//        {
//            Destroy(gameObject);
//        }// Always destroy the bullet on collision with other objects

//    }

//}



