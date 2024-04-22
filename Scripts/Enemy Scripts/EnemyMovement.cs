using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    // Scene Variables: Stuff we may need from the Scene
    public GameObject player;
    public GameObject playerCapsule;
    private Camera mainCamera;
    public Camera aiCamera;
    [SerializeField] private float baseStepSpeed;
    [SerializeField] AudioSource footstepAudioSource = default;
    [SerializeField] AudioClip[] woodClips = default;
    private float GetCurrentOffset => isSprinting ? baseStepSpeed * sprintStepMultiplier: baseStepSpeed;
    // Spatial Variables: Navigation, Physics, Raycast, etc.
    private NavMeshAgent navAgent;

    // Enemy Variables: Anything related to the enemy
    private EnemyBehavior enemyBehaviorController;
    private Animator animator;
    private bool isSprinting = false;
    [SerializeField] private float sprintStepMultiplier = 0.6f;
    private float footstepTimer;
    private float maxSpeed;

    // Player Interaction Variables: Anything related to the player-enemy interaction
    private float stopRadius;

    void Start()
    {
        // Scene Variables
        mainCamera = Camera.main;

        // Spatial Variables
        navAgent = GetComponent<NavMeshAgent>();

        // Enemy Variables
        enemyBehaviorController = GetComponent<EnemyBehavior>();
        animator = GetComponent<Animator>();
        FirstPersonController playerController = playerCapsule.GetComponent<FirstPersonController>();
        maxSpeed = playerController.MoveSpeed;
        baseStepSpeed = 0.5f;

        // Player Interaction Variables
        stopRadius = player.GetComponent<Reach>().arm_length + 1.0f;

        // Set the enemy to idle
        SetIdle();
    }
    void Update()
    {
        if (navAgent.speed > 0)
        {
            Handle_Footsteps();
        }
    }

    // Animator Fuctions
    public void SetIdle()
    {
        animator.SetBool("isIdle", true);
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
    }

    public void SetWalking()
    {
        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", true);
        animator.SetBool("isRunning", false);
    }

    public void SetRunning()
    {
        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", true);
    }

    public void NavSpeedToAnim(float speedValue)
    {
        if (speedValue == 0)
        {
            SetIdle();
        }
        else if (speedValue > 0 && speedValue <= maxSpeed)
        {
            SetWalking();
        }
        else if (speedValue > maxSpeed)
        {
            SetRunning();
        }
        navAgent.speed = speedValue;
    }

    // Find a path to the player and the stride accordingly
    public void MoveToPlayer()
    {
        /*
        The goal of this function is to create a path in which the enemy will try to sneak
        up behind the player.
        */
        Vector3 playerPosition = playerCapsule.transform.position;
        Vector3 playerForward = playerCapsule.transform.forward;
        Vector3 targetPosition = playerPosition - (playerForward * stopRadius);
        
        // Check if targetPosition is within player's field of view
        Vector3 fromPlayerToTargetDir = (targetPosition - playerPosition).normalized;
        float angle = Vector3.Angle(playerForward, fromPlayerToTargetDir);
        float playerFOV = mainCamera.fieldOfView;
        if (angle <= playerFOV / 2)
        {
            // If within FOV, find a new point to approach from
            NavSpeedToAnim(0.0f);
            navAgent.isStopped = true;
            navAgent.SetDestination(transform.position);
            return;
        }

        // Set the destination of the enemy to the target position
        navAgent.SetDestination(targetPosition);
        if ((Vector3.Distance(transform.position, playerPosition) > stopRadius) && !enemyBehaviorController.GetPlayerDetected())
        {
            NavSpeedToAnim(maxSpeed - 2.5f);
        }
        else
        {
            NavSpeedToAnim(0.0f);
            navAgent.isStopped = true;
            navAgent.SetDestination(transform.position);
            enemyBehaviorController.SetStalking(true);
        }
    }

    public void HuntPlayer()
    {
        // Set the destination of the enemy to the player's position
        navAgent.destination = playerCapsule.transform.position; // Set the destination only once or conditionally.
        navAgent.isStopped = false;
        NavSpeedToAnim(maxSpeed * 1.5f);
        enemyBehaviorController.KillClose();
    }

    public void UpdateHunting()
    {
        if (Vector3.Distance(navAgent.destination, playerCapsule.transform.position) > 0.5f)
        {
            navAgent.ResetPath();
            navAgent.SetDestination(playerCapsule.transform.position);
        }
        enemyBehaviorController.KillClose();
    }

    // Makes the enemy rotate to look at the player when called
    public void LookAtPlayer()
    {
        Vector3 playerDirection = playerCapsule.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(playerDirection);
        if (playerDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2.0f);
        }
    }

    private void Handle_Footsteps()
    {
        footstepTimer -= Time.deltaTime;
			if (footstepTimer <= 0)
			{
				if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 3))
				{
					footstepAudioSource.PlayOneShot(woodClips[Random.Range(0, woodClips.Length - 1)]);
				}
				footstepTimer = GetCurrentOffset;
			}
    }
}
