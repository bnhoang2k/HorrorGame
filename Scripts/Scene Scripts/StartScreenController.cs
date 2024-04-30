using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreenController : MonoBehaviour
{
    private LightController lc;
    private GameObject enemy;
    private Renderer meshRenderer;
    public Light roomLight;
    void Start()
    {
        // Get LightController component and turn on lights
        lc = GetComponent<LightController>();
        if (lc) {StartCoroutine(DelayEnableLights(0.1f));}
        // Get enemy object and start off
        enemy = GameObject.Find("Enemy");
        if (enemy)
        {
            meshRenderer = enemy.GetComponentInChildren<Renderer>();
            meshRenderer.enabled = false;
        }

        Invoke("ToggleLight", 0.1f);
    }
    IEnumerator DelayEnableLights(float delay)
    {
        yield return new WaitForSeconds(delay);
        lc.ToggleAllLights(true);
    }
    private void ToggleLight()
    {
        // Toggle light state
        roomLight.enabled = !roomLight.enabled;

        // Handle enemy visibility based on light state
        ManageEnemySpawn();

        // Schedule the next toggle
        float nextToggleTime = roomLight.enabled ? Random.Range(0.05f, 2.0f) : Random.Range(0.05f, 0.2f);
        Invoke("ToggleLight", nextToggleTime);
    }

    private void ManageEnemySpawn()
    {
        // Check if the light is on and manage enemy accordingly
        if (roomLight.enabled && meshRenderer != null)
        {
            if (Random.value < 0.1f) // 50% chance to spawn the enemy
            {
                meshRenderer.enabled = true;
            }
        }
        else if (meshRenderer != null)
        {
            // Light is off, despawn enemy if it was spawned
            meshRenderer.enabled = false;
        }
    }
}
