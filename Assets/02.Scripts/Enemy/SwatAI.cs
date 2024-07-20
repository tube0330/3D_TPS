using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Codice.CM.Common.Merge;
using PlasticPipe.PlasticProtocol.Messages;
using UnityEngine;
[RequireComponent(typeof(Animator))]

public class SwatAI : MonoBehaviour
{
    public enum State
    {
        PATROL = 0, TRACE, ATTACK, DIE
    }

    SwatMoveAgent C_swatmove;
    SwatFire C_SwatFire;

    public State state = State.PATROL;
    public Rigidbody rb;

    [SerializeField] Transform playerTr;
    [SerializeField] Transform swatTr;
    [SerializeField] Animator ani;

    private WaitForSeconds wait;
    private float attackDist = 5f;
    private float traceDist = 10f;
    private bool isDie = false;

    private readonly int hashMove = Animator.StringToHash("isMove");
    private readonly int hashFire = Animator.StringToHash("Fire");
    private readonly int hashSpeed = Animator.StringToHash("walkSpeed");


    void Awake()
    {
        C_swatmove = GetComponent<SwatMoveAgent>();
        C_SwatFire = GetComponent<SwatFire>();

        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        swatTr = transform;
        playerTr = GameObject.Find("Player").GetComponent<Transform>();
        if (playerTr != null)
            playerTr = playerTr.GetComponent<Transform>();
        wait = new WaitForSeconds(0.3f);
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
                state = State.ATTACK;

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
                    C_SwatFire.isFire = false;
                    rb.isKinematic = false;
                    ani.SetBool(hashFire, true);
                    break;

                case State.ATTACK:
                    C_SwatFire.isFire = true;
                    rb.isKinematic = true;
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

    void Update()
    {
        ani.SetFloat(hashSpeed, C_swatmove.speed);
    }
}
