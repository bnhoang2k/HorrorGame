using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Light directionalLight;
    private GameObject player;
    private Camera playerCamera;

    private Describable[] allDescribables;

    private LightmapData[] originalLightmaps;

    // Start is called before the first frame update
    void Start()
    {
        // On start of the simulation, we want to disable the directional light.
        // Directional light is enabled at the beginning so we can see the scene and manipulate
        // it better on our end.
        if (directionalLight)
        {
            directionalLight.enabled = false;
        }
        // Find the player
        player = GameObject.Find("Player");
        // If the player is found, get the camera component and set the background color to black.
        // This is to simulate blindness.
        if (player)
        {
            playerCamera = player.GetComponentInChildren<Camera>();
            playerCamera.clearFlags = CameraClearFlags.Color;
            playerCamera.backgroundColor = Color.black;
        }
        // GameController will keep track of all describables in the scene.
        allDescribables = FindObjectsByType<Describable>(FindObjectsSortMode.None);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Describable[] GetDescribables()
    {
        return allDescribables;
    }
}
