using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    public float viewRange = 15f;     //적 캐릭터의 추적 사정 거리

    [Range(0, 360)] public float viewAngle = 120f;    //적 캐릭터의 시야각
    [SerializeField] Transform tr;
    [SerializeField] Transform playertr;
    [SerializeField] int playerLayer;
    [SerializeField] int barrelLayer;
    [SerializeField] int boxLayer;
    [SerializeField] int layerMask;


    void Start()
    {
        tr = transform;
        playertr = GameObject.FindWithTag("Player").GetComponent<Transform>();

        playerLayer = LayerMask.NameToLayer("Player");
        boxLayer = LayerMask.NameToLayer("BOXES");
        barrelLayer = LayerMask.NameToLayer("BARREL");
        layerMask = 1 << playerLayer | 1 << boxLayer | 1 << barrelLayer;
    }

    public Vector3 CirclePoint(float angle) //원주의 시작점을 알기 위해
    {
        angle += transform.eulerAngles.y;    //로컬좌표계 기준으로 설정하기 위해서 적캐릭터의 Y 회전값을 더함

        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0f, Mathf.Cos(angle * Mathf.Deg2Rad/*변환상수로 (PI * 2)/360과 같음*/));   //일반 각도를 Radian으로 변환
    }

    public bool isTracePlayer()     //Payer 추적 할말 판단
    {
        bool isTrace = false;

        Collider[] cols = Physics.OverlapSphere(tr.position, viewRange, 1 << playerLayer);  //Enemy 위치에서 viewRange만큼의 범위 안에 player가 있는지 추출

        if (cols.Length == 1)   //반경에 있음
        {
            Vector3 dir = (playertr.position - tr.position).normalized;

            if (Vector3.Angle(tr.forward, dir) < viewAngle * 0.5f)
                isTrace = true;
        }

        return isTrace;
    }

    public bool isViewPlayer()  //장애물 있는지 보고 플레이어 공격 할말
    {
        bool isView = false;
        RaycastHit hit;

        Vector3 dir = (playertr.position - tr.position).normalized;

        if (Physics.Raycast(tr.position, dir, out hit, viewRange, layerMask))    //Raycast로 장애물이 있는지 판단
        {
            isView = hit.collider.CompareTag("Player");
        }

        return isView;
    }
}