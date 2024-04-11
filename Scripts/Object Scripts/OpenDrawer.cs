using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDrawer : MonoBehaviour
{
    public Animator ANI;
    // public AudioSource openSound;
    // public AudioSource closeSound;

    private Camera player_camera;
    private float arm_length;
    private bool open;

    // Start is called before the first frame update
    void Start()
    {
        ANI.SetBool("open", false);
        ANI.SetBool("close", false);
        open = false;

        player_camera = Camera.main;
        arm_length = GameObject.Find("Player").GetComponent<Reach>().arm_length;

    }

    public bool GetState()
    {
        return open;
    }

    public void Open()
    {
        // openSound.Play();
        Debug.Log("Open");
        ANI.SetBool("open", true);
        ANI.SetBool("close", false);
        open = true;
    }

    public void Close()
    {
        // closeSound.Play();
        Debug.Log("Close");
        ANI.SetBool("open", false);
        ANI.SetBool("close", true);
        open = false;
    }
    
}
