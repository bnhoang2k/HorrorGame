using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIDisplay : MonoBehaviour
{

    private string message = "GUIDisplay base message";
    public Texture mouse_icon;

    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {}

    void OnGUI() {

        int size = 10;
        // Show where the mouse is in the center of the screen
        GUI.DrawTexture(new Rect(Input.mousePosition.x - size/2, Input.mousePosition.y - size/2, size, size), mouse_icon);

        // Print what the player is touching
        // message is set by Describable objects
        GUI.Label(new Rect(10, 10, 500, 20), message);
        // Text will be cut off if it is longer to display than the rectangle
        // TODO: find a better way to display

    }

    public void setMessage(string new_message) {
        message = new_message;
    }
}
