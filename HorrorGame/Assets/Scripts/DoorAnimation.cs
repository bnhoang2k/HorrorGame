using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{

    public GameObject GameController;
    private Animator m_Animator;
    private bool open = false;

    // Start is called before the first frame update
    void Start() {
        // set the Animator reference
        m_Animator = GetComponent<Animator>();
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

    }
}
