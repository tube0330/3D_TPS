using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [SerializeField] private Transform camTr;
    [SerializeField] Transform target;
    [SerializeField] private float h = 5.0f;    //카메라 높이
    [SerializeField] private float distance = 7.0f;    //target과의 거리
    [SerializeField] private float movedamping = 10f;   //카메라가 이동 회전시 떨림을 부드럽게 완화하도록 값 지정
    [SerializeField] private float rotdamping = 15f;
    [SerializeField] private float targetOffset = 2.0f; //target에서의 카메라 높이값
    void Start()
    {
        /* target = GameObject.FindWithTag("player").GetComponent<Transform>();
         * target을 Player로 고정시키지 않는 이유는 카메라가 Player를 따라가야만 하는건 아니니까
         */

        camTr = transform;
    }
     void Update()
     {
        if (camTr == null) return;


     }
    void LateUpdate()
    {
        var camPos = target.position/*target 포지션에서*/ - (target.forward * distance)/*distance만큼 뒤에 있고*/ + (target.up * h);/*위에 있음*/
        camTr.position = Vector3.Slerp(transform.position, camPos, Time.deltaTime * movedamping);
        camTr.rotation = Quaternion.Slerp(camTr.rotation, target.rotation, Time.deltaTime * rotdamping);
        camTr.LookAt(target.position + (target.up * targetOffset)); //target position에서 targetOffset(2)만큼 올림
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 255, 0);
        Gizmos.DrawSphere(target.position + (target.up * targetOffset), 0.1f/*반경*/);
       // Gizmos.DrawLine(target.position + (target.up * targetOffset), camTr.position);
    }
}
