using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIDisplay : MonoBehaviour
{

    private string message = "GUIDisplay base message";

    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {}

    void OnGUI() {

        GUI.Label(new Rect(10, 10, 500, 20), message);
       // Text will be cut off if it is longer to display than the rectangle
       // TODO: find a better way to display

    }

    public void setMessage(string new_message) {
        message = new_message;
    }
}
