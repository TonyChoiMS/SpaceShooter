using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour {
    float h = 0.0f;
    float v = 0.0f;

    // Component must be used to assignment variable
    [SerializeField] Transform tr;
    // move speed variable (exposed to Inspector assign public)
    public float moveSpeed = 10.0f;

	// Use this for initialization
	void Start () {
        // Initialize Component
        tr = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        Debug.Log("h=" + h.ToString());
        Debug.Log("v=" + v.ToString());

        // Translate(MoveDirection * speed * displacement * Time.deltaTime, standard location)
        tr.Translate(Vector3.forward * moveSpeed * v * Time.deltaTime, Space.Self);
	}
}
