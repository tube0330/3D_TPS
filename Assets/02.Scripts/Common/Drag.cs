using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public static GameObject draggingItem = null;
    private Transform itemTr;
    private Transform inventoryTr;
    private Transform itemList;
    private CanvasGroup canvasGroup;

    void Start()
    {
        itemTr = GetComponent<RectTransform>();
        inventoryTr = GameObject.Find("Inventory").transform;
        itemList = GameObject.Find("Image_ItemList").transform;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnDrag(PointerEventData eventData)  //드래그 이벤트가 발생하면
    {
        itemTr.position = Input.mousePosition;    // 아이템의 위치를 마우스 커서의 위치로 변경
    }

    public void OnBeginDrag(PointerEventData eventData) //드래그를 시작할 때
    {
        this.transform.SetParent(inventoryTr);  //부모 오브젝트는 인벤토리가 됨
        draggingItem = this.gameObject;
        canvasGroup.blocksRaycasts = false; //다른 UI 이벤트와 상호작용이 일어나지 않음
    }

    public void OnEndDrag(PointerEventData eventData)   //드래그가 끝나면
    {
        draggingItem = null;
        canvasGroup.blocksRaycasts = true;  //다른 UI 이벤트를 받아서 OnDrop이 되겠지

        if (itemTr.parent == inventoryTr)   //드래그 시작하자마자 부모가 Inventory가 되니까
            itemTr.SetParent(itemList.transform);   //끌고갔는데 슬롯에 안넣었으면 본래 자리(Image_ItemList)로 돌아오도록
    }
}
