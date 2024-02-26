using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManagement : MonoBehaviour
{
    bool eyes = false;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            eyes = !eyes;
        }

    }

    void OnGUI() {

        string message;

        if (eyes) {
            message = "Eyes";
        } else {
            message = "No eyes";
        }

        GUI.Label(new Rect(10, 30, 500, 20), message);

    }
}
