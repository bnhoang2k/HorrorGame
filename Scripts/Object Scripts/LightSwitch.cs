using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{

    public GameObject[] lights;
    private bool on = false;
    private GameObject GameController;
    private Camera player_camera;
    private float arm_length;
    private string toggle_message = "Press E to toggle";
    private AudioSource audioSource;
    public AudioClip sound;

    // Start is called before the first frame update
    void Start()
    {
        // find the game controller
        GameController = GameObject.Find("GameController");
        // find the camera and arm length
        player_camera = Camera.main;
        arm_length = GameObject.Find("Player").GetComponent<Reach>().arm_length;

        // get audio source
        audioSource = gameObject.GetComponent<AudioSource>();

        // turn all controlled lights off at first
        GameController.GetComponent<LightController>().ToggleLights(lights, false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Switch() {

        // the player can only turn on the lights if they have found the eye
        if (GameController.GetComponent<InventoryManagement>().HasItem("Eye_Describable")) {
            on = !on;
            // play toggle sound
            if (audioSource && sound) {
                audioSource.PlayOneShot(sound);
            } else { Debug.Log("No toggle sound: " + gameObject.name); }

            GameController.GetComponent<LightController>().ToggleLights(lights, on);
        }

    }

     private void OnMouseOver() {
    // called every frame while the mouse is over an object's collider

        // Raycast generates a ray from the origin in the direction of the camera
        // and returns true if it hits something.
        RaycastHit hit;
        Ray ray = player_camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, arm_length)) {

            LightSwitch lightSwitch = hit.collider.GetComponent<LightSwitch>();
            // if the object is within reach, the player can feel it
            if (lightSwitch)
            {
                GameController.GetComponent<InstructionGUI>().setMessage(toggle_message);
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
