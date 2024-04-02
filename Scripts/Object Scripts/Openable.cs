using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Openable : MonoBehaviour
{
    // All openable objects need an Animator on the root object
    // The root_name is the name of which object has the Animator
    // The Animator must have a boolean "open" that controls the animation


    public string root_name;
    public GameObject key;
    private GameObject root;
    private GameObject GameController;
    private Animator animator;
    public bool open = false;
    public bool locked = true;

    // Start is called before the first frame update
    void Start()
    {
        // find the object that has the animator
        root = GameObject.Find(root_name);
        animator = root.GetComponent<Animator>();
        // find the game controller
        GameController = GameObject.Find("GameController");
    }

    // Update is called once per frame
    void Update() {}

    public void Open() {
        
        // the object is unlocked, so it can be opened
        if (!locked) {
            open = !open;
            Debug.Log("open: " + open);
            animator.SetBool("open", open);
            Debug.Log(animator);
        }
        else if (locked) {
            unlock();
        }
        
    }

    public void unlock() {

        // make sure the player has the key equipped
        if (GameController.GetComponent<InventoryManagement>().holdingItem(key)) {
            // unlock the object
            locked = false;

            // maybe destroy the key after and remove from inventory?
        }  
    }
}
