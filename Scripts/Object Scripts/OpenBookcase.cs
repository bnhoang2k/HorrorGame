using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenBookcase : MonoBehaviour
{
    public GameObject closedBookcase;
    public GameObject openBookcase;
    private MissingBookcaseItem[] missingItems;
    private List<GameObject> missingItemsList;
    private Renderer rend;
    private AudioSource audioSource;
    public AudioClip putDownSound;
    public AudioClip openSound;
    public GameObject[] candles;

    // Start is called before the first frame update
    void Start()
    {
        missingItems = FindObjectsOfType<MissingBookcaseItem>();

        missingItemsList = new List<GameObject>();

        foreach (MissingBookcaseItem item in missingItems) {
            missingItemsList.Add(item.gameObject);
        }

        rend = openBookcase.GetComponent<Renderer>();
        rend.enabled = false;

        // turn off candles on the inside of the bookcase compartment
        foreach (GameObject candle in candles) {
            foreach (Light light in candle.GetComponentsInChildren<Light>()) {
                light.enabled = false;
            }
        }

        // get audio source
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (missingItemsList.Count == 0 && closedBookcase) {
            // all items have been returned

            // play open sound
            if (audioSource && openSound) {
                audioSource.PlayOneShot(openSound);
            } else { Debug.Log("No open sound: " + gameObject.name); }

            // open the bookcase
            rend.enabled = true;
            Destroy(closedBookcase);

            // turn on candles on the inside of the bookcase compartment
            foreach (GameObject candle in candles) {
            foreach (Light light in candle.GetComponentsInChildren<Light>()) {
                light.enabled = true;
            }
        }
        }
    }

    public void RemoveMissingItem(GameObject item) {
        if (missingItemsList.Contains(item)) {
            missingItemsList.Remove(item);
        }

        // play put down sound
        if (audioSource && putDownSound) {
            audioSource.PlayOneShot(putDownSound);
        } else { Debug.Log("No put down sound: " + gameObject.name); }
    }
}
