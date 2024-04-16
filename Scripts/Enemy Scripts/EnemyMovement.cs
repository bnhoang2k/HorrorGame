using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    // Scene Variables
    public GameObject player;
    public GameObject playerCapsule;
    public Camera aiHead;

    // Spatial Variables
    private NavMeshAgent navAgent;

    // Enemy Variables
    private EnemyBehavior enemyBehaviorController;
    private Animator animator;

    // Player Interaction Variables
    public float stopRadius = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        enemyBehaviorController = GetComponent<EnemyBehavior>();
        animator.SetBool("isIdle", true);
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
    }
    void Update()
    {

    }

    public void SetIdle()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isIdle", true);
    }

    public void SetWalking()
    {
        animator.SetBool("isWalking", true);
        animator.SetBool("isRunning", false);
        animator.SetBool("isIdle", false);
    }

    public void SetRunning()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", true);
        animator.SetBool("isIdle", false);
    }

    public void MoveToPlayer()
    {
        Vector3 playerPosition = playerCapsule.transform.position;
        Vector3 playerForward = playerCapsule.transform.forward;
        float playerFOV = player.GetComponentInChildren<Camera>().fieldOfView;
        Vector3 toPlayer = transform.position - playerPosition;

        // Calculate distance behind the player to sneak up to
        Vector3 targetPosition = (playerPosition - playerForward) * stopRadius;

        // Check if the point is within the player's fieldOfView
        if (Vector3.Angle(-toPlayer, playerForward) <= playerFOV / 2)
        {
            // If within FOV, find a new point to approach from
            targetPosition = playerPosition + Quaternion.Euler(0, 90, 0) * -playerForward * stopRadius * 1.5f; // Approach from side
        }

        // Check distance and set navigation
        if (Vector3.Distance(transform.position, playerPosition) > stopRadius)
        {
            navAgent.SetDestination(targetPosition);
            SetWalking();
        }
        else
        {
            SetIdle();
            navAgent.ResetPath();  // Stop the NavMeshAgent from moving
        }
    }

    public void LookAtPlayer()
    {
        Vector3 playerPosition = playerCapsule.transform.position;
        Vector3 enemyPosition = aiHead.transform.position;

        // Zero out the y component to ignore height differences
        playerPosition.y = enemyPosition.y;

        Vector3 lookDirection = playerPosition - enemyPosition;
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
    
        // Apply the rotation only if there is a valid direction (not zero)
        if (lookDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f);
        }
    }


    public void HuntPlayer()
    {
        SetRunning();
        stopRadius = 0;
        navAgent.speed += 2;
        navAgent.SetDestination(playerCapsule.transform.position);
        // Once the enemy touches the player, the player dies
        // Figure this out again this week
        if (Vector3.Distance(transform.position, playerCapsule.transform.position) < 1.0f)
        {
            Debug.Log("you died.");
        }
    }

}
