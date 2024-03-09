using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{

    public GameObject GameController;
    private Animator m_Animator;
    private bool open = false;
    private float x_rotation;
    private float y_rotation;
    private float z_rotation;

    // Start is called before the first frame update
    void Start() {
        // set the Animator reference
        m_Animator = GetComponent<Animator>();

        // store current door rotation
        x_rotation = gameObject.transform.eulerAngles.x;
        y_rotation = gameObject.transform.eulerAngles.y;
        z_rotation = gameObject.transform.eulerAngles.z;
        
    }

    // Update is called once per frame
    void FixedUpdate() {
        
        // Using a function from GameController,
        // check if the player has a key to open the door with
        if (GameController.GetComponent<InventoryManagement>().hasKey()) {

            // check if the player hit the button to open the door
            if (Input.GetKeyDown(KeyCode.E)) {
                open = !open;
                m_Animator.SetBool("open", open);
            }
        }

        // // make sure the door is still rotated correctly
        // // animation will play at prefab rotation, not instance
        // TODO: still not working right
        // gameObject.transform.Rotate(x_rotation, y_rotation, z_rotation, Space.World);
    }
}
