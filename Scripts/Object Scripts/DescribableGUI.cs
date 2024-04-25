using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DescribableGUI : MonoBehaviour
{

    public Texture mouse_icon;
    private int mouse_size = 30;
    private string message = "GUIDisplay base message";
    public TMP_Text describable_text;

    // Start is called before the first frame update
    void Start() {
        // hide the normal mouse cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update() {
        // Print what the player is touching
        // message is set by Describable objects
        describable_text.text = message;
    }

    void OnGUI() {
        // Show where the mouse is in the center of the screen
        GUI.DrawTexture(new Rect(Input.mousePosition.x - mouse_size/2, Input.mousePosition.y - mouse_size/2, mouse_size, mouse_size), mouse_icon);
    }

    public void setMessage(string new_message) {
        message = new_message;
    }
}
