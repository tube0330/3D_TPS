using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEditor;
using UnityEngine;

public class SwatFire : MonoBehaviour
{
    [SerializeField] private Transform playerTr;
    /* 플레이어가 있는 위치와 방향으로 총을 쏴야 하기 때문에 플레이어와 enemy의 transform을 가져온다.  */
    [SerializeField] private Transform swatTr;
    [SerializeField] private Transform firePos;
    [SerializeField] private AudioClip fireClip;
    [SerializeField] private Animator ani;

    private readonly int hashFire = Animator.StringToHash("Fire");
    private readonly int hashReload = Animator.StringToHash("Reload");

    private float nextFireTime = 0.0f;
    private readonly float fireInterval = 0.1f;
    private readonly float damping = 10.0f;
    public bool isFire = false;

    [Header("Reload")]
    [SerializeField] private WaitForSeconds reloadWs;
    [SerializeField] private AudioClip reloadClip;
    [SerializeField] private readonly int maxBullet = 10;
    [SerializeField] private float reloadTime = 2.0f;
    [SerializeField] private int curBullet = 0;
    [SerializeField] private bool isReload = false;

    public MeshRenderer S_MuzzleFlash;
    private readonly string PlayerTag = "Player";

    void Start()
    {
        playerTr = GameObject.FindWithTag(PlayerTag).transform;
        swatTr = GetComponent<Transform>();
        firePos = transform.GetChild(2).GetChild(0).GetChild(0).transform;
        ani = GetComponent<Animator>();
        fireClip = Resources.Load<AudioClip>("Sounds/p_m4_1");
        reloadWs = new WaitForSeconds(reloadTime);
        reloadClip = Resources.Load<AudioClip>("Sounds/p_reload");
        S_MuzzleFlash = transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetComponent<MeshRenderer>();
        S_MuzzleFlash.enabled = false;
        curBullet = maxBullet;
    }

    void Update()
    {
        if(GameManager.G_Instance.isGameOver) return;
        
        if (isFire && !isReload)
        {
            if (Time.time >= nextFireTime)
            {
                Fire();
                nextFireTime = Time.time + fireInterval + Random.Range(0.0f, 0.3f);
            }

            Quaternion rot = Quaternion.LookRotation(playerTr.position - swatTr.position);

            swatTr.rotation = Quaternion.Slerp(swatTr.rotation, rot, damping * Time.deltaTime);
        }
    }

    void Fire()
    {
        /* var E_bullet = ObjectPoolingManager.poolingManager.E_GetBulletPool();

        if (E_bullet != null)
        {
            E_bullet.transform.position = firePos.position;
            E_bullet.transform.rotation = firePos.rotation;

            E_bullet.gameObject.SetActive(true);

            ani.SetTrigger(hashFire);
            SoundManager.S_instance.PlaySound(firePos.position, fireClip);
        } */

        isReload = (--curBullet % maxBullet) == 0;

        if (isReload)
        {
            StartCoroutine(Reloading());
        }

        StartCoroutine(ShowMuzzleFlash());

        RaycastHit hit;
        if (Physics.Raycast(firePos.position, firePos.forward, out hit, 15f))
        {
            if(hit.collider.CompareTag(PlayerTag))
            {
                object[] obj = new object[2];
                obj[0] = hit.point;
                obj[1] = 1f;

                hit.collider.SendMessage("playerDamage", obj, SendMessageOptions.DontRequireReceiver);
            }
        }
        SoundManager.S_instance.PlaySound(firePos.position, fireClip);
    }

    IEnumerator Reloading()
    {
        ani.SetTrigger(hashReload);
        SoundManager.S_instance.PlaySound(transform.position, reloadClip);

        yield return reloadWs;

        curBullet = maxBullet;
        isReload = false;
    }

    IEnumerator ShowMuzzleFlash()
    {
        S_MuzzleFlash.enabled = true;

        S_MuzzleFlash.transform.localScale = Vector3.one * Random.Range(1.5f, 2.3f);
        Quaternion rot = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        S_MuzzleFlash.transform.localRotation = rot;

        yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));

        S_MuzzleFlash.enabled = false;
    }
}
