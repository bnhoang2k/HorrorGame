using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{

    public InventoryManagement inventory;
    public RawImage[] eyeImages;
    public RawImage[] earImages;

    // Start is called before the first frame update
    void Start()
    {
        // Hide all the images at the start
        foreach (RawImage eye in eyeImages) {
            eye.enabled = false;
        }
        foreach (RawImage ear in earImages) {
            ear.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable() {
        Actions.UpdateInventory += UpdateHUD;
        Actions.UpdateItemEquipped += ShowEquipped;
    }

    private void OnDisable() {
        Actions.UpdateInventory -= UpdateHUD;
        Actions.UpdateItemEquipped -= ShowEquipped;
    }

    void UpdateHUD(Describable item) {
        // Update the HUD with the new item
        // TODO: Add more stuff here as we expand the inventory system
        // if (item.name == "Eye_Describable") {
        //     eyeImage.enabled = true;
        //     Color color = eyeImage.color;
        //     color.a = inventory.HasItem("Eye_Describable") ? 1 : 0.5f;
        //     eyeImage.color = color;
        // }
    }

    void ShowEquipped(InventoryItem item) {

        if (item.equipped) {
            item.slot.transform.GetChild(0).GetComponent<RawImage>().color = Color.white;
        } else {
            item.slot.transform.GetChild(0).GetComponent<RawImage>().color = Color.grey;
        }

        if (item.itemName == "Eye_Describable") {
            foreach (RawImage eye in eyeImages) {
                eye.enabled = !eye.enabled;
            }
        }
        else if (item.itemName == "Ear_Describable") {
            foreach (RawImage ear in earImages) {
                ear.enabled = !ear.enabled;
            }
        }
    }
}
