using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SwatMoveAgent : MonoBehaviour
{
    public List<Transform> WayPointList;

    [SerializeField] private Transform tr;
    [SerializeField] private NavMeshAgent nav;
    private float patrolSpeed = 7f;
    private float traceSpeed = 5.0f;
    private float damping = 1.0f;
    private int nextIdx = 0;

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
                damping = 1.0f;

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
            damping = 7.0f;


            TraceTarget(traceTarget);
        }
    }

    public float speed
    {
        get { return nav.velocity.magnitude; }
        set { nav.speed = value; }
    }

    void Start()
    {
        tr = transform;
        nav = GetComponent<NavMeshAgent>();
        nav.autoBraking = false;

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
        if (nav.isStopped == false)
        {
            Quaternion rot = Quaternion.LookRotation(nav.desiredVelocity);
            tr.rotation = Quaternion.Slerp(tr.rotation, rot, Time.deltaTime * damping);
        }

        float dist = Vector3.Distance(tr.position, WayPointList[nextIdx].position);

        if (patrol == false) return;

        if (dist <= 0.5f)
        {
            nextIdx = ++nextIdx % WayPointList.Count;
            MovewayPoint();
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
