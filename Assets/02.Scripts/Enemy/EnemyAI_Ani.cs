using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

[RequireComponent(typeof(Animator))]

//애니메이션 구현
public class EnemyAI_Ani : MonoBehaviour
{
    public enum State
    {
        PTROL = 0, TRACE, ATTACK, DIE, PLAYERDIE //앞에서 0으로 했으니까 뒤는 자동으로 1 2 3 들어감 ->enum이라서
    }

    private EnemyMoveAgent C_moveAgent;
    private EnemyFire C_enemyFire;
    [SerializeField] private EnemyFOV C_enemyFOV;
    //private Pet C_Pet;
    public State state = State.PTROL;   //상태(State) 안에 있는 PTROL을 상태를 받을 수 있는 변수 state에 넘김
    [SerializeField] private Transform playerTr;    //거리를 재기 위해 선언
    [SerializeField] private Transform enemyTr;     //거리를 재기 위해 선언
    [SerializeField] private Animator ani;

    public float attackDist = 5.0f;   //attack 범위 설정
    private float traceDist = 10f;     //추적 범위 설정
    public bool isDie = false;  //플레이어의 사망 여부 판단
    private WaitForSeconds wait;

    private readonly int hashMove = Animator.StringToHash("isMove");    //애니메이터컨트롤러에 정의한 parameter의 hash값을 정수로 미리 추출(정수형이 string보다 훨씬 빠르니까)
    private readonly int hashSpeed = Animator.StringToHash("moveSpeed");
    private readonly int hashDie = Animator.StringToHash("Die");
    private readonly int hashDieIndex = Animator.StringToHash("DieIdx");
    private readonly int hashOffset = Animator.StringToHash("offset");
    private readonly int hashWalkSpeed = Animator.StringToHash("walkSpeed");
    private readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");
    private readonly int hashBarrelDie = Animator.StringToHash("BarrelDie");

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
        //C_Pet = GetComponent<Pet>();
        C_enemyFOV = GetComponent<EnemyFOV>();
    }

    private void OnEnable() //오브젝트가 활성화될 때마다 호출
    {
        Damage.OnPlayerDie += OnPlayerDie;  //damage class의 delegate. 이벤트 연결

        ani.SetFloat(hashOffset, Random.Range(0.2f, 1.0f));
        ani.SetFloat(hashWalkSpeed, Random.Range(1f, 2f));
        StartCoroutine(CheckState());
        StartCoroutine(Action());
    }

    IEnumerator CheckState()
    {
        yield return new WaitForSeconds(1f);    //objectpool에 생성시 다른 스크립트의 초기화를 위해 대기하게함

        //플레이어와 적 사이의 거리를 구해서 어떤 상태인지만 파악중
        while (!isDie)
        {
            if (state == State.DIE) yield break;    //사망 상태이면 Coroutine 함수 종료

            float dist = (playerTr.position - enemyTr.position).magnitude;

            if (dist <= attackDist)
            {
                if (C_enemyFOV.isViewPlayer())
                    state = State.ATTACK;

                else state = State.TRACE;
            }
            // C_Pet.transform.position = new

            else if (C_enemyFOV.isTracePlayer())
                state = State.TRACE;

            /* else if ()
                state = State.PLAYERDIE; */

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
                    GetComponent<Rigidbody>().isKinematic = false;
                    C_enemyFire.isFire = false;
                    C_moveAgent.patrolling = true;
                    ani.SetBool(hashMove, true);
                    break;

                case State.TRACE:
                    GetComponent<Rigidbody>().isKinematic = false;
                    C_enemyFire.isFire = false;
                    C_moveAgent.traceTarget = playerTr.position;
                    ani.SetBool(hashMove, true);
                    break;

                case State.ATTACK:
                    GetComponent<Rigidbody>().isKinematic = true;
                    C_enemyFire.isFire = true;
                    C_moveAgent.Stop();
                    ani.SetBool(hashMove, false);
                    break;

                case State.DIE:
                    GetComponent<Rigidbody>().isKinematic = true;
                    EnemyDie();
                    GameManager.G_Instance.KillScore();
                    break;

                case State.PLAYERDIE:
                    C_enemyFire.isFire = false;
                    GetComponent<Rigidbody>().isKinematic = true;
                    OnPlayerDie();
                    break;
            }
        }
    }

    private void EnemyDie()
    {
        C_enemyFire.isFire = false;
        C_moveAgent.Stop();
        isDie = true;
        ani.SetTrigger(hashDie);
        ani.SetInteger(hashDieIndex, Random.Range(0, 2));

        //죽고나서 물리 제거
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<CapsuleCollider>().enabled = false;
        gameObject.tag = "Untagged";    //사망했다면 태그 뺌
        StartCoroutine(ObjectPoolPush());
    }

    IEnumerator ObjectPoolPush()
    {
        yield return new WaitForSeconds(3f);
        isDie = false;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<CapsuleCollider>().enabled = true;
        GetComponent<EnemyDamage>().E_HP = 100;
        state = State.PTROL;
        gameObject.tag = "ENEMY"; ;     //오브젝트가 활성화 되기 전 태그 이름 줌
        gameObject.SetActive(false);
    }

    void OnPlayerDie()
    {
        StopAllCoroutines();    //모든 코루틴 종료
        ani.SetTrigger(hashPlayerDie);
        GameManager.G_Instance.isGameOver = true;

    }

    void BarrelDie()
    {
        if (isDie) return;
        C_enemyFire.isFire = false;
        C_moveAgent.Stop();
        isDie = true;

        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<CapsuleCollider>().enabled = false;
        gameObject.tag = "Untagged";    //사망했다면 태그 뺌
        StartCoroutine(ObjectPoolPush());
        state = State.DIE;
        isDie = true;
        ani.SetTrigger(hashBarrelDie);
        StartCoroutine(ObjectPoolPush());
    }

    void Update()
    {
        ani.SetFloat(hashSpeed, C_moveAgent.speed);
    }

    void OnDisable()
    {
        Damage.OnPlayerDie -= OnPlayerDie;
    }
}
