using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManagement : MonoBehaviour
{
    bool ears = false;
    bool eyes = false;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        // check for equipping ears
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            ears = !ears;
        }

        // check for equipping eyes
        if(Input.GetKeyDown(KeyCode.Alpha2)) {
            eyes = !eyes;
        }

    }

    void OnGUI() {

        // For testing //
        string message;

        if (eyes) {
            message = "Eyes";
        } else {
            message = "No eyes";
        }

        GUI.Label(new Rect(10, 30, 500, 20), message);
        ////
        
        // draw inventory slots
        int slot_x = 40;
        int slot_y = 15;
        int slot_spacing = 20;
        int slot_size = 50;

        // TODO: make a GUI content to get text and images
        // TODO: figure out how to highlight buttons when they are equipped
        // slot1: ears
        if (ears) { // ears are equipped
            GUI.Box(new Rect(slot_x, slot_y, slot_size, slot_size), "1 Ears EQUIPPED");
        } else { // ears are not equipped
            GUI.Box(new Rect(slot_x, slot_y, slot_size, slot_size), "1 Ears");
        }
        

        // slot2: eyes
        GUI.Box(new Rect(slot_x + slot_size + slot_spacing, slot_y, slot_size, slot_size), "2 Eyes");


    }

    public bool hasKey() {
        // TODO: implement
        return true;
    }
}
