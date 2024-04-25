using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Test : MonoBehaviour
{
    public Animator animator;
    NavMeshAgent navAgent;
    
    private Vector2 velocity;
    private Vector2 smoothDeltaPosition;
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator.applyRootMotion = true;
        navAgent.updatePosition = false;
        navAgent.updateRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Running");
            SetRunning();
            navAgent.SetDestination(GameObject.Find("PlayerCapsule").transform.position);
        }
        // MoveToTarget();
        SynchronizeAnimatorAndAgent();
    }
    public void MoveToPlayer()
    {
        SetWalking();
        navAgent.SetDestination(GameObject.Find("PlayerCapsule").transform.position);
    }
    
    public void MoveToTarget()
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

        SetWalking();
        navAgent.SetDestination(targetPosition);
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
        if (deltaMagnitude > navAgent.radius / 2.0f)
        {
            transform.position = Vector3.Lerp(animator.rootPosition, navAgent.nextPosition, smooth);
        }
    }

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

}
