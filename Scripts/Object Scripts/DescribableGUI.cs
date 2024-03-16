using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescribableGUI : MonoBehaviour
{

    public Texture mouse_icon;
    private string message = "GUIDisplay base message";
    public GUIStyle centered_style;

    // Start is called before the first frame update
    void Start() {
        // hide the normal mouse cursor
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update() {}

    void OnGUI() {

        int mouse_size = 10;
        // Show where the mouse is in the center of the screen
        GUI.DrawTexture(new Rect(Input.mousePosition.x - mouse_size/2, Input.mousePosition.y - mouse_size/2, mouse_size, mouse_size), mouse_icon);

        // Print what the player is touching
        // message is set by Describable objects

        int message_x = Screen.width / 2;
        int message_y = Screen.height - 30;

        GUI.color = Color.white;

        GUI.Label(new Rect(0, Screen.height - 30, Screen.width, 20), message, centered_style);
        // Text will be cut off if it is longer to display than the rectangle
        // TODO: find a better way to display

    }

    public void setMessage(string new_message) {
        message = new_message;
    }
}
