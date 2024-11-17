//using UnityEngine;
//using UnityEngine.AI;

//public class EnemyFollow : MonoBehaviour
//{
//    public Transform targetObj;
//    //public NavMeshAgent enemy;
//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {
//        //target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        transform.position = Vector3.MoveTowards(this.transform.position, targetObj.Position, 10 * Time.deltaTime);
//       //enemy.SetDestination(Player.position);
//    }
//}
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;         // Reference to the player's transform
    public float followRange = 10f;  // Distance at which the enemy starts following
    public float moveSpeed = 3f;     // How fast the enemy moves towards the player

    private void Update()
    {
        // Check the distance between the enemy and the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // If the player is within follow range, move towards the player
        if (distanceToPlayer <= followRange)
        {
            FollowPlayer();
        }
    }

    void FollowPlayer()
    {
        // Move towards the player's position
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
}

