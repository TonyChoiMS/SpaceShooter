using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// shoot bullet and reload audio clip struct
[System.Serializable]
public struct PlayerSfx
{
    public AudioClip[] fire;
    public AudioClip[] reload;
}

public class CsFireCtrl : MonoBehaviour {

    // Weapon Type enum
    public enum WeaponType
    {
        RIFLE = 0,
        SHOTGUN
    }

    public WeaponType currWeapon = WeaponType.RIFLE;

    // Bullet Prefab
    public GameObject m_goBullet;
    // Bullet Cartrige
    public ParticleSystem m_psCartridge;
    // bullet fire position
    public Transform m_trFirePos;
    // Gun Fire Effect
    private ParticleSystem m_psMuszzleFlash;
    // AudioSource Component Variable
    AudioSource m_audio;
    // AudioClip Variable
    public PlayerSfx playerSfx;

    // Use this for initialization
    void Start () {
        m_psMuszzleFlash = m_trFirePos.GetComponentInChildren<ParticleSystem>();
        m_audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        // input mouse left click, call Fire Func
        if (Input.GetMouseButtonDown(0)) 
        {
            Fire();    
        }

	}

    void Fire() 
    {
        // Dynamic Creation Bullet Prefab
        Instantiate(m_goBullet, m_trFirePos.position, m_trFirePos.rotation);
        // Play Particle System.
        m_psCartridge.Play();
        m_psMuszzleFlash.Play();

        FireSfx();
    }

    void FireSfx()
    {
        var _sfx = playerSfx.fire[(int)currWeapon];
        // play sound
        m_audio.PlayOneShot(_sfx, 1.0f);
    }
}
