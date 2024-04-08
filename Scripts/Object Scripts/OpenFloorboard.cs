using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenFloorboard : MonoBehaviour
{
    // All openable objects need an Animator on the root object
    // The root_name is the name of which object has the Animator
    // The Animator must have a boolean "open" that controls the animation

    // Any other objects need to have it harcoded to call their unique script

    public GameObject solid_floor;
    private GameObject GameController;
    public bool open = false;
    public bool locked = true;
    private Camera player_camera;
    private float arm_length;
    private string open_message = "Press E to pry up";

    // Start is called before the first frame update
    void Start()
    {
        // find the game controller
        GameController = GameObject.Find("GameController");
        // find the camera and arm length
        player_camera = Camera.main;
        arm_length = GameObject.Find("Player").GetComponent<Reach>().arm_length;
    }

    // Update is called once per frame
    void Update() {}

    public void Open() {
        
        // the object is unlocked, so it can be opened
        if (!locked && !open) {
            open = !open;
            Destroy(solid_floor);

            gameObject.GetComponent<Describable>().description = "Hole in the floor";
            
        } else if (locked) {
            Debug.Log("Need to add sound and implement 'unlocking' the floorboard in OpenFloorBoard.cs");
            // TODO: change the description from Floor to Hollow Floorboard once the player makes it make a sound
        }
        
    }

    private void OnMouseOver() {
    // called every frame while the mouse is over an object's collider

        // Raycast generates a ray from the origin in the direction of the camera
        // and returns true if it hits something.
        RaycastHit hit;
        Ray ray = player_camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, arm_length)) {

            OpenFloorboard floorboard = hit.collider.GetComponent<OpenFloorboard>();
            // if the object is within reach, the player can feel it
            if (floorboard)
            {
                if (!locked && !open) {
                    GameController.GetComponent<InstructionGUI>().setMessage(open_message);
                }
            }
        } else {
            // object is out of reach
            GameController.GetComponent<InstructionGUI>().setMessage("");
        }        
    }

    private void OnMouseExit() {
    // called when the mouse exits an object's collider

        // call a function from the script GUIDisplay to set the message
        GameController.GetComponent<InstructionGUI>().setMessage("");

    }
}
