using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Barrel : MonoBehaviour
{
    [SerializeField] private NavMeshAgent nav;
    [SerializeField] private Transform playerTr;

    [SerializeField] private Transform tr;
    [SerializeField] private Rigidbody rb;
    private readonly string E_BulletTag = "E_BULLET";

    void Start()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        playerTr = GameObject.FindWithTag("Player").transform;
        nav = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        nav.isStopped = false;
        nav.destination = playerTr.position;
        rb.freezeRotation = true;

        Vector3 Distance = playerTr.position - tr.position;
        if (Distance.magnitude >= 5f)
        {
            //
        }
    }
}
