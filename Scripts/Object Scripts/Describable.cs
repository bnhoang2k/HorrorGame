using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Describable : MonoBehaviour {

    private string default_message = "You can't feel anything here";
    public string description = "EMPTY";
    private GameObject GameController;
    private Camera player_camera;
    private float arm_length;
    public bool isPickupable = false;
    public bool isOpenable = false;
    public bool isUniqueOpenable = false;

    private void Start() {
        GameController = GameObject.Find("GameController");
        player_camera = Camera.main;
        arm_length = GameObject.Find("Player").GetComponent<Reach>().arm_length;
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
            }
        } else {
            // object is out of reach
            GameController.GetComponent<DescribableGUI>().setMessage(default_message);
        }        
    }

    private void OnMouseExit() {
    // called when the mouse exits an object's collider

        // call a function from the script GUIDisplay to set the message
        GameController.GetComponent<DescribableGUI>().setMessage(default_message);

    }
}
