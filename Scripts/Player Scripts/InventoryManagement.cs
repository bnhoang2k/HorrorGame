using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem {
    public string itemName;
    public bool equipped;
    public InventoryItem (string name) {itemName = name; equipped = false;}
}



public class InventoryManagement : MonoBehaviour
{
    
    public List<InventoryItem> inventory = new List<InventoryItem>();

    public SenseController senseController;
    
    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update()
    {
        
        // check for equipping slot1 (ears)
        if (Input.GetKeyDown(KeyCode.Alpha1)) {}

        // check for equipping slot2 (eyes)
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            InventoryItem eyeItem = inventory.Find(item => item.itemName == "Eye_Describable");
            
            // check if player has eyes in their inventory
            if (eyeItem != null) {
                // equip/unequip eyes
                // set if the player is blind or not
                senseController.SetBlind(eyeItem.equipped);
                // change equipped status
                eyeItem.equipped = !eyeItem.equipped;
                Actions.UpdateItemEquipped?.Invoke("Eye_Describable", eyeItem.equipped);
            }

        }

        // check for equipping slot3
        if (Input.GetKeyDown(KeyCode.Alpha3)) {}

        // check for equipping slot4
        if (Input.GetKeyDown(KeyCode.Alpha4)) {}

        // check for equipping slot5
        if (Input.GetKeyDown(KeyCode.Alpha5)) {}

        // Debug function to check inventory
        if (Input.GetKeyDown(KeyCode.I)) {
            foreach (InventoryItem item in inventory) {
                Debug.Log(item.itemName);
            }
        }

        // Cheat add items
        if (Input.GetKeyDown(KeyCode.C)) {
            Debug.Log("Cheat activated");
            Actions.UpdateInventory?.Invoke(GameObject.Find("Eye_Describable").GetComponent<Describable>());
            Actions.UpdateInventory?.Invoke(GameObject.Find("Key_Describable").GetComponent<Describable>());
        }

    }

    public bool holdingItem(GameObject obj) {
        // get the item from inventory list
        InventoryItem invItem = inventory.Find(item => item.itemName == obj.name);

        // DEBUGGING //
        // actually implement later, how to equip items generally
        Debug.Log("Manually setting equipped for true in holdingItem function for debugging purposes, need to actually implement equipping");
        invItem.equipped = true;

        // check if it exists and is equipped
        if (invItem != null && invItem.equipped == true) {
            return true;
        } else {
            return false;
        }
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
