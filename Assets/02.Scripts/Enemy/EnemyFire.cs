using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using PlasticPipe.PlasticProtocol.Messages;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    [SerializeField] private Transform playerTr;
    /* 플레이어가 있는 위치와 방향으로 총을 쏴야 하기 때문에 플레이어와 enemy의 transform을 가져온다.  */
    [SerializeField] private Transform enemyTr;
    [SerializeField] private Transform firePos;
    [SerializeField] private AudioClip fireClip;
    [SerializeField] private Animator ani;

    private readonly int hashFire = Animator.StringToHash("Fire");
    private readonly int hashReload = Animator.StringToHash("Reload");
    private float nextFireTime = 0.0f;  //다음 발사 시간 계산용 변수 생성
    private readonly float fireInterval = 0.1f; //총알 발사 간격
    private readonly float damping = 10.0f; //플레이어를 향해 회전할 속도
    public bool isFire = false;

    [Header("Reload")]
    [SerializeField] private WaitForSeconds reloadWs;   //startcorutine에서 시간 정할 변수
    [SerializeField] private AudioClip reloadClip;
    [SerializeField] private readonly int maxBullet = 10;    //10발일 때 재장전을 하기 위한 max값
    [SerializeField] private float reloadTime = 2.0f;   //재장전하려면 2초걸림
    [SerializeField] private int curBullet = 0; //현재 총알 수
    [SerializeField] private bool isReload = false;

    void Start()
    {
        firePos = transform.GetChild(3).GetChild(0).GetChild(0).transform;
        ani = GetComponent<Animator>();
        enemyTr = GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player").transform;
        fireClip = Resources.Load<AudioClip>("Sounds/p_m4_1") as AudioClip;
        curBullet = maxBullet;
        reloadWs = new WaitForSeconds(reloadTime);
    }

    void Update()
    {
        if (isFire)
        {
            if (Time.time >= nextFireTime)
            {
                Fire();
                nextFireTime = Time.time + fireInterval + Random.Range(0.0f, 0.3f);
            }

            Quaternion rot = Quaternion.LookRotation(playerTr.position - enemyTr.position);
            // == vector3 PlayerNormal = platerTr.position - enemyTr.position;

            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, damping * Time.deltaTime);
        }
    }

    void Fire()
    {
        var E_bullet = ObjectPoolingManager.poolingManager.E_GetBulletPool();

        E_bullet.transform.position = firePos.position;
        E_bullet.transform.rotation = firePos.rotation;

        E_bullet.gameObject.SetActive(true);

        ani.SetTrigger(hashFire);
        SoundManager.S_instance.PlaySound(firePos.position, fireClip);
    }
}
