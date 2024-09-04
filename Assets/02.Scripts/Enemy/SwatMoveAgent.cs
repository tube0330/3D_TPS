using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SwatMoveAgent : MonoBehaviour
{
    public List<Transform> WayPointList;

    [SerializeField] private Transform tr;
    [SerializeField] private NavMeshAgent agent;
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
                agent.speed = patrolSpeed;
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
            agent.speed = traceSpeed;
            damping = 7.0f;


            TraceTarget(traceTarget);
        }
    }

    public float speed
    {
        get { return agent.velocity.magnitude; }
        set { agent.speed = value; }
    }

    void Start()
    {
        tr = transform;
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;

        var group = GameObject.Find("WayPointGroup");

        if (group != null)
        {
            group.GetComponentsInChildren<Transform>(WayPointList);
            WayPointList.RemoveAt(0);
        }

        nextIdx = Random.Range(0, WayPointList.Count);
        MovewayPoint();
    }

    void Update()
    {
        if (agent.isStopped == false)
        {
            if (agent.desiredVelocity != Vector3.zero)
            {
                Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
                tr.rotation = Quaternion.Slerp(tr.rotation, rot, Time.deltaTime * damping);
            }
        }

        float dist = Vector3.Distance(tr.position, WayPointList[nextIdx].position);

        if (patrol == false) return;

        if (dist <= 0.5f)
        {
            //nextIdx = ++nextIdx % WayPointList.Count;     //순서대로 가기

            nextIdx = Random.Range(0, WayPointList.Count);  //랜덤한 포인트로 가기
            MovewayPoint();
        }
    }

    void MovewayPoint()
    {
        if (agent.isPathStale) return;

        agent.destination = WayPointList[nextIdx].position;
        agent.isStopped = false;
    }

    void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale) return;

        agent.destination = pos;
        agent.isStopped = false;
    }

    public void Stop()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        patrol = false;
    }
}
