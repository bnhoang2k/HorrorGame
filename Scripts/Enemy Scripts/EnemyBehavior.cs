using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;

public class EnemyBehavior : MonoBehaviour
{
    // Scene Variables: Stuff we may need from the Scene
    public GameObject player;
    public GameObject playerCapsule;
    public GameObject enemyRoot;
    private Camera mainCamera;
    public Camera aiCamera;
    public GameObject gameController;
    private GameObject[] insideSpawns;
    private GameObject[] outsideSpawns;
    public GameObject HoldingCell;

    // Spatial Variables: Navigation, Physics, Raycast, etc.
    private NavMeshAgent navAgent;

    // Enemy Variables: Anything related to the enemy
    private EnemyMovement enemyMovementController;
    public float detectionDistance = 30.0f;
    public float killTime = 5.0f;
    private float gazeTimer = 0.0f;
    public float gazeDuration = 5.0f;
    public float cooldownTimer = 10.0f;
    private bool isTeleporting = false;

    // Player Interaction Variables: Anything related to the player-enemy interaction
    private bool isHunting = false;
    private bool objectDetected(Camera c, GameObject obj)
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(c);
        var point = obj.transform.position;
        foreach (var plane in planes)
        {
            if (plane.GetDistanceToPoint(point) < 0)
            {
                return false;
            }
        }
        RaycastHit hit;
        var rayStart = c.transform.position + c.transform.forward * 0.1f;
        if (Physics.Raycast(rayStart, point - rayStart, out hit))
        {
            if (hit.collider.gameObject == obj)
            {
                return true;
            }
        }
        return false;
    }
    private bool playerDetected = false;
    private bool enemyDetected = false;
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
        StartCoroutine(WaitForTeleport());
    }

    void Update()
    {
        // Debug.Log("Enemy Detected: " + enemyDetected + " | Player Detected: " + playerDetected);
        CheckForPlayer();
        CheckForEnemy();
        GazeEnemy();
        if (navAgent.remainingDistance < navAgent.stoppingDistance && navAgent.hasPath)
        {
            enemyMovementController.LookAtPlayer();
        }
        if (playerDetected)
        {
            navAgent.ResetPath();
            navAgent.isStopped = true;
        }
        if (enemyDetected && !isHunting) {enemyMovementController.FreezeEnemy();}
        if (!enemyDetected && !isStalking && !isHunting && !playerDetected) {enemyMovementController.MoveBehindPlayer();}
        else if (isStalking)
        {
            enemyMovementController.LookAtPlayer();
            StalkPlayer();
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
        if (isHunting) {enemyMovementController.UpdateHunting();}
        if (FindPlayerTile() != FindEnemyTile() && FindEnemyTile() != HoldingCell && !isTeleporting) {StartCoroutine(WaitForTeleport());}
    }
    
    private void CheckForPlayer()
    {
        if (objectDetected(aiCamera, playerCapsule))
        {
            playerDetected = true;
            isStalking = true;
        }
        else
        {
            playerDetected = false;
            isStalking = false;
        }
    }

    private void CheckForEnemy()
    {
        if (objectDetected(mainCamera, enemyRoot)) {enemyDetected = true;}
        else {enemyDetected = false;}
    }

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
        Vector3 potentialSpawn = Vector3.zero;
        bool valid = false;
        for (int i = 0; i < 100; i++)
        {
            potentialSpawn = new Vector3(
                UnityEngine.Random.Range(tileBounds.min.x, tileBounds.max.x), 
                0, 
                UnityEngine.Random.Range(tileBounds.min.z, tileBounds.max.z));

            if (Vector3.Distance(playerPosition, potentialSpawn) > 5.0f) {valid = true; break;}
        }

        if (tileBounds.Contains(potentialSpawn) && valid) {return potentialSpawn;}
        // else {return tileBounds.ClosestPoint(potentialSpawn);}
        else {return Vector3.zero;}
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

    public GameObject FindEnemyTile()
    {
        Vector3 enemyPosition = enemyRoot.transform.position;
        foreach (GameObject spawn in insideSpawns)
        {
            BoxCollider collider = spawn.GetComponent<BoxCollider>();
            Bounds bounds = collider.bounds;
            // Adjust the bounds to effectively make them a "2D" area at the player's height
            bounds.Expand(new Vector3(0, 1000f, 0));  // Expand the bounds infinitely along the Y axis

            if (bounds.Contains(new Vector3(enemyPosition.x, collider.transform.position.y, enemyPosition.z)))
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

            if (bounds.Contains(new Vector3(enemyPosition.x, collider.transform.position.y, enemyPosition.z)))
            {
                return spawn;
            }
        }
        return null;
    }

    // Teleports the enemy to a valid spawn location
    void Teleport() 
    {
        navAgent.Warp(FindSpawnLocation());
    }

    // Check if the player is within the enemy's field of view

    // Stalk the player
    public void StalkPlayer()
    {
        bool holdingEye = gameController.GetComponent<InventoryManagement>().holdingItem("Eye_Describable");
        if (holdingEye)
        {
            timeStalked += Time.deltaTime * 7;
        }
        else if (!holdingEye && playerDetected && !enemyDetected)
        {
            timeStalked += Time.deltaTime;
        }
    }
    public void GazeEnemy()
    {
        float playerDetectionDistance = gameController.GetComponent<InventoryManagement>().holdingItem("Eye_Describable")
                                        ? detectionDistance : player.GetComponent<Reach>().arm_length;
        RaycastHit ray;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out ray, playerDetectionDistance))
        {
            if (ray.collider.gameObject == enemyRoot)
            {
                gazeTimer += Time.deltaTime;
            }
        }
    }

    public void RestartCycle()
    {
        gazeTimer = 0.0f;
        timeStalked = 0.0f;
        isStalking = false;
        isHunting = false;
        playerDetected = false;
        enemyDetected = false;
        isTeleporting = false;
        
        // Warp back to holding HoldingCell
        navAgent.Warp(HoldingCell.transform.position);

        // Wait until spawnTimer has passed to teleport again.
        StartCoroutine(WaitForTeleport());
    }

    IEnumerator WaitForTeleport()
    {
        isTeleporting = true;
        yield return new WaitForSeconds(cooldownTimer);
        Teleport();
        isTeleporting = false;
    }
}
