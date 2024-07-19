using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using Codice.Client.BaseCommands.TubeClient;
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

    public MeshRenderer E_MuzzleFlash;

    void Start()
    {
        firePos = transform.GetChild(3).GetChild(0).GetChild(0).transform;
        ani = GetComponent<Animator>();
        enemyTr = GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player").transform;
        fireClip = Resources.Load<AudioClip>("Sounds/p_m4_1");
        curBullet = maxBullet;
        reloadWs = new WaitForSeconds(reloadTime);
        reloadClip = Resources.Load<AudioClip>("Sounds/p_reload");
        E_MuzzleFlash = transform.GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetComponent<MeshRenderer>();
        E_MuzzleFlash.enabled = false;  //mesh Renderer 안보이게
    }

    void Update()
    {
        if (isFire && !isReload)
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

        isReload = (--curBullet % maxBullet) == 0;  //0되는 순간 true됨

        if (isReload)
        {
            StartCoroutine(Reloading());
        }

        StartCoroutine(ShowMuzzleFlash());
    }

    IEnumerator Reloading()
    {
        ani.SetTrigger(hashReload);
        SoundManager.S_instance.PlaySound(transform.position, reloadClip);

        yield return reloadWs;  //재장전 시간만큼 대기하는 동안 제어권 양보

        curBullet = maxBullet;
        isReload = false;
    }

    IEnumerator ShowMuzzleFlash()
    {
        #region
        /* E_MuzzleFlash.enabled = true;
        Vector2 offset = new Vector3(Random.Range(0, 2f), Random.Range(0f, 2f)) * 0.5f;
        E_MuzzleFlash.transform.localScale = Vector3.one * offset;

        yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
        E_MuzzleFlash.enabled = false; */
        #endregion

        E_MuzzleFlash.enabled = true;

        E_MuzzleFlash.transform.localScale = Vector3.one * Random.Range(1.5f, 2.3f);
        Quaternion rot = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        E_MuzzleFlash.transform.localRotation = rot;

        yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));

        E_MuzzleFlash.enabled = false;
    }
}
