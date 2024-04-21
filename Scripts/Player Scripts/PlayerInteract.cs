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
    private AudioSource playerAudioSource;
    public AudioClip pickUpSound;
    public AudioClip floorboardSound;

    // Start is called before the first frame update
    void Start()
    {
        GameController = GameObject.Find("GameController");
        player_camera = Camera.main;
        player = GameObject.Find("Player");
        playerAudioSource = GameObject.Find("Player/PlayerCapsule").GetComponent<AudioSource>();
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

                // check if the object was actually picked up
                if (GameController.GetComponent<InventoryManagement>().HasItem(describable.name)) {
                    // play pick up sound
                    if (playerAudioSource && pickUpSound) {
                        playerAudioSource.PlayOneShot(pickUpSound);
                    } else { Debug.Log("No pickup sound in PlayerInteract"); }
                }

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

                    // play pick up sound
                    if (playerAudioSource && floorboardSound) {
                        playerAudioSource.PlayOneShot(floorboardSound);
                    } else { Debug.Log("No floorboard sound in PlayerInteract"); }

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
                // place a missing item on the bookcase
                else if (describable.gameObject.GetComponent<MissingBookcaseItem>()) {

                    describable.gameObject.GetComponent<MissingBookcaseItem>().Place();
                    Debug.Log("Placed with " + describable.gameObject.name);
                }

                GameController.GetComponent<InstructionGUI>().setMessage("");

            }
        }
    }

}