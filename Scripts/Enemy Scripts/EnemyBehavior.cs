using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    // Scene Variables
    public GameObject player;
    public GameObject playerCapsule;
    private Camera playerCamera;
    public GameObject gameController;
    public Camera aiHead;
    private GameObject[] insideSpawns;
    private GameObject[] outsideSpawns;

    // Spatial Variables
    private bool outside = false;
    private NavMeshAgent agent;

    public GameObject HoldingCell;

    // Enemy Variables
    private EnemyMovement enemyMovementController;
    public float viewDistance = 30.0f;

    private bool isGazing = false;
    private float gazeTimer = 0.0f;
    public float gazeDuration = 5.0f;

    // Player Interaction Variables
    private bool enemyDetected = false;
    private bool playerDetected = false;

    private float timeStalked = 0.0f;
    public float killTime = 5.0f;

    // Misc. Variables
    private System.Random random = new System.Random();
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyMovementController = GetComponent<EnemyMovement>();
        insideSpawns = GameObject.FindGameObjectsWithTag("Spawn/Inside");
        outsideSpawns = GameObject.FindGameObjectsWithTag("Spawn/Outside");
        HoldingCell = GameObject.Find("HoldingCell");
        insideSpawns = Array.FindAll(insideSpawns, spawn => spawn != HoldingCell);
        outsideSpawns = Array.FindAll(outsideSpawns, spawn => spawn != HoldingCell);
        playerCamera = Camera.main;
        EnemyTeleport();
    }

    // Update is called once per frame
    void Update()
    {
        // Continuously check for the player
        // If so, initialize second movement phase
        enemyMovementController.LookAtPlayer();
        CheckForPlayer();
        CheckForEnemy();
        // If the enemy hasn't been detected, the enemy will continously move towards the player.
        if (!enemyDetected && !playerDetected)
        {
            enemyMovementController.MoveToPlayer();
        }
        if (playerDetected)
        {
            // TODO: Play music or other indicator in the future?
            enemyMovementController.SetIdle();
            StalkPlayer();
        }
    }

    public bool DetermineSpawn()
    {
        outside = random.Next(2) == 1;
        return outside;
    }

    public GameObject FindSpawnLocation(bool outside)
    {
        float playerFOV = player.GetComponentInChildren<Camera>().fieldOfView;
        Vector3 playerDirection = player.transform.forward;
        Vector3 playerPosition = player.transform.position;

        GameObject[] spawnLocations = outside ? outsideSpawns : insideSpawns;
        foreach (GameObject spawn in spawnLocations)
        {
            Vector3 directionToSpawn = spawn.transform.position - playerPosition;
            float angle = Vector3.Angle(directionToSpawn, playerDirection);
            if (angle > playerFOV / 2)  // Check if spawn is outside of player's field of view
            {
                return spawn;
            }
        }
        Debug.Log("No suitable spawn location found");
        return null;  // Return null if no valid location is found
    }


    void EnemyTeleport()
    {
        // Figure out what to do without outside spawns later
        // outside = DetermineSpawn();
        outside = false;
        GameObject spawnLocation = FindSpawnLocation(outside);
        agent.Warp(spawnLocation.transform.position);
    }

    void CheckForPlayer()
    {
        // Check if the player is within the enemy's field of view
        Vector3 playerDirection = playerCapsule.transform.position - transform.position;
        float angle = Vector3.Angle(playerDirection, transform.forward);
        if (angle < viewDistance)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, playerDirection, out hit, viewDistance))
            {
                if (hit.collider.gameObject == playerCapsule)
                {
                    playerDetected = true;
                }
                else
                {
                    playerDetected = false;
                }
            }
        }
        else
        {
            playerDetected = false;
        }
    }

    void CheckForEnemy()
    {
        RaycastHit hit;
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, player.GetComponent<Reach>().arm_length))
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
    }

    void StalkPlayer()
    {
        bool holdingEye = gameController.GetComponent<InventoryManagement>().holdingItem("Eye_Describable");
        // TODO: Fix this
        // if (playerDetected && !enemyDetected)
        // {
        //     isGazing = true;
        //     timeStalked += Time.deltaTime;
        // }
        if (holdingEye && playerDetected)
        {
            isGazing = true;
            timeStalked += Time.deltaTime * 3;
        }

        if (isGazing && (!holdingEye || enemyDetected))
        {
            gazeTimer += Time.deltaTime;
        }

        if (timeStalked >= killTime)
        {
            enemyMovementController.HuntPlayer();
        }
        if (gazeTimer >= gazeDuration)
        {
            RestartCycle();
        }
    }

    void RestartCycle()
    {
        gazeTimer = 0.0f;
        playerDetected = false;
        enemyDetected = false;
        isGazing = false;
        timeStalked = 0.0f;
        EnemyTeleport();
    }

    // TODO: Add more here in the future
    public void KillPlayer()
    {
        Destroy(player);
    }

    /* TODO: Leftover code from the description within your text. Need to figure if we want to scrap this
    or not.
    */
    // GameObject FindNearestWindow()
    // {
    //     GameObject[] windows = GameObject.FindGameObjectsWithTag("Window");
    //     GameObject closest = null;
    //     float distance = Mathf.Infinity;
    //     Vector3 position = transform.position;
    //     foreach (GameObject window in windows)
    //     {
    //         Vector3 diff = window.transform.position - position;
    //         float curDistance = diff.sqrMagnitude;
    //         if (curDistance < distance)
    //         {
    //             closest = window;
    //             distance = curDistance;
    //         }
    //     }
    //     return closest;
    // }

    //  IEnumerator LookThroughWindow(Transform windowTransform)
    //     {
    //         // Wait until the agent reaches the window
    //         yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);
    //         isGazing = true;
    //         // Look at the window
    //         Vector3 directionToWindow = windowTransform.position - aiHead.transform.position;
    //         directionToWindow.y = 0; // This keeps the enemy's rotation only on the horizontal plane

    //         yield return new WaitForSeconds(gazeDuration);

    //     }

}
