using UnityEngine;
using System.Collections; // Required for coroutines

public class TreasureChest : MonoBehaviour
{

    public bool isPlayerWithTreasure;


    public bool isPlayerInRange;
   
    private SphereCollider chestCollider; // The collider for the treasure chest
   
   
    private Enemy enemyScript;

    // Start is called before the first frame update
    void Start()
    {
        chestCollider = GetComponent<SphereCollider>();

        if (chestCollider == null)
        {
            Debug.LogError("Treasure chest does not have a SphereCollider!");
        }

      
    }

    // Update is called once per frame
    void Update()
    {
    

        //if (isPlayerWithTreasure || isPlayerInRange)
        //{
        //    // Rotate enemies to face the player
        //    RotateEnemiesToFacePlayer();

        //    // Move enemies towards the player
        //    FollowPlayer();

        //    // Optionally, shoot at the player if enemies have the shooting capability
        //    //if (enemyScript != null)
        //    //{
        //    //    enemyScript.ShootAtPlayer();
        //    //}
        //}
        //else
        //{
        //    // Patrol when the player is not in range and hasn't picked up the treasure
        //    Patrol();
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Player entered the range of the treasure chest.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log("Player exited the range of the treasure chest.");
        }
    }

    // Call this function when the player picks up the treasure
    public void PlayerPickedUpTreasure()
    {
        isPlayerWithTreasure = true; // Player has picked up the treasure
        Debug.Log("Player picked up the treasure.");
    }

    // Rotate the enemies to face the player
    //void RotateEnemiesToFacePlayer()
    //{
    //    if (enemy1 != null && player != null)
    //    {
    //        Vector3 direction1 = player.position - enemy1.position;
    //        direction1.y = 0; // Ignore vertical component (y-axis)
    //        Quaternion targetRotation1 = Quaternion.LookRotation(direction1);
    //        enemy1.rotation = Quaternion.Slerp(enemy1.rotation, targetRotation1, rotationSpeed * Time.deltaTime);
    //    }

    //    if (enemy2 != null && player != null)
    //    {
    //        Vector3 direction2 = player.position - enemy2.position;
    //        direction2.y = 0; // Ignore vertical component (y-axis)
    //        Quaternion targetRotation2 = Quaternion.LookRotation(direction2);
    //        enemy2.rotation = Quaternion.Slerp(enemy2.rotation, targetRotation2, rotationSpeed * Time.deltaTime);
    //    }
    //}

    //// Function to move both enemies towards the player
    //void FollowPlayer()
    //{
    //        Vector3 direction1 = player.position - enemy1.position;
    //        direction1.y = 0; // Keep on the same height level
    //        enemy1.position += direction1.normalized * moveSpeed * Time.deltaTime;
        
    //        Vector3 direction2 = player.position - enemy2.position;
    //        direction2.y = 0;
    //        enemy2.position += direction2.normalized * moveSpeed * Time.deltaTime;     
    //}

    //// Function to patrol the guards between waypoints
    //void Patrol()
    //{
    //    if (enemy1 != null && patrolPoints1.Length > 0)
    //    {
    //        StartCoroutine(PatrolEnemy(enemy1, patrolPoints1, currentPatrolIndex1));
    //    }

    //    if (enemy2 != null && patrolPoints2.Length > 0)
    //    {
    //        StartCoroutine(PatrolEnemy(enemy2, patrolPoints2, currentPatrolIndex2));
    //    }
    //}

    //// Coroutine to handle patrol behavior with pauses and rotation for each enemy
    //IEnumerator PatrolEnemy(Transform enemy, Transform[] patrolPoints, int currentPatrolIndex)
    //{
    //    Transform targetPatrolPoint = patrolPoints[currentPatrolIndex];

    //        // Move towards the current patrol point
    //        while (Vector3.Distance(enemy.position, targetPatrolPoint.position) > 0.1f)
    //        {
    //            Vector3 direction = targetPatrolPoint.position - enemy.position;
    //            direction.y = 0; // Keep the guard on the same height level
    //            enemy.position += direction.normalized * moveSpeed * Time.deltaTime;
    //            yield return null;
    //        }

    //        // Wait for the pause time before continuing to the next patrol point
    //        yield return new WaitForSeconds(pauseTime);

    //        // Rotate the enemy 90 degrees after the pause
    //        enemy.Rotate(0f, 180f, 0f); // Rotate 90 degrees along the Y-axis

    //        // After pause and rotation, switch to the next patrol point
    //        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        
    //}

    //public void FollowPlayer()
    //{
    //    // Calculate the direction towards the player
    //    Vector3 direction = player.position - transform.position;

    //    // Optional: Set the y component to 0 to keep the enemy on the same height level as the player
    //    direction.y = 0;

    //    // Normalize the direction and move towards the player
    //    transform.position += direction.normalized * moveSpeed * Time.deltaTime;
    //}
}


//using UnityEngine;
//using System.Collections; // Required for coroutines



//public class TreasureChest : MonoBehaviour
//{
//    public Transform player;          // Reference to the player
//    public Transform enemy1;          // Reference to the first enemy
//    public Transform enemy2;          // Reference to the second enemy
//    public Transform[] patrolPoints1; // Patrol points for enemy1
//    public Transform[] patrolPoints2; // Patrol points for enemy2
//    public float moveSpeed = 2.5f;    // Speed of the enemies' movement
//    public float pauseTime = 2f;      // Time to pause between patrol points
//    private bool isPlayerInRange = false; // Whether the player is inside the treasure chest's collider
//    private int currentPatrolIndex1 = 0;  // Current patrol point index for enemy1
//    private int currentPatrolIndex2 = 0;  // Current patrol point index for enemy2
//    private Vector3 originalPosition1; // The first enemy's original position to return to
//    private Vector3 originalPosition2; // The second enemy's original position to return to
//    private SphereCollider chestCollider; // The collider for the treasure chest
//    private bool isPatrolling1 = false;  // Whether enemy1 is patrolling
//    private bool isPatrolling2 = false;  // Whether enemy2 is patrolling
//    public float rotationSpeed = 2f;     // Speed of rotation when facing the player
//    private Enemy enemyScript;
//    // Start is called before the first frame update
//    void Start()
//    {
//        chestCollider = GetComponent<SphereCollider>();

//        if (chestCollider == null)
//        {
//            Debug.LogError("Treasure chest does not have a SphereCollider!");
//        }

//        if (enemy1 != null)
//        {
//            originalPosition1 = enemy1.position;
//        }

//        if (enemy2 != null)
//        {
//            originalPosition2 = enemy2.position;
//        }

//        if (patrolPoints1.Length == 0 || patrolPoints2.Length == 0)
//        {
//            Debug.LogError("Patrol points for one or both enemies are not assigned!");
//        }
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (isPlayerInRange)
//        {
//            // Rotate enemies to face the player
//            RotateEnemiesToFacePlayer();

//            // Move enemies towards the player
//            FollowPlayer();
//            enemyScript.ShootAtPlayer();
//        }
//        else
//        {
//            // Patrol when the player is not in range
//            Patrol();
//        }
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            isPlayerInRange = true;
//            Debug.Log("Player entered the range of the treasure chest.");
//        }
//    }

//    private void OnTriggerExit(Collider other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            isPlayerInRange = false;
//            Debug.Log("Player exited the range of the treasure chest.");
//        }
//    }

//    // Rotate the enemies to face the player
//    void RotateEnemiesToFacePlayer()
//    {
//        if (enemy1 != null && player != null)
//        {
//            Vector3 direction1 = player.position - enemy1.position;
//            direction1.y = 0; // Ignore vertical component (y-axis)
//            Quaternion targetRotation1 = Quaternion.LookRotation(direction1);
//            enemy1.rotation = Quaternion.Slerp(enemy1.rotation, targetRotation1, rotationSpeed * Time.deltaTime);
//        }

//        if (enemy2 != null && player != null)
//        {
//            Vector3 direction2 = player.position - enemy2.position;
//            direction2.y = 0; // Ignore vertical component (y-axis)
//            Quaternion targetRotation2 = Quaternion.LookRotation(direction2);
//            enemy2.rotation = Quaternion.Slerp(enemy2.rotation, targetRotation2, rotationSpeed * Time.deltaTime);
//        }
//    }

//    // Function to move both enemies towards the player
//    void FollowPlayer()
//    {
//        if (enemy1 != null && player != null)
//        {
//            Vector3 direction1 = player.position - enemy1.position;
//            direction1.y = 0; // Keep on the same height level
//            enemy1.position += direction1.normalized * moveSpeed * Time.deltaTime;
//        }

//        if (enemy2 != null && player != null)
//        {
//            Vector3 direction2 = player.position - enemy2.position;
//            direction2.y = 0;
//            enemy2.position += direction2.normalized * moveSpeed * Time.deltaTime;
//        }
//    }

//    // Function to patrol the guards between waypoints
//    void Patrol()
//    {
//        if (enemy1 != null && patrolPoints1.Length > 0 && !isPatrolling1)
//        {
//            isPatrolling1 = true;
//            StartCoroutine(PatrolEnemy(enemy1, patrolPoints1, currentPatrolIndex1));
//        }

//        if (enemy2 != null && patrolPoints2.Length > 0 && !isPatrolling2)
//        {
//            isPatrolling2 = true;
//            StartCoroutine(PatrolEnemy(enemy2, patrolPoints2, currentPatrolIndex2));
//        }
//    }

//    // Coroutine to handle patrol behavior with pauses and rotation for each enemy
//    IEnumerator PatrolEnemy(Transform enemy, Transform[] patrolPoints, int currentPatrolIndex)
//    {
//        while (true)
//        {
//            Transform targetPatrolPoint = patrolPoints[currentPatrolIndex];

//            // Move towards the current patrol point
//            while (Vector3.Distance(enemy.position, targetPatrolPoint.position) > 0.1f)
//            {
//                Vector3 direction = targetPatrolPoint.position - enemy.position;
//                direction.y = 0; // Keep the guard on the same height level
//                enemy.position += direction.normalized * moveSpeed * Time.deltaTime;
//                yield return null;
//            }

//            // Wait for the pause time before continuing to the next patrol point
//            yield return new WaitForSeconds(pauseTime);

//            // Rotate the enemy 90 degrees after the pause
//            enemy.Rotate(0f, 180f, 0f); // Rotate 90 degrees along the Y-axis

//            // After pause and rotation, switch to the next patrol point
//            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
//        }
//    }
//}
