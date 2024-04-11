using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private GameObject GameController;
    private Camera player_camera;
    private GameObject player;
    private float arm_length;

    // Start is called before the first frame update
    void Start()
    {
        GameController = GameObject.Find("GameController");
        player_camera = Camera.main;
        player = GameObject.Find("Player");
        arm_length = player.GetComponent<Reach>().arm_length;
    }

    // Update is called once per frame
    void Update()
    {
        // if E or the left mouse button is pressed, the player is interacting with the object
        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) { Interact(); }
    }

    void Interact()
    {
        // Raycast generates a ray from the origin in the direction of the camera
        // and returns true if it hits something.
        // We want to check if the player is looking at an object that can be picked up.
        // If it's describable, we want to "grab" it, remove it from the environment, and add it to the inventory.
        RaycastHit hit;
        Ray ray = player_camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, arm_length)) {

            Describable describable = hit.collider.GetComponent<Describable>();
            // if the object can be picked up, grab it
            if (describable && describable.isPickupable)
            {
                Actions.UpdateInventory(describable);
                // Destroy(describable.gameObject);
                describable.gameObject.SetActive(false);

                GameController.GetComponent<InstructionGUI>().setMessage("");
                Debug.Log("Grabbed " + describable.gameObject.name);
            }
            // if the object can be opened, open it
            else if (describable && describable.isOpenable)
            {
                describable.gameObject.GetComponent<Openable>().Open();

                GameController.GetComponent<InstructionGUI>().setMessage("");

                Debug.Log("Opened or unlocked " + describable.gameObject.name);
            }
            // if the object can't use the Openable script, but can be opened
            // needs to be hard coded at the moment
            else if (describable && describable.isUniqueInteractable)
            {

                // open the floorboard compartment
                if (describable.gameObject.GetComponent<OpenFloorboard>()) {

                    describable.gameObject.GetComponent<OpenFloorboard>().Open();
                    Debug.Log("Opened " + describable.gameObject.name);
                }
                // toggle a light switch
                else if (describable.gameObject.GetComponent<LightSwitch>()) {

                    describable.gameObject.GetComponent<LightSwitch>().Switch();
                    Debug.Log("Switched " + describable.gameObject.name);

                }
                // interact with the Ouiji board
                else if (describable.gameObject.GetComponent<OuijaInteract>()) {
                    
                    describable.gameObject.GetComponent<OuijaInteract>().DoInput();
                    Debug.Log("Interacted with " + describable.gameObject.name);
                }
                // interact with Desk_Describable
                else if (describable.gameObject.GetComponent<OpenDrawer>()) {
                    bool open = describable.gameObject.GetComponent<OpenDrawer>().GetState();
                    if (open) {describable.gameObject.GetComponent<OpenDrawer>().Close();}
                    else {describable.gameObject.GetComponent<OpenDrawer>().Open();}
                    Debug.Log("Opened " + describable.gameObject.name);
                }

                GameController.GetComponent<InstructionGUI>().setMessage("");

            }
        }
    }

}