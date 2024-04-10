using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorboardSound : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip creak;
    private GameObject gameController;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("GameController");
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {

        // only "unlock" the secret compartment if the player could actually hear the creak
        if (audioSource.volume > 0) {
            audioSource.PlayOneShot(creak);

            // also change the floorboard's description, now that the player has heard the creak
            gameObject.GetComponent<Describable>().description = "Hollow floorboard";
            // "unlock" the floorboard, the player can now open it
            gameObject.GetComponent<OpenFloorboard>().locked = false;
        }
        
    }
}
