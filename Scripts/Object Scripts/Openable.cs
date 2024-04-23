using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Openable : MonoBehaviour
{
    // All openable objects need an Animator on the root object
    // The root_name is the name of which object has the Animator
    // The Animator must have a boolean "open" that controls the animation

    // Openable objects that cannot use this script need to be marked as isUniqueOpenable in Describable
    // and their opening script be hard coded into PlayerInteract

    public string root_name;
    public GameObject key;
    private GameObject root;
    private GameObject GameController;
    private Animator animator;
    public bool open = false;
    public bool locked = true;
    private Camera player_camera;
    private float arm_length;
    private string unlock_message = "Press E to unlock";
    private string locked_message = "Locked";
    private string open_message = "Press E to open";
    private string close_message = "Press E to close";
    private AudioSource audioSource;
    public AudioClip openSound;
    public AudioClip unlockSound;

    // Start is called before the first frame update
    void Start()
    {
        // find the object that has the animator
        root = GameObject.Find(root_name);
        animator = root.GetComponent<Animator>();
        // get the audio source of the openable object
        audioSource = gameObject.GetComponent<AudioSource>();
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
        if (!locked) {
            open = !open;
            Debug.Log("open: " + open);
            animator.SetBool("open", open);
            // play opening sound
            if (audioSource && openSound) {
                audioSource.PlayOneShot(openSound);
            } else { Debug.Log("No opening sound: " + gameObject.name); }
            // if the object is a door, rebake the NavMesh
            if (gameObject.name == "Door_Main1")
            {
                NavMeshBaker navMeshBaker = GameObject.Find("GameController").GetComponent<NavMeshBaker>();
                if (open) {navMeshBaker.LayerChanger(root, "Default");}
                else {navMeshBaker.LayerChanger(root, "NavMeshLayer");}
                navMeshBaker.ReBakeNavMesh();
            }
        }
        else if (locked) {
            unlock();
        }
        
    }

    public void unlock() {

        // make sure the player has the key equipped
        if (GameController.GetComponent<InventoryManagement>().holdingItem(key)) {
            // unlock the object
            locked = false;

            // play unlocking sound
            if (audioSource && unlockSound) {
                audioSource.PlayOneShot(unlockSound);
            } else { Debug.Log("No unlocking sound: " + gameObject.name); }

            // destroy the key and remove from inventory
            GameController.GetComponent<InventoryManagement>().RemoveItem(key);
            Destroy(key);
        }  
    }

    private void OnMouseOver() {
    // called every frame while the mouse is over an object's collider

        // Raycast generates a ray from the origin in the direction of the camera
        // and returns true if it hits something.
        RaycastHit hit;
        Ray ray = player_camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, arm_length)) {

            Openable openable = hit.collider.GetComponent<Openable>();
            // if the object is within reach, the player can feel it
            if (openable)
            {
                if (locked && GameController.GetComponent<InventoryManagement>().holdingItem(key)) {
                    GameController.GetComponent<InstructionGUI>().setMessage(unlock_message);
                } else if (locked) {
                    GameController.GetComponent<InstructionGUI>().setMessage(locked_message);
                } else if (open) {
                    GameController.GetComponent<InstructionGUI>().setMessage(close_message);
                } else {
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
