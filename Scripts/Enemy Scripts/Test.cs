using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator anim;
    NavMeshAgent navAgent;
    void Start()
    {
        anim.SetBool("isWalking", true);
        anim.SetBool("isRunning", false);
        anim.SetBool("isIdle", false);
        navAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveToPlayer();
    }
    public void MoveToPlayer()
    {
        navAgent.SetDestination(GameObject.Find("PlayerCapsule").transform.position);
    }
    public void LookAtPlayer()
    {
        GameObject playerCapsule = GameObject.Find("PlayerCapsule");
        Vector3 playerDirection = playerCapsule.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(playerDirection);
        if (playerDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2.0f);
        }
    }
}
