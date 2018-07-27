using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveAgent : MonoBehaviour {

    // patrol way point list
    public List<Transform> wayPoints;
    // next way point index
    public int nextIdx;

    readonly float patrolSpeed = 1.5f;
    readonly float traceSpeed = 4.0f;

    private NavMeshAgent agent;

    bool m_bPatrolling;
    bool Patrolling { get { return m_bPatrolling; } set { m_bPatrolling = value; if (m_bPatrolling) { agent.speed = patrolSpeed; MoveWayPoint(); } } }

    Vector3 m_vtTraceTarget;


	// Use this for initialization
	void Start () {
        // NavMeshAgent Initialize
        agent = GetComponent<NavMeshAgent>();
        // autoBraking option off
        agent.autoBraking = false;

        var group = GameObject.Find("WaypointGroup");
        if (group != null)
        {
            // export all transform component in waypointgroup 
            // add list
            group.GetComponentsInChildren<Transform>(wayPoints);
            // remove first child
            wayPoints.RemoveAt(0);
        }

        MoveWayPoint();
	}

    void MoveWayPoint()
    {
        // wait calculate path
        if (agent.isPathStale) return;

        agent.destination = wayPoints[nextIdx].position;
        // turn on navigation
        agent.isStopped = false;
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (agent.velocity.sqrMagnitude >= 0.2f * 0.2f
            && agent.remainingDistance <= 0.5f)
        {
            nextIdx = ++nextIdx & wayPoints.Count;
            // move next point
            MoveWayPoint();
        }
	}
}
