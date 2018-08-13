using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour {

    private const string bulletTag = "BULLET";
    private const string enemyTag = "ENEMY";

    private float initHp = 100.0f;
    public float currHp;

    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;

	// Use this for initialization
	void Start () {
        currHp = initHp;
	}

    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == bulletTag)
        {
            Destroy(coll.gameObject);

            currHp -= 5.0f;
            Debug.Log("Player Hp = " + currHp.ToString());

            if (currHp <= 0.0f)
            {
                PlayerDie();
            }
        }
    }

    void PlayerDie()
    {
        Debug.Log("PlayerDie!");
        OnPlayerDie();
        //GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        //for (int i = 0; i < enemies.Length; i++)
        //{
        //    enemies[i].SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver
        //}
    }

    // Update is called once per frame
    void Update()
    {
		
	}
}
