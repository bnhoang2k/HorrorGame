using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem {
    public string itemName;
    public Texture icon;
    public GameObject slot;
    public int x, y;
    public float x_scale, y_scale;
    public bool equipped = false;
    public InventoryItem (string name, GameObject new_slot, Texture image, int _x, int _y, float xs, float ys) {
        itemName = name;
        slot = new_slot;
        icon = image;
        x = _x;
        y = _y;
        x_scale = xs;
        y_scale = ys;
    }
}



public class InventoryManagement : MonoBehaviour
{
    
    public List<InventoryItem> inventory = new List<InventoryItem>();
    public List<GameObject> invSlots = new List<GameObject>();
    private int num_slots = 5;
    public SenseController senseController;
    // index of the next open slot in the list, ears = 0 and eyes = 1
    private int next_slot = 2;

    // TODO: may need to add a check to make sure the inventory doesn't overfill (or just add as many slots as items that can be picked up)
    // TODO: need to figure out how UpdateSlot will work when removing an item
    
    // Start is called before the first frame update
    void Start() {
        // initialize inventory slots
        for (int i = 0; i < num_slots; i++) {
            
            invSlots.Add(GameObject.Find("Slot" + (i+1).ToString()));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        // check for equipping slot1 (ears)
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            InventoryItem earItem = inventory.Find(item => item.itemName == "Ear_Describable");
            // check if player has ears in their inventory
            if (earItem != null) {
                // equip/unequip ears
                // set if the player is deaf or not
                senseController.SetDeaf(earItem.equipped);
                // change equipped status
                earItem.equipped = !earItem.equipped;
                Actions.UpdateItemEquipped?.Invoke(earItem);
            }
        }

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
                Actions.UpdateItemEquipped?.Invoke(eyeItem);
            }

        }

        // check for equipping slot3
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            GameObject slot = invSlots[2];
            InventoryItem item = inventory.Find(item => item.slot == slot);
            item.equipped = !item.equipped;
            Actions.UpdateItemEquipped?.Invoke(item);
        }

        // check for equipping slot4
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            GameObject slot = invSlots[3];
            InventoryItem item = inventory.Find(item => item.slot == slot);
            item.equipped = !item.equipped;
            Actions.UpdateItemEquipped?.Invoke(item);
        }

        // check for equipping slot5
        if (Input.GetKeyDown(KeyCode.Alpha5)) {
            GameObject slot = invSlots[4];
            InventoryItem item = inventory.Find(item => item.slot == slot);
            item.equipped = !item.equipped;
            Actions.UpdateItemEquipped?.Invoke(item);
        }

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
            Actions.UpdateInventory?.Invoke(GameObject.Find("Ear_Describable").GetComponent<Describable>());
        }

    }

    public bool holdingItem(GameObject obj) {
        // get the item from inventory list
        InventoryItem item = inventory.Find(item => item.itemName == obj.name);

        // check if it exists and is equipped
        if (item != null && item.equipped == true) {
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
        // get the object's data
        Pickupable iconInfo = GameObject.Find(item.name).GetComponent<Pickupable>();
        
        Texture icon = iconInfo.icon;
        int x = iconInfo.x;
        int y = iconInfo.y;
        float x_scale = iconInfo.x_scale;
        float y_scale = iconInfo.y_scale;

        // select the slot to put the item into
        int slot;
        if (item.name == "Eye_Describable") {slot = 1;}
        else if (item.name == "Ear_Describable") {slot = 0;}
        else {slot = next_slot; next_slot++;}

        InventoryItem new_item = new InventoryItem(item.name, invSlots[slot], icon, x, y, x_scale, y_scale);
        inventory.Add(new_item);
        // Debug.Log("Added " + item.name + " to inventory");
        UpdateSlot(new_item);
    }

    public void UpdateSlot(InventoryItem item) {

        // create new empty child of the slot object to attach the new item's icon to
        GameObject slotImage = new GameObject("ItemIcon");
        slotImage.transform.parent = item.slot.transform;

        // add the image
        slotImage.AddComponent<RawImage>();
        slotImage.GetComponent<RawImage>().texture = item.icon;

        // move and size the image appropriately
        slotImage.GetComponent<RectTransform>().localPosition = new Vector3(item.x, item.y, 0);
        slotImage.GetComponent<RectTransform>().localScale = new Vector3(item.x_scale, item.y_scale, 1);
    }

}
