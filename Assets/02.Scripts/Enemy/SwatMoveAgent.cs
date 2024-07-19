using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.AI;

public class SwatMoveAgent : MonoBehaviour
{
    public List<Transform> WayPointList;

    [SerializeField] private Transform tr;
    [SerializeField] private NavMeshAgent nav;

    private int nextIdx = 0;
    private float patrolSpeed = 7f;
    private float traceSpeed = 5.0f;
    private bool patrol;
    public bool Pub_isPatrol
    {
        get { return patrol; }
        set
        {
            patrol = value;

            if (patrol)
            {
                nav.speed = patrolSpeed;

                MovewayPoint();
            }
        }
    }
    private Vector3 traceTarget;
    public Vector3 Pub_traceTarget
    {
        get { return traceTarget; }
        set
        {
            traceTarget = value;
            nav.speed = traceSpeed;

            TraceTarget(traceTarget);
        }
    }

    void Start()
    {
        tr = transform;
        nav = GetComponent<NavMeshAgent>();

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

        if (patrol == false) return;

        if (patrol)
        {
            if (dist <= 0.5f)
            {
                nextIdx = ++nextIdx % WayPointList.Count;
                MovewayPoint();
            }
        }
    }

    void MovewayPoint()
    {
        if (nav.isPathStale) return;
        nav.destination = WayPointList[nextIdx].position;
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
        patrol = false;
    }
}
