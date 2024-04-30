using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffShadow : MonoBehaviour
{
    public MeshRenderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
