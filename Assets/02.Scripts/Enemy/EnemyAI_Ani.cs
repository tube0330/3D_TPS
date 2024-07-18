using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]

//애니메이션 구현
public class EnemyAI_Ani : MonoBehaviour
{
    public enum State
    {
        PTROL = 0, TRACE, ATTACK, DIE //앞에서 0으로 했으니까 뒤는 자동으로 1 2 3 들어감 ->enum이라서
    }

    private EnemyMoveAgent C_moveAgent;
    private EnemyFire C_enemyFire;
    public State state = State.PTROL;   //상태(State) 안에 있는 PTROL을 상태를 받을 수 있는 변수 state에 넘김
    [SerializeField] private Transform playerTr;    //거리를 재기 위해 선언
    [SerializeField] private Transform enemyTr;     //거리를 재기 위해 선언
    [SerializeField] private Animator ani;

    public float attackDist = 5.0f;   //attack 범위 설정
    public float traceDist = 15f;     //추적 범위 설정
    public bool isDie = false;  //플레이어의 사망 여부 판단
    private WaitForSeconds wait;

    private readonly int hashMove = Animator.StringToHash("isMove");    //애니메이터컨트롤러에 정의한 parameter의 hash값을 정수로 미리 추출(정수형이 string보다 훨씬 빠르니까)
    private readonly int hashSpeed = Animator.StringToHash("moveSpeed");

    void Awake()
    {
        ani = GetComponent<Animator>();
        C_moveAgent = GetComponent<EnemyMoveAgent>();
        C_enemyFire = GetComponent<EnemyFire>();

        var player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
            playerTr = player.GetComponent<Transform>();

        enemyTr = GetComponent<Transform>();
        wait = new WaitForSeconds(0.3f);
    }

    private void OnEnable() //오브젝트가 활성화될 때마다 호출
    {
        StartCoroutine(CheckState());
        StartCoroutine(Action());
    }

    IEnumerator CheckState()
    {
        //플레이어와 적 사이의 거리를 구해서 어떤 상태인지만 파악중
        while (!isDie)
        {
            if (state == State.DIE) yield break;    //사망 상태이면 Coroutine 함수 종료

            float dist = (playerTr.position - enemyTr.position).magnitude;

            if (dist <= attackDist)
                state = State.ATTACK;

            else if (dist <= traceDist)
                state = State.TRACE;

            else state = State.PTROL;

            yield return wait;
        }
    }

    IEnumerator Action()
    {
        while (!isDie)
        {
            yield return wait;  //wait에 넣은 숫자만큼 기다렸다가 아래 코드 실행
            switch (state)
            {
                case State.PTROL:
                    C_enemyFire.isFire = false;
                    C_moveAgent.patrolling = true;
                    ani.SetBool(hashMove, true);
                    break;

                case State.TRACE:
                    C_enemyFire.isFire = false;
                    C_moveAgent.traceTarget = playerTr.position;
                    ani.SetBool(hashMove, true);
                    break;

                case State.ATTACK:
                    C_enemyFire.isFire = true;
                    C_moveAgent.Stop();
                    ani.SetBool(hashMove, false);
                    break;

                case State.DIE:
                    C_enemyFire.isFire = false;
                    C_moveAgent.Stop();
                    break;
            }
        }
    }

    void Update()
    {
        ani.SetFloat(hashSpeed, C_moveAgent.speed);
    }
}
