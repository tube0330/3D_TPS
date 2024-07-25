using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class SwatAI : MonoBehaviour
{
    public enum State
    {
        PATROL = 0, TRACE, ATTACK, DIE, PLAYERDIE
    }

    SwatMoveAgent C_swatmove;
    SwatFire C_SwatFire;

    public State state = State.PATROL;
    public Rigidbody rb;

    [SerializeField] Transform playerTr;
    [SerializeField] Transform swatTr;
    [SerializeField] Animator ani;
    [SerializeField] CapsuleCollider cap;

    private WaitForSeconds wait;
    private float attackDist = 5f;
    private float traceDist = 10f;
    private bool isDie = false;

    private readonly int hashMove = Animator.StringToHash("isMove");
    private readonly int hashFire = Animator.StringToHash("Fire");
    private readonly int hashSpeed = Animator.StringToHash("walkSpeed");
    private readonly int hashDie = Animator.StringToHash("Die");
    private readonly int hashDieIndex = Animator.StringToHash("dieIdx");
    private readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");


    void Awake()
    {
        C_swatmove = GetComponent<SwatMoveAgent>();
        C_SwatFire = GetComponent<SwatFire>();

        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        cap = GetComponent<CapsuleCollider>();
        swatTr = transform;
        playerTr = GameObject.Find("Player").GetComponent<Transform>();
        if (playerTr != null)
            playerTr = playerTr.GetComponent<Transform>();
        wait = new WaitForSeconds(0.3f);
    }

    private void OnEnable()
    {
        Damage.OnPlayerDie += OnPlayerDie;

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
                    SwatDie();
                    GameManager.G_Instance.KillScore();
                    break;

                case State.PLAYERDIE:
                    C_SwatFire.isFire = false;
                    rb.isKinematic = true;
                    OnPlayerDie();
                    break;
            }
        }

    }

    private void SwatDie()
    {
        C_swatmove.Stop();
        C_SwatFire.isFire = false;
        isDie = true;
        rb.isKinematic = true;
        cap.enabled = false;

        ani.SetTrigger(hashDie);
        ani.SetInteger(hashDieIndex, Random.Range(0, 2));
        gameObject.tag = "Untagged";
        state = State.DIE;
        StartCoroutine(ObjectPoolPush());
    }

    IEnumerator ObjectPoolPush()
    {
        yield return new WaitForSeconds(3f);

        isDie = false;
        rb.isKinematic = false;
        cap.enabled = true;
        gameObject.tag = "SWAT";
        gameObject.SetActive(false);
    }

    void OnPlayerDie()
    {
        state = State.PLAYERDIE;
        StopAllCoroutines();    //모든 코루틴 종료
        ani.SetTrigger(hashPlayerDie);
        GameManager.G_Instance.isGameOver = true;
    }

    void Update()
    {
        ani.SetFloat(hashSpeed, C_swatmove.speed);
    }

    void OnDisable()
    {
        Damage.OnPlayerDie -= OnPlayerDie;
    }
}
