using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Openable : MonoBehaviour
{
    public GameObject GameController;
    private Animator m_Animator;
    private bool isOpen = false;
    private bool locked = true;

    // Start is called before the first frame update
    void Start()
    {
        // set the Animator reference
        m_Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void open() {
        
        // if the object is locked and the player is holding the right key, open the door
        if (locked && GameController.GetComponent<InventoryManagement>().hasKey(gameObject.name)) {
            locked = !locked;
            isOpen = !isOpen;
            m_Animator.SetBool("open", isOpen);
        }
        // the object is unlocked, so the player does not need a key anymore
        else if (!locked) {
            isOpen = !isOpen;
            m_Animator.SetBool("open", isOpen);
        }
        
    }
}
