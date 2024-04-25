using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    Walking,
    Running
}
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
    private Vector2 velocity;
    private Vector2 smoothDeltaPosition;

    // Enemy Variables: Anything related to the enemy
    private EnemyBehavior enemyBehaviorController;
    public Animator animator;
    private bool isSprinting = false;
    [SerializeField] private float sprintStepMultiplier = 0.6f;
    private float footstepTimer;

    // Player Interaction Variables: Anything related to the player-enemy interaction
    private float stopRadius;

    void Start()
    {
        // Scene Variables
        mainCamera = Camera.main;

        // Spatial Variables
        navAgent = GetComponent<NavMeshAgent>();
        animator.applyRootMotion = true;
        navAgent.updatePosition = false;
        navAgent.updateRotation = true;

        // Enemy Variables
        enemyBehaviorController = GetComponent<EnemyBehavior>();
        FirstPersonController playerController = playerCapsule.GetComponent<FirstPersonController>();
        baseStepSpeed = 0.5f;

        // Player Interaction Variables
        stopRadius = player.GetComponent<Reach>().arm_length;

        // Set the enemy to idle
        SetIdle();
    }
    void Update()
    {
        // LookAtPlayer();
        SynchronizeAnimatorAndAgent();
        Handle_Footsteps();
    }

    // Animator Fuctions
    private void SetIdle()
    {
        animator.SetBool("isIdle", true);
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
    }

    private void SetWalking()
    {
        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", true);
        animator.SetBool("isRunning", false);
    }

    private void SetRunning()
    {
        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", true);
    }

    // Find a path to the player and the stride accordingly
    public void MoveBehindPlayer()
    {
        Vector3 playerPosition = GameObject.Find("PlayerCapsule").transform.position;
        Vector3 playerForward = GameObject.Find("PlayerCapsule").transform.forward;
        Vector3 targetPosition = playerPosition - (playerForward * 3.0f);

        Vector3 fromPlayerToTargetDir = (targetPosition - playerPosition).normalized;
        float angle = Vector3.Angle(playerForward, fromPlayerToTargetDir);
        float playerFOV = Camera.main.fieldOfView;
        if (angle <= playerFOV / 2)
        {
            SetIdle();
            navAgent.isStopped = true;
            navAgent.SetDestination(transform.position);
            return;
        }    
        else
        {
            SetWalking();
            navAgent.SetDestination(targetPosition);
        }
    }

    public void Creep()
    {
        navAgent.isStopped = false;
        navAgent.SetDestination(playerCapsule.transform.position);
        SetWalking();
    }

    public void FreezeEnemy()
    {
        navAgent.isStopped = true;
        navAgent.ResetPath();
        SetIdle();
    }

    void OnAnimatorMove()
    {
        // This functions ensures that the root motion is applied to the navAgent
        // and the navAgent's current position updates to the root position
        // This is important to prevent foot sliding. Foot sliding occurs when the walking animation
        // doesn't match the navAgent's position.
        Vector3 rootPosition = animator.rootPosition;
        rootPosition.y = navAgent.nextPosition.y;
        transform.position = rootPosition;
        transform.rotation = animator.rootRotation;
        navAgent.nextPosition = transform.position;
    }

    private void SynchronizeAnimatorAndAgent()
    {
        // Not sure what the majority of this code does math-wise
        // https://www.youtube.com/watch?v=uAGjKxH4sDQ
        Vector3 worldDeltaPosition = navAgent.nextPosition - transform.position;
        worldDeltaPosition.y = 0;
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);

        Vector2 vector2 = new Vector2(dx, dy);
        float smooth = Mathf.Min(1, Time.deltaTime /0.1f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, vector2, smooth);
        if (navAgent.remainingDistance <= navAgent.stoppingDistance)
        {
            velocity = Vector2.Lerp(Vector2.zero,
                                    velocity,
                                    navAgent.remainingDistance /navAgent.stoppingDistance);
        }
        // Determines whether we should move or not
        bool shouldMove = velocity.magnitude > 0.0f && navAgent.remainingDistance > navAgent.stoppingDistance;
        if (shouldMove) {SetWalking();}
        else if (navAgent.remainingDistance <= navAgent.stoppingDistance) {SetIdle();}

        float deltaMagnitude = worldDeltaPosition.magnitude;
        if (deltaMagnitude > navAgent.radius / 3.0f)
        {
            transform.position = Vector3.Lerp(animator.rootPosition, navAgent.nextPosition, smooth);
        }
    }

    public void HuntPlayer()
    {
        // Set the destination of the enemy to the player's position
        if (navAgent.remainingDistance <= navAgent.stoppingDistance) { return; }
        navAgent.destination = playerCapsule.transform.position;
        SetRunning();
    }

    public void UpdateHunting()
    {
        Debug.Log("Updating Hunting");
        SetRunning();
        navAgent.SetDestination(playerCapsule.transform.position);
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

    public void Handle_Footsteps()
    {
        if (GetEnemyState() == EnemyState.Walking || GetEnemyState() == EnemyState.Running)
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

    public EnemyState GetEnemyState()
    {
        if (animator.GetBool("isIdle")) { return EnemyState.Idle; }
        else if (animator.GetBool("isWalking")) { return EnemyState.Walking; }
        else if (animator.GetBool("isRunning")) { return EnemyState.Running; }
        else { return EnemyState.Idle; }
    }
}
