using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator anim;
    void Start()
    {
        anim.SetBool("isWalking", true);
        anim.SetBool("isRunning", false);
        anim.SetBool("isIdle", false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
