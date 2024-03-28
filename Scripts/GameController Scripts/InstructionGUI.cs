using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InstructionGUI : MonoBehaviour
{
    private string message = "";
    public TMP_Text instruction_text;

    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update()
    {
        // Print the instruction message
        // message is set by Describable objects that are either pickupable or openable
        instruction_text.text = message;
    }

    public void setMessage(string new_message) {
        message = new_message;
    }
}
