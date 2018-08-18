using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IDragHandler {

    private Transform itemTr;
    private Transform inventoryTr;

	// Use this for initialization
	void Start () 
    {
        itemTr = GetComponent<Transform>();
        inventoryTr = GameObject.Find("Inventory").GetComponent<Transform>();
	}

    // Drag Event
    public void OnDrag(PointerEventData eventData)
    {
        // when get drag event, change item position to mouse cursor
        itemTr.position = Input.mousePosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        this.transform.SetParent(inventoryTr);
    }
}
