using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightController : MonoBehaviour
{
    public class LightControl {
        public Light light;
        public float original_intensity;

        public LightControl(Light l, float i) { light = l; original_intensity = i; }
    }

    private Light[] all_base_lights;
    private List<LightControl> all_lights;

    // Start is called before the first frame update
    void Start()
    {
        // find all the lights in the scene
        all_base_lights = FindObjectsOfType<Light>();

        all_lights = new List<LightControl>();

        foreach(Light light in all_base_lights) {
            LightControl lc = new LightControl(light, light.intensity);
            all_lights.Add(lc);
        }

        // DEBUGGING //
        string base_lights = "";
        string all_l = "";
        foreach (Light light in all_base_lights) {
            base_lights += ", " + light.name;
        }
        foreach (LightControl lc in all_lights) {
            all_l += ", " + lc.light.name + ": " + lc.original_intensity;
        }
        Debug.Log("Base lights: " + base_lights);
        Debug.Log("LC lights: " + all_l);

        ////

        // turn all the lights off at first
        ToggleAllLights(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleAllLights(bool on) {

        foreach (LightControl lc in all_lights) {
            // if the lights are off, set the intensity to 0
            if (!on) {
                lc.light.intensity = 0;
            } else {
                // set the intensity to the original instensity
                lc.light.intensity = lc.original_intensity;
            }
        }
    }

    public void ToggleLights(GameObject[] lights, bool on) {

        foreach (GameObject light in lights) {
            // turn on/off each object's light component
            if (light.GetComponent<Light>()) {
                light.GetComponent<Light>().enabled = on;
            }
            // turn on/off for each object all of their children object lights
            else if (light.GetComponentsInChildren<Light>().Length > 0) {
                foreach (Light child_light in light.GetComponentsInChildren<Light>()) {
                    child_light.enabled = on;
                }
            }
        }

    }
}
