using UnityEngine;
using System.Collections;


public class Enemy : MonoBehaviour
{
    public Transform player;
    public Transform enemy;
    private PlayerMovement playerMovement;  // Reference to the PlayerMovement script

    //public TreasureChest treasureScript;
    //public Transform treasureBox;
    public Collider treasureCollider;

    public GameObject projectilePrefab;     // The projectile prefab to shoot
    public Transform gunSpawnPoint;         // The position from where the enemy shoots
    public float shootingSpeed = 10f;       // Speed of the projectile
    public float shootInterval = 2.5f;        // Interval between each shot (in seconds)

    private float shootCooldown = 0f;       // Timer to control when the enemy can shoot
    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;
    private Vector3 originalPosition;

    public float moveSpeed = 1.5f;    // Speed of the enemies' movement
    public float pauseTime = 2f;      // Time to pause between patrol points

    public float rotationSpeed = 2f;     // Speed of rotation when facing the player

    void Start()
    {
        playerMovement = FindFirstObjectByType<PlayerMovement>(); // Finding the PlayerMovement script

        //Collider treasureCollider = GameObject.Find("Treasure").GetComponent<Collider>();

        Patrol();

    }

    void Update()
    {
        if (treasureCollider.bounds.Contains(player.position))
        {
            FollowPlayer();

            if (shootCooldown <= 0f)
            {
                ShootAtPlayer();
                shootCooldown = shootInterval; // Reset the cooldown timer
            }
            else
            {
                shootCooldown -= Time.deltaTime; // Decrease the cooldown timer over time
            }          
        }
        

        else if (treasureCollider == null)
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
            FollowPlayer();
            
        }
        RotateEnemiesToFacePlayer();
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

    void FollowPlayer()
    {
        Vector3 direction1 = player.position - enemy.position;
        direction1.y = 0; // Keep on the same height level
        enemy.position += direction1.normalized * moveSpeed * Time.deltaTime;
    }
    void Patrol()
    {
        if (patrolPoints.Length > 0)
        {
            StartCoroutine(PatrolEnemy(enemy, patrolPoints, currentPatrolIndex));
        }
    }

    // Coroutine to handle patrol behavior with pauses and rotation for each enemy
    IEnumerator PatrolEnemy(Transform enemy, Transform[] patrolPoints, int currentPatrolIndex)
    {
        while (true)
        {
            Transform targetPatrolPoint = patrolPoints[currentPatrolIndex];

            // Move towards the current patrol point
            while (Vector3.Distance(enemy.position, targetPatrolPoint.position) > 0.1f)
            {
                Vector3 direction = targetPatrolPoint.position - enemy.position;
                direction.y = 0; // Keep the guard on the same height level
                enemy.position += direction.normalized * moveSpeed * Time.deltaTime;
                yield return null;
            }

            // Wait for the pause time before continuing to the next patrol point
            yield return new WaitForSeconds(pauseTime);

            // Rotate the enemy 90 degrees after the pause
            enemy.Rotate(0f, 180f, 0f); // Rotate 90 degrees along the Y-axis

            // After pause and rotation, switch to the next patrol point
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    void RotateEnemiesToFacePlayer()
    {
        Vector3 direction1 = player.position - enemy.position;
        direction1.y = 0; // Ignore vertical component (y-axis)
        Quaternion targetRotation1 = Quaternion.LookRotation(direction1);
        enemy.rotation = Quaternion.Slerp(enemy.rotation, targetRotation1, rotationSpeed * Time.deltaTime);
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