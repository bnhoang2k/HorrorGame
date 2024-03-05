using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMouseInput : MonoBehaviour {

    private string default_message = "You can't feel anything here";
    public string description = "EMPTY";
    public GameObject GUIDisplay;
    // You must drag in the INSTANCE of the game object that has GUIdisplay code
    // into the inspector window (NOT the prefab)
    

    private void Start() {}
    private void Update() {}

    private void OnMouseOver() {
        // called every frame while the mouse is over an object's collider mesh
        
        // call a function from the script GUIDisplay to set the message
        GUIDisplay.GetComponent<GUIDisplay>().setMessage(description);

    }

    private void OnMouseExit() {
        // called when the mouse exits an object's collider mesh

        // call a function from the script GUIDisplay to set the message
        GUIDisplay.GetComponent<GUIDisplay>().setMessage(default_message);

    }
}
