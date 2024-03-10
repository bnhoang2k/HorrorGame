using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E)) { Grab(); }
    }

    void Grab()
    {
        // Raycast generates a ray from the origin in the direction of the camera
        // and returns true if it hits something.
        // We want to check if the player is looking at an object that can be picked up.
        // If it's describable, we want to "grab" it, remove it from the environment, and add it to the inventory.
        // 2f is adjustable, but ideally we want it to be arm length.
        RaycastHit hit;

        // Debug raycast
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(transform.position, forward, Color.green);

        if (Physics.Raycast(transform.position, transform.forward, out hit, 3f))
        {
            Describable describable = hit.collider.GetComponent<Describable>();
            if (describable && describable.isPickupable)
            {
                // Debug.Log("Grabbed " + describable.gameObject.name);
                Actions.UpdateInventory(describable);
                Destroy(describable.gameObject);
            }
        }

    }

}