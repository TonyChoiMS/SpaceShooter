using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {

    private Transform itemTr;
    private Transform inventoryTr;
    private Transform itemListTr;
    private CanvasGroup canvasGroup;

    public static GameObject draggingItem = null;

	// Use this for initialization
	void Start () 
    {
        itemTr = GetComponent<Transform>();
        inventoryTr = GameObject.Find("Inventory").GetComponent<Transform>();
        itemListTr = GameObject.Find("ItemList").GetComponent<Transform>();

        // CanvasGroup 컴포넌트 추출
        canvasGroup = GetComponent<CanvasGroup>();
	}

    // Drag Event
    public void OnDrag(PointerEventData eventData)
    {
        // when get drag event, change item position to mouse cursor
        itemTr.position = Input.mousePosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 부모를 Inventory로 변경
        this.transform.SetParent(inventoryTr);
        // 드래그가 시작되면 드래그되는 아이템 정보를 저장
        draggingItem = this.gameObject;
        // 드래그가 시작되면 다른 UI 이벤트를 받지 않도록 설정
        canvasGroup.blocksRaycasts = false;

    }

    // 드래그가 종료됬을 때 한번 호출되는 이벤트
    public void OnEndDrag(PointerEventData eventData)
    {
        // 드래그가 종료되면 드래그 아이템을 null로 변경
        draggingItem = null;
        // 드래그가 종료되면 다른 UI 이벤트를 받도록 설정
        canvasGroup.blocksRaycasts = true;
        // 슬롯에 드래그하지 않았을 때 원래대로 ItemList로 돌린다.
        if (itemTr.parent == inventoryTr)
        {
            itemTr.SetParent(itemListTr.transform);
            GameManager.instance.RemoveItem(GetComponent<ItemInfo>().itemData);
        }
    }
}
