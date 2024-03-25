using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class SenseController : MonoBehaviour
{
    
    private GameObject gameController;
    private GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
        gameController = GameObject.Find("GameController");
    }

    public void SetBlind(bool isBlind)
    {
        // TODO: Currently hardcoded for just the directional light.
        // However, we want to disable all lights in the scene in the future.
        // After turning on/off the lights, we want to update all describables in the scene.
        // We want to hide the wireframe property of the describable objects if the player can see.
        Light directionalLight = GameObject.Find("Directional Light").GetComponent<Light>();
        Describable[] allDescribables = gameController.GetComponent<GameController>().GetDescribables();
        if (isBlind)
        {
            directionalLight.enabled = false;
            player.GetComponentInChildren<Camera>().clearFlags = CameraClearFlags.Color;
            player.GetComponentInChildren<Camera>().backgroundColor = Color.black;
            foreach (Describable describable in allDescribables)
            {
                if (describable)
                {
                    Material describableMaterial = describable.GetComponent<Renderer>().material;
                    if (describableMaterial)
                    {
                        Color wireframeColor = describableMaterial.GetColor("_Wireframe_Color");
                        Color newColor = new Color(wireframeColor.r, wireframeColor.g, wireframeColor.b, 1.0f);
                        describableMaterial.SetColor("_Wireframe_Color", newColor);
                    }
                }
            }
        }
        else
        {
            directionalLight.enabled = true;
            player.GetComponentInChildren<Camera>().clearFlags = CameraClearFlags.Skybox;
            foreach (Describable describable in allDescribables)
            {
                if (describable)
                {
                    Material describableMaterial = describable.GetComponent<Renderer>().material;
                    if (describableMaterial)
                    {
                        Color wireframeColor = describableMaterial.GetColor("_Wireframe_Color");
                        Color newColor = new Color(wireframeColor.r, wireframeColor.g, wireframeColor.b, 0.0f);
                        describableMaterial.SetColor("_Wireframe_Color", newColor);
                    }
                }
            }
        }
    }
}
