using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DataInfo;

public class GameManager : MonoBehaviour
{
    [Header("Enemy Create Info")]
    //적 캐릭터가 출현할 위치를 담을 배열
    public Transform[] points;
    //적 캐릭터 프리팹을 저장할 변수
    public GameObject enemy;
    //적 캐릭터를 생성할 주기
    public float createTime = 2.0f;
    //적 캐릭터의 최대 생성 개수
    public int maxEnemy = 10;
    //게임 종료 여부를 판단할 변수
    public bool isGameOver = false;
    // 게임 일시정지
    public bool isPaused;

    //싱글턴에 접근하기 위한 Static 변수 선언
    public static GameManager instance = null;

    [Header("Object Pool")]
    //생성할 총알 프리팹
    public GameObject bulletPrefab;
    //오브젝트 풀에 생성할 개수
    public int maxPool = 10;
    public List<GameObject> bulletPool = new List<GameObject>();
    public CanvasGroup inventoryCG;

    // Player Kill count
    //[HideInInspector] public int killCount;
    [Header("GameData")]
    // kill count text
    public Text killCountTxt;
    private DataManager dataManager;
    public GameData gameData;

    public delegate void ItemChangeDelegate();
    public static event ItemChangeDelegate OnItemChange;

    // SlotList 게임오브젝트를 저장할 변수
    private GameObject slotList;
    public GameObject[] itemObjects;                                           

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        //instance에 할당된 클래스의 인스턴스가 다를 경우 새로 생성된 클래스를 의미함
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
        //다른 씬으로 넘어가더라도 삭제하지 않고 유지함
        DontDestroyOnLoad(this.gameObject);

        dataManager = GetComponent<DataManager>();
        dataManager.Initialize();

        // 인벤토리에 추가된 아이템을 검색하기 위해 SlotList 게임오브젝트 추출
        slotList = inventoryCG.transform.Find("SlotList").gameObject;
        // load game data
        LoadGameData();
        //오브젝트 풀링 생성함수 호출
        CreatePooling();
    }

    void SaveGameData()
    {
        dataManager.Save(gameData);
    }

    public void AddItem(Item item)
    {
        if (gameData.equipItem.Contains(item)) return;

        gameData.equipItem.Add(item);

        switch(item.itemType)
        {
            case Item.ItemType.HP:
                if (item.itemCalc == Item.ItemCalc.INC_VALUE)
                    gameData.hp += item.value;
                else
                    gameData.hp += gameData.hp * item.value;
                break;
            case Item.ItemType.DAMAGE:
                if (item.itemCalc == Item.ItemCalc.INC_VALUE)
                    gameData.damage += item.value;
                else
                    gameData.damage += gameData.damage * item.value;
                break;
            case Item.ItemType.SPEED:
                if (item.itemCalc == Item.ItemCalc.INC_VALUE)
                    gameData.speed += item.value;
                else
                    gameData.speed += gameData.speed * item.value;
                break;

            case Item.ItemType.GRENADE:
                break;
        }
    }

    public void RemoveItem(Item item)
    {
        gameData.equipItem.Remove(item);

        switch(item.itemType)
        {
            case Item.ItemType.HP:
                if (item.itemCalc == Item.ItemCalc.INC_VALUE)
                    gameData.hp -= item.value;
                else
                    gameData.hp = gameData.hp / (1.0f + item.value);
                break;

            case Item.ItemType.DAMAGE:
                if (item.itemCalc == Item.ItemCalc.INC_VALUE)
                    gameData.damage -= item.value;
                else
                    gameData.damage = gameData.damage / (1.0f + item.value);
                break;

            case Item.ItemType.SPEED:
                if (item.itemCalc == Item.ItemCalc.INC_VALUE)
                    gameData.speed -= item.value;
                else
                    gameData.speed = gameData.speed / (1.0f + item.value);
                break;

            case Item.ItemType.ItemType.GRENADE:
                break;
        }
        OnItemChange();
    }

    // Load Initialize Game Data
    void LoadGameData()
    {
        GameData data = dataManager.Load();

        gameData.hp = data.hp;
        gameData.damage = data.damage;
        gameData.speed = data.speed;
        gameData.killCount = data.killCount;
        gameData.equipItem = data.equipItem;
        //killCount = PlayerPrefs.GetInt("KILL_COUNT", 0);
        // 보유한 아이템이 있을 때만 호출
        if (gameData.equipItem.Count > 0)
        {
            InventorySetup();
        }

        killCountTxt.text = "Kill " + gameData.killCount.ToString("0000"); 
    }

    // Use this for initialization
    void Start()
    {
        OnInventoryOpen(false);     // 인벤토리 비활성화
        //하이러키 뷰의 SpawnPointGroup을 찾아 하위에 있는 모든 Transform 컴포넌트를 찾아옴
        points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        if (points.Length > 0)
        {
            StartCoroutine(this.CreateEnemy());
        }
    }

    void InventorySetup()
    {
        // SlotList 하위에 있는 모든 Slot을 추출
        var slots = slotList.GetComponentsInChildren<Transform>();

        // 보유한 아이템의 개수만큼 반복
        for (int i = 0; i < gameData.equipItem.Count; i++)
        {
            // 인벤토리 UI에 있는 Slot 개수만큼 반복
            for (int j = 0; j < slots.Length; j++)
            {
                // Slot 하위에 다른 아이템이 있으면 다음 인덱스로 넘어감
                if (slots[j].childCount > 0) continue;

                // 보유한 아이템의 종류에 따라 인덱스를 추출
                int itemIndex = (int)gameData.equipItem[i].itemType;

                // 아이템의 부모를 Slot 게임오브젝트로 변경
                itemObjects[itemIndex].GetComponent<Transform>().SetParent(slots[j]);
                // 아이템의 ItemInfo 클래스의 itemData에 로드한 데이터 값을 저장
                itemObjects[itemIndex].GetComponent<ItemInfo>().itemData = gameData.equipItem[i];

                // 아이템을 Slot에 추가하면 바깥 for 구문으로 빠져나감
                break;
            }
        }
    }

    //적 캐릭터를 생성하는 코루틴 함수
    IEnumerator CreateEnemy()
    {
        //게임 종료 시까지 무한 루프
        while (!isGameOver)
        {
            //현재 생성된 적 캐릭터의 개수 산출
            int enemyCount = (int)GameObject.FindGameObjectsWithTag("ENEMY").Length;
            //적 캐릭터의 최대 생성 개수보다 작을 때만 적 캐릭터를 생성
            if (enemyCount < maxEnemy)
            {
                //적 캐릭터의 생성 주기 시간만큼 대기
                yield return new WaitForSeconds(createTime);
                //불규칙적인 위치 산출
                int idx = Random.Range(1, points.Length);
                //적 캐릭터의 동적 생성
                Instantiate(enemy, points[idx].position, points[idx].rotation);
            }
            else
            {
                yield return null;
            }
        }
    }

    //오브젝트 풀에서 사용 가능한 총알을 가져오는 함수
    public GameObject GetBullet()
    {
        for (int i = 0; i < bulletPool.Count; i++)
        {
            //비활성화 여부로 사용 가능한 총알인지를 판단
            if (bulletPool[i].activeSelf == false)
            {
                return bulletPool[i];
            }
        }
        return null;
    }

    //오브젝트 풀에 총알을 생성하는 함수
    public void CreatePooling()
    {
        //총알을 생성해 차일드화할 페어런트 게임오브젝트를 생성
        GameObject objectPools = new GameObject("ObjectPools");
        //풀링 개수만큼 미리 총알을 생성
        for (int i = 0; i < maxPool; i++)
        {
            var obj = Instantiate<GameObject>(bulletPrefab, objectPools.transform);
            obj.name = "Bullet_" + i.ToString("00");
            //비활성화 시킴
            obj.SetActive(false);
            //리스트에 생성한 총알 추가
            bulletPool.Add(obj);
        }
    }

    // 일시 정지 버튼 클릭 시 호출할 함수
    public void OnPauseClick()
    {
        // 일시 정지 값을 토글시킴
        isPaused = !isPaused;
        // Time Scale이 0이면 정지, 1이면 정상 속도
        Time.timeScale = (isPaused) ? 0.0f : 1.0f;
        // 주인공 객체 추출
        var playerObj = GameObject.FindGameObjectWithTag("PLAYER");
        // 주인공 캐릭터에 추가된 모든 스크립트를 추출함
        var scripts = playerObj.GetComponents<MonoBehaviour>();
        // 주인공 캐릭터의 모든 스크립트를 활성화/비활성화
        foreach (var script in scripts)
        {
            script.enabled = !isPaused;
        }
        var canvasGroup = GameObject.Find("Panel - Weapon").GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = !isPaused;
    }

    public void OnInventoryOpen(bool isOpened)
    {
        inventoryCG.alpha = (isOpened) ? 1.0f : 0.0f;
        inventoryCG.interactable = isOpened;
        inventoryCG.blocksRaycasts = isOpened;
    }

    // call function when player kill enemy
    public void IncKillCount()
    {
        ++gameData.killCount;
        killCountTxt.text = "KILL " + gameData.killCount.ToString("0000");
        // save kill count
        //PlayerPrefs.SetInt("KILL_COUNT", killCount);
    }

    private void OnApplicationQuit()
    {
        SaveGameData();
    }
}