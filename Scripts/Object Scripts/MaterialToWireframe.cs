using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Describable))]
public class MaterialToWireframe : MonoBehaviour
{
    // To use this script, the object MUST MUST MUST have a MeshRenderer.
    private Renderer[] objectRenderers;
    public float tessellationValue = 1.0f; // 1.0f is the default. Higher values = higher complexity.
    public float fadeStart = 2.0f;
    public float fadeEnd = 3.0f; // Can't attach the arm length of the player because it'll slow down the program.
    void Start()
    {
        objectRenderers = GetComponentsInChildren<Renderer>(true);

        foreach (Renderer renderer in objectRenderers)
        {
            Material[] materials = renderer.materials;
            foreach (Material material in materials)
            {
                // WIP: We need to store certain variables in the material before we change the shader.
                float renderingMode = material.GetFloat("_Mode");
                // Set the shader to wireframe
                material.shader = Shader.Find("Amazing Assets/Wireframe Shader/Dynamic (SM5)/Tessellation");
                material.SetFloat("_Wireframe_Title_Rendering_Options", renderingMode);
                material.SetColor("_Wireframe_Color", Color.white);
                material.SetFloat("_Wireframe_Tessellation", 1.0f);
                
                material.EnableKeyword("WIREFRAME_TRY_QUAD_ON");
                material.EnableKeyword("WIREFRAME_NORMALIZE_EDGES_ON");

                material.SetFloat("_Wireframe_DistanceFade", 1.0f);
                material.EnableKeyword("WIREFRAME_DISTANCE_FADE_ON");
                material.SetFloat("_Wireframe_DistanceFadeStart", fadeStart);
                material.SetFloat("_Wireframe_DistanceFadeEnd", fadeEnd);
            }
        }
    }
}
