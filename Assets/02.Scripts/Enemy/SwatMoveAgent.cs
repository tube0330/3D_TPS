using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class SwatMoveAgent : MonoBehaviour
{
    public List<Transform> WayPointList;
    private int nextIdx = 0;
    [SerializeField] private Transform tr;
    [SerializeField] private NavMeshAgent nav;
    [SerializeField] private Animator ani;
    private bool isChase;
    void Start()
    {
        tr = transform;
        nav = GetComponent<NavMeshAgent>();
        isChase = true;
        ani = GetComponent<Animator>();        

        var group = GameObject.Find("WayPointGroup");

        if (group != null)
        {
            group.GetComponentsInChildren<Transform>(WayPointList);
            WayPointList.RemoveAt(0);
        }
        
        MovewayPoint();
        
    }

    void Update()
    {
        float dist = Vector3.Distance(tr.position, WayPointList[nextIdx].position);

        if (isChase == false) return;
        if (isChase)
        {
            ani.SetBool("isMove", true);
            if (dist <= 0.5f)
            {
                nextIdx = ++nextIdx % WayPointList.Count;
                MovewayPoint();
            }
        }
    }

    void MovewayPoint()
    {
        nav.destination = WayPointList[nextIdx].position;
        nav.isStopped = false;
    }
}
