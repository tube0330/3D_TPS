using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCtrl : MonoBehaviour
{
    [SerializeField] private Transform firePos;
    [SerializeField] private AudioSource AudioSource;
    [SerializeField] private AudioClip AudioClip;
    [SerializeField] private ParticleSystem MuzzleFlash;
    private readonly string EnemyTag ="ENEMY";  //레이캐스트를 위해 선언

    void Start()
    {
        AudioSource = GetComponent<AudioSource>();
        AudioClip = Resources.Load("Sounds/p_ak_1") as AudioClip;
        MuzzleFlash.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        //raycast
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
        #region projectile movement 방식
        /* #region not object pooling
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
        #endregion */
        #endregion

        //광선이 오브젝트에 맞으면 충돌지점이나 거리 등을 알려주는 구조
        RaycastHit hit;

        //광선을 쏘았을 때 맞았는지 여부
        if (Physics.Raycast(firePos.position, firePos.forward, out hit, 15f))
        {
            if(hit.collider.CompareTag(EnemyTag) || hit.collider.CompareTag("WALL"))
            {
                Debug.Log("맞음");
                object[] _params = new object[2];
                _params[0] = hit.point; //맞은 위치 전달
                _params[1] = 25f;       //데미지 값 전달

                //광선에 맞은 오브젝트의 함수를 호출하면서 매개변수 값을 전달
                hit.collider.SendMessage("OnDamage", _params, SendMessageOptions.DontRequireReceiver);
            }
        }
        AudioSource.PlayOneShot(AudioClip, 1f);
    }
}
