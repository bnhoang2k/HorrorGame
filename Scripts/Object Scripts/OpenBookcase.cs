using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenBookcase : MonoBehaviour
{
    public GameObject openableBookcase;
    private MissingBookcaseItem[] missingItems;
    private List<GameObject> missingItemsList;
    // Start is called before the first frame update
    void Start()
    {
        missingItems = FindObjectsOfType<MissingBookcaseItem>();

        missingItemsList = new List<GameObject>();

        foreach (MissingBookcaseItem item in missingItems) {
            missingItemsList.Add(item.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (missingItemsList.Count == 0) {
            // all items have been returned
            // open the bookcase
            Destroy(openableBookcase);
        }
    }

    public void RemoveMissingItem(GameObject item) {
        if (missingItemsList.Contains(item)) {
            missingItemsList.Remove(item);
        }
    }
}
