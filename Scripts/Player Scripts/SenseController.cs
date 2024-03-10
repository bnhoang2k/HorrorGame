using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class SenseController : MonoBehaviour
{

    public RawImage blindness_image;

    public void SetVision(bool hasEyes) {
        if (blindness_image) {blindness_image.enabled = !hasEyes;}
    }

}
