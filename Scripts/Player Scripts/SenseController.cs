using System.Collections;
using System.Collections.Generic;
using StarterAssets;
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            foreach (Describable describable in gameController.GetComponent<GameController>().GetDescribables())
            {
                Debug.Log(describable.gameObject.name);
            }
        }
    }

    public void SetBlind(bool isBlind)
    {
        // TODO: Currently hardcoded for just the directional light.
        // However, we want to disable all lights in the scene in the future.
        // After turning on/off the lights, we want to update the shaders for all describables within the scene.
        // We want to hide the wireframe property of the describable objects if the player can see.
        Describable[] allDescribables = gameController.GetComponent<GameController>().GetDescribables();
        if (isBlind)
        {
            gameController.GetComponent<ControlLights>().ToggleAllLights(!isBlind);
            player.GetComponentInChildren<Camera>().clearFlags = CameraClearFlags.Color;
            player.GetComponentInChildren<Camera>().backgroundColor = Color.black;
        }
        else
        {
            gameController.GetComponent<ControlLights>().ToggleAllLights(!isBlind);
            player.GetComponentInChildren<Camera>().clearFlags = CameraClearFlags.Skybox;
        }
        foreach (Describable describable in allDescribables)
        {
            SetWireFrame(describable.gameObject, isBlind);
        }
    }

    // All describable objects must have a mesh renderer in order for the mesh wireframe to be visible.
    // This function sets the wireframe property of the describable objects to be visible or invisible by setting the
    // alpha color of the wireframe color to 1 or 0, respectively.
    private void SetWireFrame(GameObject obj, bool isBlind)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>(true);
        foreach (Renderer describableRenderer in renderers)
        {
            foreach (Material describableMaterial in describableRenderer.materials)
            {
                if (describableMaterial.HasProperty("_Wireframe_Color"))
                {
                    Color wireframeColor = describableMaterial.GetColor("_Wireframe_Color");
                    Color newColor = new Color(wireframeColor.r, wireframeColor.g, wireframeColor.b, isBlind ? 1.0f : 0.0f);
                    describableMaterial.SetColor("_Wireframe_Color", newColor);
                }
            }
        }
    }

    public void SetDeaf(bool isDeaf)
    {
        FirstPersonController playerController = player.transform.Find("PlayerCapsule").GetComponent<FirstPersonController>();
        playerController.useFootsteps = !isDeaf;
    }

}
