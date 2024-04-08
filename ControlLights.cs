using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlLights : MonoBehaviour
{

    private Light[] all_lights;
    // Start is called before the first frame update
    void Start()
    {
        // find all the lights in the scene
        all_lights = FindObjectsOfType<Light>();

        // turn all the lights off at first
        ToggleAllLights(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleAllLights(bool on) {

        foreach (Light light in all_lights) {
            light.enabled = on;
        }
    }

    public void ToggleLights(GameObject[] lights, bool on) {

        foreach (GameObject light in lights) {
            // turn on/off each object's light component
            if (light.GetComponent<Light>()) {
                light.GetComponent<Light>().enabled = on;

                Debug.Log("Toggled " + light.name + " light.");
            }
            // turn on/off for each object all of their children object lights
            else if (light.GetComponentsInChildren<Light>().Length > 0) {
                foreach (Light child_light in light.GetComponentsInChildren<Light>()) {
                    child_light.enabled = on;

                    Debug.Log("Toggled child light.");
                }
            }
        }

    }
}
