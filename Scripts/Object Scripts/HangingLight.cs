using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangingLight : MonoBehaviour
{

    private Shader off_shader;
    private Shader on_shader;
    private Renderer rend;
    private Light _light;

    // Start is called before the first frame update
    void Start()
    {
        rend = gameObject.transform.Find("incandescent lamp").GetComponent<Renderer>();
        off_shader = rend.material.shader;
        on_shader = Shader.Find("Unlit/Color");   

        _light = gameObject.transform.Find("Light").GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_light.intensity > 0 && _light.enabled) {
            rend.materials[1].shader = on_shader;
        } else {
            rend.materials[1].shader = off_shader;
        }   
    }
}
