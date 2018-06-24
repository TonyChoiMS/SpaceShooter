using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// if you want expose class in Inspector View, 
// you must be declare [System.Serializable] Attribute
[System.Serializable]
public class PlayerAnim
{
    public AnimationClip idle;
    public AnimationClip runF;
    public AnimationClip runB;
    public AnimationClip runL;
    public AnimationClip runR;
}

public class PlayerCtrl : MonoBehaviour {
    float h = 0.0f;
    float v = 0.0f;
    float r = 0.0f;

    // Component must be used to assignment variable
    Transform tr;
    // move speed variable (exposed to Inspector assign public)
    public float m_flMoveSpeed = 10.0f;
    // rotate speed variable
    public float m_flRotSpeed = 80.0f;

    // Animation class variable when expose Inspector View
    public PlayerAnim m_playerAnim;
    // this variable is save to Animation Component
    // when you connect animClip, 
    // non serialize class
    [HideInInspector] // = [System.NonSerialize && private
    public Animation m_anim;

	// Use this for initialization
	void Start () {
        // Initialize Component
        tr = GetComponent<Transform>();

        // Intialize Animation Component;
        m_anim = GetComponent<Animation>();
        // save AnimationClip in Animation Component and play
        m_anim.clip = m_playerAnim.idle;
        m_anim.Play();
	}
	
	// Update is called once per frame
	void Update () {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        r = Input.GetAxis("Mouse X");

        Debug.Log("h=" + h.ToString());
        Debug.Log("v=" + v.ToString());
        Debug.Log("r = " + r.ToString());

        // foward, back, left, right moving direction vector
        Vector3 m_vtMoveDir = (Vector3.forward * v) + (Vector3.right * h);

        // Translate(MoveDirection * speed * displacement * Time.deltaTime, standard location)
        tr.Translate(m_vtMoveDir.normalized * m_flMoveSpeed * Time.deltaTime, Space.Self);

        // Rotate for m_flRotSpeed to benchmark Vector3.up
        tr.Rotate(Vector3.up * m_flRotSpeed * Time.deltaTime * r);

        // play animation benchmark keyboard input
        if (v >= 0.1f) 
        {
            m_anim.CrossFade(m_playerAnim.runF.name, 0.3f); // moving forward animation    
        }
        else if (v <= -0.1f)
        {
            m_anim.CrossFade(m_playerAnim.runB.name, 0.3f); // moving back animation
        }
        else if (h >= 0.1f)
        {
            m_anim.CrossFade(m_playerAnim.runR.name, 0.3f); // moving right animation
        }
        else if (h <= -0.1f)
        {
            m_anim.CrossFade(m_playerAnim.runL.name, 0.3f); // moving left animation
        }
        else
        {
            m_anim.CrossFade(m_playerAnim.idle.name, 0.3f); // idle animation when stop character
        }

	}
}
