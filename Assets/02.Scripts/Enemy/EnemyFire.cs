using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour {

    private AudioSource audio;
    private Animator animator;
    private Transform playerTr;
    private Transform enemyTr;

    readonly int hashFire = Animator.StringToHash("Fire");
    readonly int hashReload = Animator.StringToHash("Reload");

    float nextFire = 0.0f;
    readonly float fireRate = 0.1f;
    readonly float damping = 10.0f;

    readonly float reloadTime = 2.0f;
    readonly int maxBullet = 10;
    int currBullet = 10;
    bool isReload = false;
    WaitForSeconds wsReload;

    public bool isFire = false;
    public AudioClip fireSfx;
    public AudioClip reloadSfx;

    public GameObject Bullet;
    public Transform firePos;
    public MeshRenderer muzzleFlash;

	// Use this for initialization
	void Start () {
        playerTr = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<Transform>();
        enemyTr = GameObject.FindGameObjectWithTag("ENEMY").GetComponent<Transform>();
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();

        wsReload = new WaitForSeconds(reloadTime);
	}
	
	// Update is called once per frame
	void Update () {
        if (isFire && !isReload)
        {
            if (Time.time >= nextFire)
            {
                Fire();
                nextFire = Time.time + fireRate + Random.Range(0.0f, 0.3f);
            }

            Quaternion rot = Quaternion.LookRotation(playerTr.position - enemyTr.position);
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);
        }
	}

    void Fire()
    {
        animator.SetTrigger(hashFire);
        audio.PlayOneShot(fireSfx, 1.0f);
        StartCoroutine(ShowMuzzleFlash());

        GameObject _bullet = Instantiate(Bullet, firePos.position, firePos.rotation);
        Destroy(_bullet, 3.0f);

        isReload = (--currBullet % maxBullet == 0);
        if (isReload)
        {
            StartCoroutine(Reloading());
        }
    }

    IEnumerator ShowMuzzleFlash()
    {
        muzzleFlash.enabled = true;
        Quaternion rot = Quaternion.Euler(Vector3.forward * Random.Range(0, 360));
        muzzleFlash.transform.localRotation = rot;
        muzzleFlash.transform.localScale = Vector3.one * Random.Range(1.0f, 2.0f);

        Vector2 offest = new Vector2(Random.Range(0, 2), Random.Range(0, 2)) * 0.5f;
        muzzleFlash.material.SetTextureOffset("_MainTex", offset);

        yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
        muzzleFlash.enabled = false;
    }

    IEnumerator Reloading()
    {
        animator.SetTrigger(hashReload);
        audio.PlayOneShot(reloadSfx, 1.0f);

        yield return wsReload;

        currBullet = maxBullet;
        isReload = false;
    }
}
