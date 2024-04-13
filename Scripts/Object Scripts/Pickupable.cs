using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour
{
    public Texture icon;
    public int x = 0;
    public int y = -5;
    public float x_scale = 1;
    public float y_scale = 1;
    private GameObject GameController;
    private Camera player_camera;
    private float arm_length;
    private string grab_message = "Press E to pick up";

    // Start is called before the first frame update
    void Start()
    {
        GameController = GameObject.Find("GameController");
        player_camera = Camera.main;
        arm_length = GameObject.Find("Player").GetComponent<Reach>().arm_length;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     private void OnMouseOver() {
    // called every frame while the mouse is over an object's collider

        // Raycast generates a ray from the origin in the direction of the camera
        // and returns true if it hits something.
        RaycastHit hit;
        Ray ray = player_camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, arm_length)) {

            Pickupable pickupable = hit.collider.GetComponent<Pickupable>();
            // if the object is within reach, the player can feel it
            if (pickupable)
            {
                GameController.GetComponent<InstructionGUI>().setMessage(grab_message);
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
