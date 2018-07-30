using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MoveAgent : MonoBehaviour {

    // patrol way point list
    public List<Transform> wayPoints;
    // next way point index
    public int nextIdx;

    readonly float patrolSpeed = 1.5f;
    readonly float traceSpeed = 4.0f;
    float damping = 1.0f;

    NavMeshAgent agent;
    Transform enemyTr;

    bool m_bPatrolling;
    public bool Patrolling { 
        get { return m_bPatrolling; } 
        set 
        { 
            m_bPatrolling = value; 
            if (m_bPatrolling) 
            { 
                agent.speed = patrolSpeed; 
                damping = 1.0f; 
                MoveWayPoint(); 
            } 
        } 
    }
    public float speed { get { return agent.velocity.magnitude; } }

    Vector3 m_vtTraceTarget;
    public Vector3 traceTarget
    {
        get { return m_vtTraceTarget; }
        set
        {
            m_vtTraceTarget = value;
            agent.speed = traceSpeed;
            damping = 7.0f;
            TraceTarget(m_vtTraceTarget);
        }
    }

	// Use this for initialization
	void Start () {
        enemyTr = GetComponent<Transform>();
        // NavMeshAgent Initialize
        agent = GetComponent<NavMeshAgent>();
        // autoBraking option off
        agent.autoBraking = false;
        agent.updateRotation = false;
        agent.speed = patrolSpeed;

        var group = GameObject.Find("WaypointGroup");
        if (group != null)
        {
            // export all transform component in waypointgroup 
            // add list
            group.GetComponentsInChildren<Transform>(wayPoints);
            // remove first child
            wayPoints.RemoveAt(0);
        }

        //MoveWayPoint();
        this.Patrolling = true;
	}

    void MoveWayPoint()
    {
        // wait calculate path
        if (agent.isPathStale) return;

        agent.destination = wayPoints[nextIdx].position;
        // turn on navigation
        agent.isStopped = false;
    }
	

    // 주인공을 추적할 때 이동시키는 함수
    void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale) return;

        agent.destination = pos;
        agent.isStopped = false;
    }

    // 순찰 및 추적을 정지시키는 함수
    public void Stop()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        m_bPatrolling = false;
    }

	// Update is called once per frame
	void Update () 
    {
        if (agent.isStopped == false)
        {
            Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);
        }

        if (!m_bPatrolling) return;

        if (agent.velocity.sqrMagnitude >= 0.2f * 0.2f
            && agent.remainingDistance <= 0.5f)
        {
            nextIdx = ++nextIdx & wayPoints.Count;
            // move next point
            MoveWayPoint();
        }
	}
}
