using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DataInfo;

public class Drop : MonoBehaviour, IDropHandler
{

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)  //자식 obj가 없다면
            Drag.draggingItem.transform.SetParent(transform, false);    //draggingItem의 부모 오브젝트는 Slot(본인)이 됨

        Item item = Drag.draggingItem.GetComponent<ItemInfo>().itemData;    //드래그중인 obj의 ItemInfo 클래스에 ItemData를 대입. 인벤토리 UI에서 올라간 아이템을 item에 대입

        GameManager.G_Instance.AddItem(item);   //인벤토리에서 아이템을 추가했을 때 데이터 정보를 업데이트하는 함수 불러옴
    }
}
