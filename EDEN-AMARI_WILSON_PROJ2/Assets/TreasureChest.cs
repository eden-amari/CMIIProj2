using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    public Transform player;          // Reference to the player
    public Transform enemy;           // Reference to the first enemy
    public Transform enemy2;          // Reference to the second enemy
    private bool isPlayerInRange = false; // Whether the player is inside the treasure chest collider

    public float moveSpeed = 2.5f;     // How fast the enemy moves towards the player
    private Vector3 originalPosition; // The enemy's original position to return to
    private Vector3 originalPosition2; // The second enemy's original position to return to

    private SphereCollider chestCollider; // The collider for the treasure chest

    // Start is called before the first frame update
    void Start()
    {
        // Get the SphereCollider attached to the treasure chest
        chestCollider = GetComponent<SphereCollider>();

        // Check if there's a SphereCollider attached to this GameObject
        if (chestCollider == null)
        {
            Debug.LogError("Treasure chest does not have a SphereCollider!");
        }

        // Initialize original position of the enemies
        if (enemy != null)
        {
            originalPosition = enemy.position; // Store the first enemy's original position
        }

        if (enemy2 != null)
        {
            originalPosition2 = enemy2.position; // Store the second enemy's original position
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If the player is within the range, move the enemies toward the player
        if (isPlayerInRange)
        {
            FollowPlayer();
        }
        else
        {
            ReturnEnemiesToOriginalPosition();
        }
    }

    // Called when the player enters the treasure chest's collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the range of the treasure chest
        if (other.CompareTag("Player")) // Ensure it is the player
        {
            // Player is inside the treasure chest's radius
            isPlayerInRange = true;
            Debug.Log("Player entered the range of the treasure chest.");
        }
    }

    // Called when the player exits the treasure chest's collider
    private void OnTriggerExit(Collider other)
    {
        // Check if the player leaves the range of the treasure chest
        if (other.CompareTag("Player")) // Ensure it is the player
        {
            // Player left the range of the treasure chest
            isPlayerInRange = false;
            Debug.Log("Player exited the range of the treasure chest.");
        }
    }

    // Function to move both enemies towards the player
    void FollowPlayer()
    {
        if (enemy != null && player != null)
        {
            // Calculate the direction to the player for the first enemy
            Vector3 direction1 = player.position - enemy.position;
            direction1.y = 0; // Keep the first enemy on the same height level
            enemy.position += direction1.normalized * moveSpeed * Time.deltaTime; // Move towards the player
        }

        if (enemy2 != null && player != null)
        {
            // Calculate the direction to the player for the second enemy
            Vector3 direction2 = player.position - enemy2.position;
            direction2.y = 0; // Keep the second enemy on the same height level
            enemy2.position += direction2.normalized * moveSpeed * Time.deltaTime; // Move towards the player
        }
    }

    // Function to return both enemies to their original positions when the player leaves the range
    void ReturnEnemiesToOriginalPosition()
    {
        if (enemy != null)
        {
            // Calculate the direction to return to the original position for the first enemy
            Vector3 direction1 = originalPosition - enemy.position;
            direction1.y = 0; // Keep the first enemy on the same height level
            enemy.position += direction1.normalized * moveSpeed * Time.deltaTime; // Move back to the original position

            // Stop moving when close to the original position for the first enemy
            if (Vector3.Distance(enemy.position, originalPosition) < 0.1f)
            {
                enemy.position = originalPosition; // Snap to the original position
            }
        }

        if (enemy2 != null)
        {
            // Calculate the direction to return to the original position for the second enemy
            Vector3 direction2 = originalPosition2 - enemy2.position;
            direction2.y = 0; // Keep the second enemy on the same height level
            enemy2.position += direction2.normalized * moveSpeed * Time.deltaTime; // Move back to the original position

            // Stop moving when close to the original position for the second enemy
            if (Vector3.Distance(enemy2.position, originalPosition2) < 0.1f)
            {
                enemy2.position = originalPosition2; // Snap to the original position
            }
        }
    }
}


//using UnityEngine;

//public class TreasureChest : MonoBehaviour
//{

//    public Transform player;          // Reference to the player
//    public Transform enemy;           // Reference to the enemy that will follow the player
//    public Transform enemy2;
//    private bool isPlayerInRange = false; // Whether the player is inside the treasure chest collider

//    public float moveSpeed = 2.5f;     // How fast the enemy moves towards the player
//    private Vector3 originalPosition; // The enemy's original position to return to
//    private Vector3 originalPosition2; // The enemy's original position to return to

//    private SphereCollider chestCollider; // The collider for the treasure chest

//    // Start is called before the first frame update
//    void Start()
//    {
//        // Get the SphereCollider attached to the treasure chest
//        chestCollider = GetComponent<SphereCollider>();

//        // Check if there's a SphereCollider attached to this GameObject
//        if (chestCollider == null)
//        {
//            Debug.LogError("Treasure chest does not have a SphereCollider!");
//        }

//        // Initialize original position of the enemy
//        if (enemy != null)
//        {
//            originalPosition = enemy.position; // Store the enemy's original position
//        }

//        if (enemy2 != null) 
//        {
//            originalPosition2 = enemy2.position; // Store the enemy's original position
//        }
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        // If the player is within the range, move the enemy toward the player
//        if (isPlayerInRange)
//        {
//            FollowPlayer();
//        }
//        else
//        {
//            ReturnEnemyToOriginalPosition();
//        }
//    }

//    // Called when the player enters the treasure chest's collider
//    private void OnTriggerEnter(Collider other)
//    {
//        // Check if the player enters the range of the treasure chest
//        if (other.CompareTag("Player")) // Ensure it is the player
//        {
//            // Player is inside the treasure chest's radius
//            isPlayerInRange = true;
//            Debug.Log("Player entered the range of the treasure chest.");
//        }
//    }

//    // Called when the player exits the treasure chest's collider
//    private void OnTriggerExit(Collider other)
//    {
//        // Check if the player leaves the range of the treasure chest
//        if (other.CompareTag("Player")) // Ensure it is the player
//        {
//            // Player left the range of the treasure chest
//            isPlayerInRange = false;
//            Debug.Log("Player exited the range of the treasure chest.");
//        }
//    }

//    // Function to move the enemy towards the player
//    void FollowPlayer()
//    {
//        if (enemy == null || player == null) return;

//        // Calculate the direction to the player
//        Vector3 direction = player.position - enemy.position;

//        // Optionally, keep the enemy on the same height level as the player
//        direction.y = 0;

//        // Normalize the direction and move towards the player
//        enemy.position += direction.normalized * moveSpeed * Time.deltaTime;
//    }

//    // Function to return the enemy to its original position when the player leaves the range
//    void ReturnEnemyToOriginalPosition()
//    {
//        if (enemy == null) return;

//        // Calculate the direction to return to the original position
//        Vector3 direction = originalPosition - enemy.position;

//        // Optionally, keep the enemy on the same height level
//        direction.y = 0;

//        // Normalize the direction and move towards the original position
//        enemy.position += direction.normalized * moveSpeed * Time.deltaTime;

//        // Stop moving when close to the original position
//        if (Vector3.Distance(enemy.position, originalPosition) < 0.1f)
//        {
//            enemy.position = originalPosition; // Snap to the original position
//        }
//    }
//}
