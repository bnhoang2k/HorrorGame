using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenable : MonoBehaviour
{
    public string root_door_name;
    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {}

    public void open() {

        // get the root door object that actually contains the animator
        GameObject root_door = GameObject.Find(root_door_name);
        // open the door
        root_door.GetComponent<DoorAnimation>().openDoor();

    }
}
