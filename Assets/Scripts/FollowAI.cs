using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowAI : MonoBehaviour
{
    public Transform goal;
    public bool isFollowing;
    public NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        isFollowing = false;
        StartCoroutine(DelayFollow());
    }

    // Update is called once per frame
    void Update()
    {
        if(isFollowing)
        {
            agent.destination = goal.position;
        }
    }

    public IEnumerator DelayFollow()
    {
        yield return new WaitForSeconds(10f);
        transform.gameObject.AddComponent<NavMeshAgent>();
        agent = GetComponent<NavMeshAgent>();
        isFollowing = true;
    }
}