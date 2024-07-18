using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]    //이 오브젝트에서 navmeshagent가 없으면 안된다고 명시

public class EnemyMoveAgent : MonoBehaviour
{
    public List<Transform> WayPointList;    //패트롤 지점(위치)을 담기 위한 List Generic(일반형) 변수
    [SerializeField] private NavMeshAgent nav;
    private Transform enemyTr;
    private readonly float patrolSpeed = 7f;
    private readonly float traceSpeed = 4.0f;
    private float damping = 1.0f;    //회전할 때 속도 조절
    public int nextIdx = 0;  //다음 순찰 지점의 배열 인덱스 값
    private bool _patrolling;
    public bool patrolling  //_patrolling를 보호하기 위해 프로퍼티 생성
    {
        get { return _patrolling; }

        set
        {
            _patrolling = value;

            if (_patrolling)
            {
                nav.speed = patrolSpeed;
                damping = 1.0f;

                MovewayPoint();
            }
        }
    }

    private Vector3 _traceTarget;
    public Vector3 traceTarget
    {
        get { return _traceTarget; }
        set
        {
            _traceTarget = value;
            nav.speed = traceSpeed;
            damping = 7.0f;

            TraceTarget(_traceTarget);
        }
    }

    public float speed
    {
        get { return nav.velocity.magnitude; }  //navMeshAgent 속도
        set { nav.speed = value; }
    }

    void Start()
    {
        enemyTr = GetComponent<Transform>();
        nav = GetComponent<NavMeshAgent>();
        nav.autoBraking = false;
        nav.updateRotation = false; //부드럽지 않기 때문에 navmeshagent를 이용해 회전하는 기능 비활성화.

        var group = GameObject.Find("WayPointGroup");   //하이라키에 있는 오브젝트명이 WayPointGroup를 찾아 대입

        if (group != null)
        {
            group.GetComponentsInChildren<Transform>(WayPointList); //하위 오브젝트의 트랜스폼을 WayPointList에 다 담음
            WayPointList.RemoveAt(0);   //0번째 인덱스는 삭제(부모가 잡히니까 부모 빼고 싶어서)
        }

        MovewayPoint();
    }

    void Update()
    {
        if(nav.isStopped == false)
        {
            Quaternion rot = Quaternion.LookRotation(nav.desiredVelocity);  //NavmeshAgent가 가야할 방향 벡터를 쿼터니언 타입의 각도로 변환
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime*damping); //보간 함수를 이용해 점진적으로 부드럽게 회전시킴
        }

        float dist = Vector3.Distance(transform.position, WayPointList[nextIdx].position);  //현재 위치와 도착지점의 거리를 구함
        /* => float dist = (WayPointList[nextIdx].position - transform.position).magnitude; */
        if (_patrolling == false) return;

        if (dist <= 0.5f)   //다음 도착지점이 0.5보다 작거나 같다면
        {
            nextIdx = ++nextIdx % WayPointList.Count;
            MovewayPoint();
        }
    }

    void MovewayPoint()
    {
        if (nav.isPathStale) return;    //최단 경로 계산이 끝나지 않거나 길을 잃어버린 경우

        nav.destination = WayPointList[nextIdx].position;   //추적대상 = List에 담았던 트랜스폼
        nav.isStopped = false;
    }

    void TraceTarget(Vector3 pos)
    {
        if (nav.isPathStale) return;

        nav.destination = pos;
        nav.isStopped = false;
    }

    public void Stop()
    {
        nav.isStopped = true;
        nav.velocity = Vector3.zero;
        _patrolling = false;
    }
}