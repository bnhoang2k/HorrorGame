using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableEnemy : MonoBehaviour
{
    public GameObject enemy;
    public bool disableEnemy = false;
    // Start is called before the first frame update
    void Start()
    {
        if (disableEnemy) {
            enemy.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
