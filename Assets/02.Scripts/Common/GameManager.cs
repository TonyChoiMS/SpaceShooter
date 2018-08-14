using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [Header("Enemy Create Info")]
    // 적 캐릭터가 출현할 위치를 담을 배열
    public Transform[] points;
    // 적 캐릭터 프리팹을 저장할 변수
    public GameObject enemy;
    // 적 캐릭터를 생성할 주기
    public float createTime = 2.0f;
    // 적 캐릭터의 최대 생성 개수
    public int maxEnemy = 10;
    // 게임 종료 여부를 판단할 변수
    public bool isGameOver = false;

    // 싱글턴
    public static GameManager instance = null;

    [Header("Object Pool")]
    // 생성할 총알 프리팹
    public GameObject bulletPrefab;
    public int maxPool = 10;
    public List<GameObject> bulletPool = new List<GameObject>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        CreatePooling();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public GameObject GetBullet()
    {
        for (int i = 0; i < bulletPool.Count; i++)
        {
            if (bulletPool[i].activeSelf == false)
            {
                return bulletPool[i];
            }
        }
        return null;
    }

    public void CreatePooling()
    {
        GameObject objectPools = new GameObject("ObjectPools");

        for (int i = 0; i < maxPool; i++)
        {
            var obj = Instantiate<GameObject>(bulletPrefab, objectPools.transform);
            obj.name = "Bullet_" + i.ToString("00");
            obj.SetActive(false);
            bulletPool.Add(obj);
        }
    }
}
