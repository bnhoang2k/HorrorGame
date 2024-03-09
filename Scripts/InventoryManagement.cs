using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManagement : MonoBehaviour
{
    bool ears = false;
    bool eyes = false;
    bool slot3 = false;
    bool slot4 = false;
    bool slot5 = false;
    public GUIStyle equipped_style;
    public GUIStyle unequipped_style;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        // check for equipping slot1 (ears)
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            ears = !ears;
        }

        // check for equipping slot2 (eyes)
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            eyes = !eyes;
        }

        // check for equipping slot3
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            slot3 = !slot3;
        }

        // check for equipping slot4
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            slot4 = !slot4;
        }

        // check for equipping slot5
        if (Input.GetKeyDown(KeyCode.Alpha5)) {
            slot5 = !slot5;
        }

    }

    void OnGUI() {
        
        // draw inventory slots
        int slot_x = 10;
        int slot_y = 10;
        int slot_size = 50;
        int slot_spacing = slot_size + 10;

        

        // TODO: make a GUI content to get text and images in the box
        // TODO: make sure you can still see the boxes even in the dark
        GUIStyle style = unequipped_style;

        // slot1: ears
        if (ears) { // ears are equipped
            style = equipped_style;
        } else { // ears are not equipped
            style = unequipped_style;
        }
        // draw slot
        GUI.Box(new Rect(slot_x, slot_y, slot_size, slot_size), "1 Ears", style);
        

        // slot2: eyes
        slot_x = slot_x + slot_spacing;

        if (eyes) { // eyes are equipped
            style = equipped_style;
        } else { // eyes are not equipped
            style = unequipped_style;
        }
        // draw slot
        GUI.Box(new Rect(slot_x, slot_y, slot_size, slot_size), "2 Eyes", style);


        // slot3: EMPTY
        slot_x = slot_x + slot_spacing;

        if (slot3) { // slot3 is equipped
            style = equipped_style;
        } else { // slot3 is not equipped
            style = unequipped_style;
        }
        // draw slot
        GUI.Box(new Rect(slot_x, slot_y, slot_size, slot_size), "3", style);


        // slot4: EMPTY
        slot_x = slot_x + slot_spacing;

        if (slot4) { // slot4 is equipped
            style = equipped_style;
        } else { // slot4 is not equipped
            style = unequipped_style;
        }
        // draw slot
        GUI.Box(new Rect(slot_x, slot_y, slot_size, slot_size), "4", style);


        // slot5: EMPTY
        slot_x = slot_x + slot_spacing;

        if (slot5) { // slot5 is equipped
            style = equipped_style;
        } else { // slot5 is not equipped
            style = unequipped_style;
        }
        // draw slot
        GUI.Box(new Rect(slot_x, slot_y, slot_size, slot_size), "5", style);

    }

    public bool hasKey() {
        // TODO: implement
        return true;
    }

}
