using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pet : MonoBehaviour
{
    [SerializeField] private NavMeshAgent nav;
    [SerializeField] private Transform playerTr;

    [SerializeField] private Transform tr;
    [SerializeField] private Rigidbody rb;
    //[SerializeField] private Transform EnemyTr;
    [SerializeField] string E_bulletTag = "E_BULLET";


    void Start()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        playerTr = GameObject.FindWithTag("Player").transform;
        nav = GetComponent<NavMeshAgent>();
        //EnemyTr = GameObject.FindWithTag("ENEMY").transform;
    }

    void Update()
    {
        nav.isStopped = false;
        nav.destination = playerTr.position;
        rb.freezeRotation = true;

        //Vector3 Distance = playerTr.position - tr.position;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(E_bulletTag))
            col.gameObject.SetActive(false);
    }
}
