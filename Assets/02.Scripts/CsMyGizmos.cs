using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsMyGizmos : MonoBehaviour {

    public Color _color = Color.yellow;
    public float m_flRadius = 0.1f;

    void OnDrawGizmos()
    {
        // set gizmos color
        Gizmos.color = _color;      
        // create Sphere gizmos params(create position, sphere radius)
        Gizmos.DrawSphere(transform.position, m_flRadius);      

    }
}
