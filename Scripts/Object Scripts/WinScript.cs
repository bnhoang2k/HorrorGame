using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScript : MonoBehaviour
{
    private Openable door;
    // Start is called before the first frame update
    void Start()
    {
        door = GetComponent<Openable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (door.open)
        {
            StartCoroutine(Win());
        }
    }
    IEnumerator Win()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("WinScreen");
    }
}
