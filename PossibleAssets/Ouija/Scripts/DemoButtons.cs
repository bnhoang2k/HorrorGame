using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoButtons : MonoBehaviour {
    public InputField inputF;
    public Slider speedSlider;
    public Slider transitionSlider;
    public Ouija ouija;
     List<Camera> cameras = new List<Camera>();
    int activeCamera;
    private void Start()
    {
        foreach (Camera item in FindObjectsOfType<Camera>())
        {
            cameras.Add(item);
            if (item.enabled)
                activeCamera = cameras.Count-1;
        }
    }
    public void CallOuija()
    {
        ouija.CallSpell(inputF.text);
    }

    public void ChangeCamera()
    {

        if (activeCamera<cameras.Count-1)
        {
            cameras[activeCamera].enabled = false;
            cameras[activeCamera + 1].enabled = true; 
            activeCamera++;
        }
        else
        {
            cameras[activeCamera].enabled = false;
            cameras[0].enabled = true;
            activeCamera = 0;
        }
    }

    public void ChangeSpeed()
    {
        ouija.markerSpeed = speedSlider.value;
    }
    public void ChangeTransitionTime()
    {
        ouija.transitionTime = transitionSlider.value;
    }

}
