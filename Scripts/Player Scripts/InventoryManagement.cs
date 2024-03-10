using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem {
    public string itemName;
    public InventoryItem (string name) {itemName = name;}
}



public class InventoryManagement : MonoBehaviour
{
    
    public List<InventoryItem> inventory = new List<InventoryItem>();

    public SenseController senseController;

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
        if (Input.GetKeyDown(KeyCode.Alpha1)) {ears = !ears;}

        // check for equipping slot2 (eyes)
        // I changed this to test my new function.
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            InventoryItem eyeItem = inventory.Find(item => item.itemName == "Eye_Describable");
            if (eyeItem != null) {
                eyes = !eyes;
                senseController.SetVision(eyes);
                Actions.UpdateItemEquipped?.Invoke("Eye_Describable", eyes);
            }
        }

        // check for equipping slot3
        if (Input.GetKeyDown(KeyCode.Alpha3)) {slot3 = !slot3;}

        // check for equipping slot4
        if (Input.GetKeyDown(KeyCode.Alpha4)) {slot4 = !slot4;}

        // check for equipping slot5
        if (Input.GetKeyDown(KeyCode.Alpha5)) {slot5 = !slot5;}

        // Debug function to check inventory
        if (Input.GetKeyDown(KeyCode.I)) {
            foreach (InventoryItem item in inventory) {
                Debug.Log(item.itemName);
            }
        }

        // Cheat add everything
        if (Input.GetKeyDown(KeyCode.C)) {
            Actions.UpdateInventory?.Invoke(GameObject.Find("Eye_Describable").GetComponent<Describable>());
        }

    }

    // void OnGUI() {
        
    //     // draw inventory slots
    //     int slot_x = 10;
    //     int slot_y = 10;
    //     int slot_size = 50;
    //     int slot_spacing = slot_size + 10;
        

    //     // TODO: make a GUI content to get text and images in the box
    //     // TODO: make sure you can still see the boxes even in the dark
    //     GUIStyle style = unequipped_style;

    //     // slot1: ears
    //     if (ears) { // ears are equipped
    //         style = equipped_style;
    //     } else { // ears are not equipped
    //         style = unequipped_style;
    //     }
    //     // draw slot
    //     GUI.Box(new Rect(slot_x, slot_y, slot_size, slot_size), "1 Ears", style);
        

    //     // slot2: eyes
    //     slot_x = slot_x + slot_spacing;

    //     if (eyes) { // eyes are equipped
    //         style = equipped_style;
    //     } else { // eyes are not equipped
    //         style = unequipped_style;
    //     }
    //     // draw slot
    //     GUI.Box(new Rect(slot_x, slot_y, slot_size, slot_size), "2 Eyes", style);


    //     // slot3: EMPTY
    //     slot_x = slot_x + slot_spacing;

    //     if (slot3) { // slot3 is equipped
    //         style = equipped_style;
    //     } else { // slot3 is not equipped
    //         style = unequipped_style;
    //     }
    //     // draw slot
    //     GUI.Box(new Rect(slot_x, slot_y, slot_size, slot_size), "3", style);


    //     // slot4: EMPTY
    //     slot_x = slot_x + slot_spacing;

    //     if (slot4) { // slot4 is equipped
    //         style = equipped_style;
    //     } else { // slot4 is not equipped
    //         style = unequipped_style;
    //     }
    //     // draw slot
    //     GUI.Box(new Rect(slot_x, slot_y, slot_size, slot_size), "4", style);


    //     // slot5: EMPTY
    //     slot_x = slot_x + slot_spacing;

    //     if (slot5) { // slot5 is equipped
    //         style = equipped_style;
    //     } else { // slot5 is not equipped
    //         style = unequipped_style;
    //     }
    //     // draw slot
    //     GUI.Box(new Rect(slot_x, slot_y, slot_size, slot_size), "5", style);

    // }
    public bool hasKey() {
        // TODO: implement
        return true;
    }

    private void OnEnable() {
        Actions.UpdateInventory += AddItem;
    }
    private void OnDisable() {
        Actions.UpdateInventory -= AddItem;
    }

    public bool HasItem(string itemName) {
        return inventory.Exists(item => item.itemName == itemName);
    }

    public void AddItem(Describable item) {
        inventory.Add(new InventoryItem(item.name));
        // Debug.Log("Added " + item.name + " to inventory");
    }

}
