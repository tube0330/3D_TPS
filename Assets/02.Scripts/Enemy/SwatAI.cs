using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwatAI : MonoBehaviour
{
    public enum State
    {
        PATROL = 0, TRACE, ATTACK, DIE
    }

    SwatMoveAgent C_swatmove;
    EnemyFire C_enemyFire;

    [SerializeField] Transform playerTr;
    [SerializeField] Transform swatTr;
    [SerializeField] Animator ani;

    private readonly int hashMove = Animator.StringToHash("isMove");
    private readonly int hashFire = Animator.StringToHash("Fire");

    public State state = State.PATROL;
    public Rigidbody rb;

    private WaitForSeconds wait;
    private float attackDist = 5f;
    private float traceDist = 10f;
    private bool isDie = false;

    void Awake()
    {
        C_swatmove = GetComponent<SwatMoveAgent>();

        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        playerTr = GameObject.Find("Player").GetComponent<Transform>();
        wait = new WaitForSeconds(0.3f);
        swatTr = transform;
    }

    private void OnEnable()
    {
        StartCoroutine(CheckState());
        StartCoroutine(Action());
    }

    IEnumerator CheckState()
    {
        while (!isDie)
        {
            if (state == State.DIE) yield break;

            float distance = (playerTr.position - swatTr.position).magnitude;

            if (distance <= attackDist)
            {
                state = State.ATTACK;
            }

            else if (distance <= traceDist)
                state = State.TRACE;

            else state = State.PATROL;

            yield return wait;
        }
    }

    IEnumerator Action()
    {
        while (!isDie)
        {
            yield return wait;
            
            switch (state)
            {
                case State.PATROL:
                    C_swatmove.Pub_isPatrol = true;
                    rb.isKinematic = false;
                    ani.SetBool(hashMove, true);
                    break;

                case State.TRACE:
                    C_swatmove.Pub_traceTarget = playerTr.position;
                    rb.isKinematic = false;
                    ani.SetBool(hashFire, true);
                    break;

                case State.ATTACK:
                    C_enemyFire.isFire = true;
                    rb.isKinematic = false;
                    C_swatmove.Stop();
                    ani.SetBool(hashMove, false);
                    break;

                case State.DIE:
                    C_swatmove.Stop();
                    isDie = true;
                    break;
            }
        }

    }
}
