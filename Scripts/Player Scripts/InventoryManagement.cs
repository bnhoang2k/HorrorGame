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
    
    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update()
    {
        
        // check for equipping slot1 (ears)
        if (Input.GetKeyDown(KeyCode.Alpha1)) {ears = !ears;}

        // check for equipping slot2 (eyes)
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            InventoryItem eyeItem = inventory.Find(item => item.itemName == "Eye_Describable");
            
            // check if player has eyes in their inventory
            if (eyeItem != null) {
                // equip/uneqiup eyes
                eyes = !eyes;
                // set if the player is blind or not
                senseController.SetVision(eyes);
                // change equipped status
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
            Debug.Log("Cheat activated");
            Actions.UpdateInventory?.Invoke(GameObject.Find("Eye_Describable").GetComponent<Describable>());
        }

    }

    public bool hasKey(string object_name) {
        // TODO: implement
        // add pairs of objects and their corresponding keys to see if the player has the right key
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
