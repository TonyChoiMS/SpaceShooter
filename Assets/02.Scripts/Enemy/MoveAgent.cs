using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAgent : MonoBehaviour {

    // patrol way point list
    public List<Transform> wayPoints;
    // next way point index
    public int nextIdx;

	// Use this for initialization
	void Start () {
        var group = GameObject.Find("WaypointGroup");
        if (group != null)
        {
            // export all transform component in waypointgroup 
            // add list
            group.GetComponentsInChildren<Transform>(wayPoints);
            // remove first child
            wayPoints.RemoveAt(0);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
