using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCtrl : MonoBehaviour
{
    [SerializeField] private Transform firePos;
    [SerializeField] private AudioSource AudioSource;
    [SerializeField] private AudioClip AudioClip;
    [SerializeField] private ParticleSystem MuzzleFlash;
    void Start()
    {
        AudioSource = GetComponent<AudioSource>();
        AudioClip = Resources.Load("Sounds/p_ak_1") as AudioClip;
        MuzzleFlash.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(firePos.position, firePos.forward * 100f, Color.red);
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
            MuzzleFlash.Play();
        }

        else if (Input.GetMouseButtonUp(0))
            MuzzleFlash.Stop();
    }

    void Fire()
    {
        #region
        //Instantiate(bullet, firePos.position, firePos.rotation);
        #endregion

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W)) return;

        #region object pooling
        var _bullet = ObjectPoolingManager.poolingManager.GetBulletPool();

        if (_bullet != null)
        {
            _bullet.transform.position = firePos.position;
            _bullet.transform.rotation = firePos.rotation;
            _bullet.SetActive(true);
        }

        AudioSource.PlayOneShot(AudioClip, 1f);


        #endregion
    }
}
