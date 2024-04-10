using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuijaInteract : MonoBehaviour
{
    public GetTextInput getInput;
    public string code;
    private GameObject GameController;
    private Camera player_camera;
    private float arm_length;
    private string interact_message = "Press E to communicate";
    public GameObject bookcase;
    private string input;
    private bool open = false;

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
    void Update()
    {
        
    }

    public void DoInput() {

        // get text input from the player
        getInput.EnableInput(true);

    }

    public void Interact(string text) {

        // disable the text input field
        getInput.EnableInput(false);
        // enter the input into the Ouija board
        gameObject.GetComponent<Ouija>().CallSpell(text);

        // store what was input into the oujia
        input = text;

    }

    // called by Ouija after it is done moving
    public void Open() {
        if (!open) {
            // check if the input is the correct code
            if (input == code) {
                // unlock the secret compartment
                open = true;
                Destroy(bookcase);
                Debug.Log("Code correct, unlocking bookcase");
            } else {
                // spell out "NO GOODBYE" on the ouija board, starting and ending at the center
                gameObject.GetComponent<Ouija>().CallSpell("*NO@*");
            }
        }
    }

    private void OnMouseOver() {
    // called every frame while the mouse is over an object's collider

        // Raycast generates a ray from the origin in the direction of the camera
        // and returns true if it hits something.
        RaycastHit hit;
        Ray ray = player_camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, arm_length)) {

            OuijaInteract ouija = hit.collider.GetComponent<OuijaInteract>();
            // if the object is within reach, the player can feel it
            if (ouija)
            {
                GameController.GetComponent<InstructionGUI>().setMessage(interact_message);
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
