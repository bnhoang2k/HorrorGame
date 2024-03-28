using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Describable : MonoBehaviour {

    private string default_message = "You can't feel anything here";
    public string description = "EMPTY";
    private GameObject GameController;
    private Camera player_camera;
    private GameObject player;
    private float arm_length;
    public bool isPickupable = false;
    public bool isOpenable = false;
    private string unlock = "Press E to unlock";
    private string open = "Press E to open";
    private string grab = "Press E to pick up";

    private void Start() {
        GameController = GameObject.Find("GameController");
        player_camera = Camera.main;
        player = GameObject.Find("Player");
        arm_length = player.GetComponent<Reach>().arm_length;
    }
    private void Update() {}

    private void OnMouseOver() {
    // called every frame while the mouse is over an object's collider

        // Raycast generates a ray from the origin in the direction of the camera
        // and returns true if it hits something.
        RaycastHit hit;
        Ray ray = player_camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, arm_length)) {

            Describable describable = hit.collider.GetComponent<Describable>();
            // if the object is within reach, the player can feel it
            if (describable)
            {
                // call a function from the script GUIDisplay to set the message
                GameController.GetComponent<DescribableGUI>().setMessage(description);

                if (isOpenable) {
                    bool locked = gameObject.GetComponent<Openable>().locked;

                    if (locked && GameController.GetComponent<InventoryManagement>().holdingItem(gameObject.GetComponent<Openable>().key)) {
                        GameController.GetComponent<InstructionGUI>().setMessage(unlock);
                    } else {
                        GameController.GetComponent<InstructionGUI>().setMessage(open);
                    }
                }
                else if (isPickupable) {
                    GameController.GetComponent<InstructionGUI>().setMessage(grab);
                }
            }
        } else {
            // object is out of reach
            GameController.GetComponent<DescribableGUI>().setMessage(default_message);
            GameController.GetComponent<InstructionGUI>().setMessage("");
        }        
    }

    private void OnMouseExit() {
    // called when the mouse exits an object's collider

        // call a function from the script GUIDisplay to set the message
        GameController.GetComponent<DescribableGUI>().setMessage(default_message);
        GameController.GetComponent<InstructionGUI>().setMessage("");

    }
}
