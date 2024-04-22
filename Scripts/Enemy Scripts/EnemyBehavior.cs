using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    // Scene Variables: Stuff we may need from the Scene
    public GameObject player;
    public GameObject playerCapsule;
    private Camera mainCamera;
    public Camera aiCamera;
    public GameObject gameController;
    private GameObject[] insideSpawns;
    private GameObject[] outsideSpawns;
    public GameObject HoldingCell;

    // Spatial Variables: Navigation, Physics, Raycast, etc.
    private NavMeshAgent navAgent;
    private bool outside = false;

    // Enemy Variables: Anything related to the enemy
    private EnemyMovement enemyMovementController;
    public float detectionDistance = 30.0f;
    public float killTime = 5.0f;
    private float gazeTimer = 0.0f;
    public float gazeDuration = 5.0f;
    public float cooldownTimer = 10.0f;

    // Player Interaction Variables: Anything related to the player-enemy interaction
    private bool isHunting = false;
    private bool enemyDetected = false;
    private bool playerDetected = false;
    private bool isStalking = false;
    private float timeStalked = 0.0f;

    // Misc. Variables
    private System.Random random;

    void Start()
    {
        // Scene Variables
        mainCamera = Camera.main;
        insideSpawns = GameObject.FindGameObjectsWithTag("Spawn/Inside");
        outsideSpawns = GameObject.FindGameObjectsWithTag("Spawn/Outside");
        insideSpawns = Array.FindAll(insideSpawns, spawn => spawn != HoldingCell);
        outsideSpawns = Array.FindAll(outsideSpawns, spawn => spawn != HoldingCell);

        // Spatial Variables
        navAgent = GetComponent<NavMeshAgent>();

        // Enemy Variables
        enemyMovementController = GetComponent<EnemyMovement>();

        // Misc. Variables
        random = new System.Random(Environment.TickCount);

        // Teleport to a valid spawn location
        Teleport();
    }

    void Update()
    {
        enemyMovementController.LookAtPlayer();
        CheckForPlayer();
        CheckForEnemy();
        if (!enemyDetected && !isStalking && !isHunting)
        {
            enemyMovementController.MoveToPlayer();
        }
        else if (isStalking)
        {
            StalkPlayer();
            Debug.Log("Stalking Player: " + timeStalked + " seconds | Gaze Timer: " + gazeTimer + " seconds");
        }
        if (timeStalked >= killTime)
        {
            isStalking = false;
            if (!isHunting)
            {
                isHunting = true;
                enemyMovementController.HuntPlayer();
            }
        }
        if (gazeTimer >= gazeDuration)
        {
            isStalking = false;
            RestartCycle();
        }
        if (isHunting)
        {
            enemyMovementController.UpdateHunting();
        }
    }

    // Get enemy detected
    public bool GetEnemyDetected() {return enemyDetected;}
    
    // Get player detected
    public bool GetPlayerDetected() {return playerDetected;}

    // Change value of isStalking
    public void SetStalking(bool value) {isStalking = value;}

    // Find suitable spawn location for the enemy
    public Vector3 FindSpawnLocation()
    {
        GameObject playerTile = FindPlayerTile();
        if (!playerTile) {
            Debug.Log("Player tile not found");
            return Vector3.zero;
        }

        Collider collider = playerTile.GetComponent<Collider>();
        if (!collider) {
            Debug.Log("Collider not found");
            return Vector3.zero;
        }

        Bounds tileBounds = collider.bounds;
        Vector3 playerPosition = playerCapsule.transform.position;
        Vector3 playerForward = mainCamera.transform.forward;
        float spawnDistance = 5.0f; // Adjustable

        Vector3 potentialSpawn = playerPosition - (playerForward * spawnDistance);
        Debug.Log("Potential Spawn: " + potentialSpawn);
        if (tileBounds.Contains(potentialSpawn)) {return potentialSpawn;}
        else {return tileBounds.ClosestPoint(potentialSpawn);}
    }

    // Find the tile the player is currently on
    public GameObject FindPlayerTile()
{
    Vector3 playerPosition = playerCapsule.transform.position;
    foreach (GameObject spawn in insideSpawns)
    {
        BoxCollider collider = spawn.GetComponent<BoxCollider>();
        Bounds bounds = collider.bounds;
        // Adjust the bounds to effectively make them a "2D" area at the player's height
        bounds.Expand(new Vector3(0, 1000f, 0));  // Expand the bounds infinitely along the Y axis

        if (bounds.Contains(new Vector3(playerPosition.x, collider.transform.position.y, playerPosition.z)))
        {
            return spawn;
        }
    }
    foreach (GameObject spawn in outsideSpawns)
    {
        BoxCollider collider = spawn.GetComponent<BoxCollider>();
        Bounds bounds = collider.bounds;
        // Adjust the bounds to effectively make them a "2D" area at the player's height
        bounds.Expand(new Vector3(0, 1000f, 0));  // Expand the bounds infinitely along the Y axis

        if (bounds.Contains(new Vector3(playerPosition.x, collider.transform.position.y, playerPosition.z)))
        {
            return spawn;
        }
    }
    return null;
}

    // Teleports the enemy to a valid spawn location
    void Teleport() {navAgent.Warp(FindSpawnLocation());}

    // Check if the player is within the enemy's field of view
    public void CheckForPlayer()
    {
        Vector3 playerDirection = playerCapsule.transform.position - aiCamera.transform.position;
        float angle = Vector3.Angle(aiCamera.transform.forward, playerDirection.normalized);

        if (angle < aiCamera.fieldOfView / 2)
        {
            RaycastHit hit;
            if (Physics.Raycast(aiCamera.transform.position, playerDirection.normalized, out hit, detectionDistance))
            {
                if (hit.collider.gameObject == playerCapsule)
                {
                    isStalking = true;
                    playerDetected = true;
                }
                else
                {
                    isStalking = false;
                    playerDetected = false;
                }
            }
        }
        else
        {
            isStalking = false;
            playerDetected = false;
        }
    }

    // Check for the enemy
    public void CheckForEnemy()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        float playerDetectionDistance = gameController.GetComponent<InventoryManagement>().holdingItem("Eye_Describable")
                                        ? detectionDistance : player.GetComponent<Reach>().arm_length;
        
        if (Physics.Raycast(ray, out hit, playerDetectionDistance))
        {
            if (hit.collider.gameObject == gameObject)
            {
                enemyDetected = true;
            }
            else
            {
                enemyDetected = false;
            }
        }
        else
        {
            enemyDetected = false;
        }
    }

    // Stalk the player
    public void StalkPlayer()
    {
        bool holdingEye = gameController.GetComponent<InventoryManagement>().holdingItem("Eye_Describable");
        if (enemyDetected)
        {
            gazeTimer += Time.deltaTime;
        }
        if (holdingEye)
        {
            timeStalked += Time.deltaTime * 7;
        }
        else if (!holdingEye && playerDetected && !enemyDetected)
        {
            timeStalked += Time.deltaTime;
        }
    }

    public void KillClose()
    {
        if (Vector3.Distance(transform.position, playerCapsule.transform.position) < 0.5f)
        {
            KillPlayer();
        }
    }

    public void KillPlayer()
    {
        EditorApplication.isPlaying = false;
        EditorApplication.ExitPlaymode();
    }

    public void RestartCycle()
    {
        gazeTimer = 0.0f;
        timeStalked = 0.0f;
        isStalking = false;
        isHunting = false;
        playerDetected = false;
        enemyDetected = false;
        
        // Warp back to holding HoldingCell
        navAgent.Warp(HoldingCell.transform.position);

        // Wait until spawnTimer has passed to teleport again.
        StartCoroutine(WaitForTeleport());
    }

    IEnumerator WaitForTeleport()
    {
        yield return new WaitForSeconds(cooldownTimer);
        Teleport();
    }
}
