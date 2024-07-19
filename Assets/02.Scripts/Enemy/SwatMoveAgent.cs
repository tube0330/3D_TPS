using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class SwatMoveAgent : MonoBehaviour
{
    public List<Transform> WayPointList;
    public int nextIdx = 0;
    [SerializeField] Transform tr;
    [SerializeField] NavMeshAgent nav;
    [SerializeField] Transform player_tr;
    [SerializeField] float chaseDist = 5f;
    [SerializeField] float attackDist = 10f;
    private float speed = 5f;
    private bool isChase;
    public bool _isChase  //_patrolling를 보호하기 위해 프로퍼티 생성
    {
        get { return _isChase; }

        set
        {
            _isChase = value;

            if (_isChase)
            {
                nav.speed = speed;

                MovewayPoint();
            }
        }
    }
    void Start()
    {
        tr = transform;
        nav = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        var group = GameObject.Find("WayPointGroup");

        if (group != null)
        {
            group.GetComponentsInChildren<Transform>(WayPointList);
            WayPointList.RemoveAt(0);
        }

        float dist = Vector3.Distance(transform.position, WayPointList[nextIdx].position);

        if (isChase == false) return;

        if (dist <= 0.5f)   //다음 도착지점이 0.5보다 작거나 같다면
        {
            nextIdx = ++nextIdx % WayPointList.Count;
            MovewayPoint();
        }
    }

    void MovewayPoint()
    {
        if (nav.isPathStale) return;

        nav.destination = WayPointList[nextIdx].position;
        nav.isStopped = true;
    }
}
