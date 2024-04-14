using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MissingBookcaseItem : MonoBehaviour
{
    public GameObject missingItem;
    public GameObject bookcasePuzzle;
    private GameObject GameController;
    private Camera player_camera;
    private float arm_length;
    private string place_message = "Press E to place";

    // Start is called before the first frame update
    void Start()
    {
        // find the game controller
        GameController = GameObject.Find("GameController");
        // find the camera and arm length
        player_camera = Camera.main;
        arm_length = GameObject.Find("Player").GetComponent<Reach>().arm_length;

        // set self as invisible
        gameObject.GetComponent<Renderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Place() {

        if (GameController.GetComponent<InventoryManagement>().holdingItem(missingItem)) {
            // make self visible
            gameObject.GetComponent<Renderer>().enabled = true;

            // tell the bookcase the object is no longer missing
            bookcasePuzzle.GetComponent<OpenBookcase>().RemoveMissingItem(gameObject);
        }

    }

    private void OnMouseOver() {
    // called every frame while the mouse is over an object's collider

        // Raycast generates a ray from the origin in the direction of the camera
        // and returns true if it hits something.
        RaycastHit hit;
        Ray ray = player_camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, arm_length)) {

            MissingBookcaseItem item = hit.collider.GetComponent<MissingBookcaseItem>();
            // if the object is within reach, the player can feel it
            if (item)
            {
                if (GameController.GetComponent<InventoryManagement>().holdingItem(missingItem)) {
                    GameController.GetComponent<InstructionGUI>().setMessage(place_message);
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
