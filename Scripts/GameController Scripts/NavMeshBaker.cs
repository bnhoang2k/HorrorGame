using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshSurface))]
public class NavMeshBaker : MonoBehaviour
{
    private NavMeshSurface navMeshSurface;
    public GameObject[] doors;
    [SerializeField] private float voxelSize = 0.1666667f;
    // Start is called before the first frame update
    void Start()
    {
        BakeNavMesh();
    }

    // For some reason, it keeps resetting when we remove the data so we have to create a function
    // has all of ours settings
    public void BakeNavMesh()
    {
        navMeshSurface = GetComponent<NavMeshSurface>();
        // Set the NavMesh Agent Type to be "Humanoid"
        navMeshSurface.agentTypeID = NavMesh.GetSettingsByIndex(0).agentTypeID;
        // Set the NavMesh default area to "Walkable"
        navMeshSurface.defaultArea = 0;
        // Set NavMeshSurface to only collect objects from layer "NavMeshLayer"
        navMeshSurface.layerMask = LayerMask.GetMask("NavMeshLayer");
        // Override the Voxel Count to be voxelSize
        navMeshSurface.overrideVoxelSize = true;
        navMeshSurface.voxelSize = voxelSize;
        // Set the Use Geometry to "Physics Colliders"
        navMeshSurface.useGeometry = NavMeshCollectGeometry.PhysicsColliders;
        // Bake the NavMesh
        navMeshSurface.BuildNavMesh();
    }

    // Due to the way the NavMeshSurface works, we need to bake the NavMesh every time the doors are opened or closed
    // Our method of NavMeshSurface baking will depend on the GameObject's layer, so we need to change the layer of the doors
    // to "Default" when they are opened and back to "NavMeshLayer" when they are closed.
    // Plan is to call it from the Openable script IF it's a door and call the rebake function.
    public void LayerChanger(GameObject currentDoor, string layer)
    {
        currentDoor.layer = LayerMask.NameToLayer(layer);
        foreach (Transform child in currentDoor.transform)
        {
            LayerChanger(child.gameObject, layer);
        }
    }
    public void ReBakeNavMesh()
    {
        navMeshSurface.RemoveData();
        // Make the agent re-aware of the new changes
        NavMeshAgent enemyAgent = GameObject.Find("Enemy").GetComponent<NavMeshAgent>();
        enemyAgent.ResetPath();

        BakeNavMesh();
    }
}
