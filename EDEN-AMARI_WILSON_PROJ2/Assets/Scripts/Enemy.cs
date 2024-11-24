using UnityEngine;

public class Enemy : MonoBehaviour
{
    private PlayerMovement playerMovement;  // Reference to the PlayerMovement script
    public TreasureChest treasureScript;
    public EnemyFollow enemyFollow;
    public GameObject treasureChest;
    public GameObject projectilePrefab;     // The projectile prefab to shoot
    public Transform gunSpawnPoint;         // The position from where the enemy shoots
    public float shootingSpeed = 10f;       // Speed of the projectile
    public float shootInterval = 2.5f;        // Interval between each shot (in seconds)

    private float shootCooldown = 0f;       // Timer to control when the enemy can shoot

    void Start()
    {
        playerMovement = FindFirstObjectByType<PlayerMovement>(); // Finding the PlayerMovement script
    }

    void Update()
    {
        if (shootCooldown <= 0f)
        {
            ShootAtPlayer();
            shootCooldown = shootInterval; // Reset the cooldown timer
        }
        else
        {
            shootCooldown -= Time.deltaTime; // Decrease the cooldown timer over time
        }

        if (treasureChest == null)
        {
            ShootAtPlayer();
            shootCooldown = shootInterval; // Reset the cooldown timer
            if (enemyFollow != null)
            {
                enemyFollow.FollowPlayer();
            }
        }
    }
    public void ShootAtPlayer()
    {
        if (playerMovement != null)
        {
            // Get the direction towards the player
            Vector3 directionToPlayer = playerMovement.transform.position - gunSpawnPoint.position;
            directionToPlayer.y = 0f; // Ensure the enemy shoots horizontally (no vertical angle)

            // Create a rotation to face the player
            Quaternion rotation = Quaternion.LookRotation(directionToPlayer);
            gunSpawnPoint.rotation = rotation;

            // Call ShootGun function to shoot the projectile
            ShootGun(projectilePrefab, gunSpawnPoint, shootingSpeed);
        }
        else
        {
            Debug.LogWarning("PlayerMovement not found!");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has the "Grenade" or "playerBullet" tag
        if (collision.gameObject.CompareTag("Grenade") || collision.gameObject.CompareTag("playerBullet"))
        {
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
    }

    // Provided function to shoot a projectile
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

        // Remove this line if you want the projectile to remain active
        // Destroy(projectile);

        return projectile;
    }
}


//using UnityEngine;

//public class Enemy : MonoBehaviour
//{

//    private PlayerMovement playerMovement;


//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {
//        playerMovement = FindFirstObjectByType<PlayerMovement>();

//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }
//    void OnCollisionEnter(Collision collision)
//    {
//        // Check if the collided object has the "Enemy" tag
//        if (collision.gameObject.CompareTag("Grenade") || collision.gameObject.CompareTag("Bullet"))
//        {
//            // Destroy the enemy object
//            Destroy(gameObject);
//            Destroy(collision.gameObject);
//            if (playerMovement != null)
//            {
//                playerMovement.enemiesDefeated++;
//                playerMovement.enemiesLeft--;
//                playerMovement.UpdateEnemyText();
//            }
//            else
//            {
//                Debug.LogWarning("PlayerMovement not found!");
//            }

//        }

//        // Always destroy the bullet on collision


//    }
//}