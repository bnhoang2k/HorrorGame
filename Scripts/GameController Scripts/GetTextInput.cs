using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetTextInput : MonoBehaviour
{
    private string inputText;
    private TMP_InputField inputField;
    public GameObject ouija;

    // Start is called before the first frame update
    void Start()
    {
        // get the input field
        inputField = gameObject.GetComponent<TMP_InputField>();
        // disable the input field at first
        EnableInput(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void EnableInput(bool enable) {
        gameObject.SetActive(enable);
        inputField.ActivateInputField();
    }

    public void GetInput(string input) {
        inputText = input;

        ouija.GetComponent<OuijaInteract>().Interact(input);
    }
}
