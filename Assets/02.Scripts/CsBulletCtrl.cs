using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsBulletCtrl : MonoBehaviour {

    public float m_flDamage;        // Bullet Power
    public float m_flSpeed;         // Bullet shoot speed

	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody>().AddForce(transform.forward * m_flSpeed);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
