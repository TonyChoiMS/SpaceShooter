using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsBarrelCtrl : MonoBehaviour {

    // Explosion Effect 
    public GameObject m_goExpEffect;
    Rigidbody m_rb;
    public Mesh[] meshes;
    MeshFilter m_meshFilter;
    public Texture[] m_textures;
    MeshRenderer _renderer;
    AudioSource m_audio;

    // Explosion radius
    public float m_flExpRaudius = 10.0f;
    public AudioClip m_acExpSfx;

    // bullet hit count
    int m_nHitCount = 0;

	// Use this for initialization
	void Start () {
        m_rb = GetComponent<Rigidbody>();
        m_meshFilter = GetComponent<MeshFilter>();
        _renderer = GetComponent<MeshRenderer>();
        m_audio = GetComponent<AudioSource>();

        _renderer.material.mainTexture = m_textures[Random.Range(0, m_textures.Length)];
	}

    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("BULLET")) 
        {
            if (++m_nHitCount == 3)
            {
                ExpBarrel();
            }    
        }
    }

    // Explosion Effect func
    void ExpBarrel()
    {
        GameObject effect = Instantiate(m_goExpEffect, transform.position, Quaternion.identity);
        // Destroy Effect delay 2 seconds
        Destroy(effect, 2.0f);
        // edit rigidbody mass to weight low
        //m_rb.mass = 1.0f;
        // add force to up
        //m_rb.AddForce(Vector3.up * 1000.0f);

        // create explosion
        IndirectDamage(transform.position);

        // create random variable
        int idx = Random.Range(0, meshes.Length);
        // destroied barrel mesh
        m_meshFilter.sharedMesh = meshes[idx];
        GetComponent<MeshCollider>().sharedMesh = meshes[idx];

        m_audio.PlayOneShot(m_acExpSfx, 1.0f);
    }

    void IndirectDamage(Vector3 pos) 
    {
        // get in radius barrel
        Collider[] colls = Physics.OverlapSphere(pos, m_flExpRaudius, 1 << 8);
        foreach(var coll in colls)
        {
            // in radius barrel
            var _rb = coll.GetComponent<Rigidbody>();
            // low weight barrel
            _rb.mass = 1.0f;
            // send Explosion
            _rb.AddExplosionForce(1200.0f, pos, m_flExpRaudius, 1000.0f);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
