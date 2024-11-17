using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAI : MonoBehaviour
{
    public Transform[] patrolPoints; // Points for the guard to patrol
    public float chaseDistance = 10f; // Distance at which the guard will chase the player
    public float speed = 2f; // Speed of the guard
    private Transform player;
    private int currentPatrolIndex = 0;
    private bool isChasing = false;

    private void Update()
    {
        if (isChasing && player != null)
        {
            // Chase the player
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, player.position) > chaseDistance)
            {
                StopChasing();
            }
        }
        else
        {
            Patrol();
        }
    }

    public void ChasePlayer(Transform target)
    {
        player = target;
        isChasing = true;
    }

    private void StopChasing()
    {
        player = null;
        isChasing = false;
    }

    private void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }
}
