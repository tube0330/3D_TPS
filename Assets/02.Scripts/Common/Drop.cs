using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour, IDropHandler
{

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)  //자식 obj가 없다면
            Drag.draggingItem.transform.SetParent(transform, false);    //draggingItem의 부모 오브젝트는 Slot(본인)이 됨
    }
}
