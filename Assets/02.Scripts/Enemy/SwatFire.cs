using System.Collections;
using System.Collections.Generic;
using log4net.DateFormatter;
using PlasticPipe.PlasticProtocol.Messages;
using UnityEngine;

public class SwatFire : MonoBehaviour
{
    [SerializeField] private Transform swatTr;
    [SerializeField] private Transform playerTr;
    [SerializeField] private Transform firePos;
    [SerializeField] private Animator ani;

    private readonly int hashFire = Animator.StringToHash("Fire");
    private readonly int hashReload = Animator.StringToHash("Reload");

    public bool isFire = false;
    public bool isReload = false;
    private float nextFireTime = 0.0f;
    private float fireInterval = 0.1f;
    private int curBullet = 0;
    private readonly int maxBullet = 10;

    void Start()
    {
        swatTr = transform;
        playerTr = GameObject.FindWithTag("Player").transform;
        firePos = transform.GetChild(2).GetChild(0).GetChild(0).transform;
        ani = GetComponent<Animator>();
        curBullet = maxBullet;
    }

    void Update()
    {
        if (isFire)
        {
            if (Time.time >= nextFireTime)
                Fire();
        }
    }

    void Fire()
    {
        curBullet--;

        var E_bullet = ObjectPoolingManager.poolingManager.E_GetBulletPool();

        if (E_bullet != null)
        {
            E_bullet.SetActive(true);
            ani.SetTrigger(hashFire);
        }

        isReload = curBullet % maxBullet == 0;
        
        if (isReload)
            ani.SetTrigger(hashReload);
    }
}
