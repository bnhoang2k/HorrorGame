using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshSurface))]
public class NavMeshBaker : MonoBehaviour
{
    private NavMeshSurface navMeshSurface;
    [SerializeField] private float voxelSize = 0.1666667f;
    // Start is called before the first frame update
    void Start()
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
}
