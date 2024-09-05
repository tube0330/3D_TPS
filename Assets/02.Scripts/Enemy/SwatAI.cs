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
    SwatFire C_swatFire;
    SwatFOV C_swatFOV;

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

    IEnumerator Start()
    {
        C_swatmove = GetComponent<SwatMoveAgent>();
        C_swatFire = GetComponent<SwatFire>();
        C_swatFOV = GetComponent<SwatFOV>();

        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        cap = GetComponent<CapsuleCollider>();
        swatTr = transform;
        yield return new WaitForSeconds(1.5f);
        playerTr = GameObject.FindWithTag("Player").transform;
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
        yield return new WaitForSeconds(2f);
        while (!isDie)
        {
            if (state == State.DIE) yield break;

            float distance = (playerTr.position - swatTr.position).magnitude;

            if (distance <= attackDist)
            {
                if (C_swatFOV.isTracePlayer())
                    state = State.ATTACK;

                else state = State.TRACE;
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
                    if (playerTr != null)  // playerTr가 null인지 체크
                    {
                        C_swatmove.Pub_traceTarget = playerTr.position;
                        C_swatFire.isFire = false;
                        rb.isKinematic = false;
                        ani.SetBool(hashFire, true);
                    }
                    break;

                case State.ATTACK:
                    C_swatFire.isFire = true;
                    rb.isKinematic = true;
                    C_swatmove.Stop();
                    ani.SetBool(hashMove, false);
                    break;

                case State.DIE:
                    SwatDie();
                    GameManager.G_Instance.KillScore();
                    break;

                case State.PLAYERDIE:
                    C_swatFire.isFire = false;
                    rb.isKinematic = true;
                    OnPlayerDie();
                    break;
            }
        }

    }

    private void SwatDie()
    {
        C_swatmove.Stop();
        C_swatFire.isFire = false;
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
        GetComponent<SwatDamage>().HP = 100;
        state = State.PATROL;
        gameObject.tag = "ENEMY";
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
