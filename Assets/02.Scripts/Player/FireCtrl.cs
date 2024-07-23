using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public struct PlayerSound
{
    public AudioClip[] fire;
    public AudioClip[] reload;
}
public class FireCtrl: MonoBehaviour 
{
    public enum weaponType
    {
        RIFLE=0,SHOTGUN=1
    }
    public weaponType curtype = weaponType.SHOTGUN;
    public PlayerSound playerSound;
    [SerializeField] float firetime;
    public Transform firePosTr;
    [SerializeField] GameObject bulletprfab;
    [SerializeField] AudioClip fireclip;
    [SerializeField] AudioSource Source;
    [SerializeField] Player Player;

    [SerializeField] private ParticleSystem muzzleFlash;
    private readonly string enemyTag = "Enemy";
    private readonly string WallTag = "Wall";
    private readonly string BarrelTag = "Barrel";
    private const float DIST = 20f;

    public Image magazineImage;
    public Text magazineTxt;
    public float reloadTime = 2.0f;
    public bool isReload = false;
    public int maxBullet = 10;
    public int curBullet = 10;



    void Start()
    {
        firetime = Time.time;
        fireclip = Resources.Load("Sounds/p_ak_1_1") as AudioClip;
        Player =GetComponent<Player>();
        muzzleFlash.Stop();
        magazineImage = GameObject.Find("Canvas").transform.GetChild(1).GetChild(2).GetComponent<Image>();
        magazineTxt = GameObject.Find("Canvas").transform.GetChild(1).GetChild(0).GetComponent<Text>();
        
    }
    void Update()
    {
        Debug.DrawRay(firePosTr.position, firePosTr.forward*100f,Color.red);
        if (Input.GetMouseButtonDown(0)&&!isReload)
        {
            if ( !isReload)
            {
                --curBullet;
                
                Fire();
                muzzleFlash.Play();
               
                if (curBullet == 0)
                {
                    StartCoroutine(Reloading());
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            muzzleFlash.Stop();
        }
        else
            muzzleFlash.Stop();        
    }
    IEnumerator Reloading()
    {
        isReload = true;
        #region 오류
        /* SoundManager.S_instance.PlaySound(transform.position, playerSound.reload([int]curtype)); */
        #endregion


        yield return new WaitForSeconds(playerSound.reload[(int)curtype].length+0.3f);
        
        curBullet = maxBullet;
        isReload = false;
        magazineTxt.text = $"<color=#64FFFF>{curBullet}</color>/{maxBullet}";
        magazineImage.fillAmount = curBullet * 0.1f;
    }
    

    private void Fire()
    {

        RaycastHit hit;//광선이 오브젝트에 맞으면 충돌지점이나 거리들을 알려주는 광선 구조체
                       //광선을 쏘았을 때 맞았는지 여부를 측정
        if (Physics.Raycast(firePosTr.position, firePosTr.forward, out hit, DIST))
        {
            if (hit.collider.CompareTag(enemyTag))
            {

                object[] _params = new object[2];
                _params[0] = hit.point;//첫번째 배열에는 맞은 위치를 전달
                _params[1] = 25f;// 데미지 값을 전달
                Debug.Log($"{_params[0]}   ");
                hit.collider.gameObject.SendMessage("OnDamage", _params, SendMessageOptions.DontRequireReceiver);
                //광선에 맞은 오브젝트의 함수를 호출하면서 매개변수 값을 전달
            }
            if (hit.collider.CompareTag(WallTag))
            {

                object[] _params = new object[2];
                _params[0] = hit.point;//첫번째 배열에는 맞은 위치를 전달

                Debug.Log($"{_params[0]}   ");
                hit.collider.gameObject.SendMessage("OnDamage", _params, SendMessageOptions.DontRequireReceiver);
                //광선에 맞은 오브젝트의 함수를 호출하면서 매개변수 값을 전달
            }
            if (hit.collider.CompareTag(BarrelTag))
            {

                object[] _params = new object[2];
                _params[0] = firePosTr.position;//발사위치
                _params[1] = hit.point;//맞은위치
                Debug.Log($"{_params[0]}   ");
                hit.collider.gameObject.SendMessage("OnDamage", _params, SendMessageOptions.DontRequireReceiver);
                //광선에 맞은 오브젝트의 함수를 호출하면서 매개변수 값을 전달
            }
        }
        Source.PlayOneShot(fireclip, 1.0f);
        UpdateBulletTxt();



        #region Projectile Movement Method

        ////오브젝트 풀링이 아닐때
        ////Instantiate(bulletprfab, firePosTr.position, firePosTr.rotation);
        ////오브젝트 풀링일 때
        //var _bullet = ObjectPoolingManager.poolingManager.GetBulletPool();
        //if (_bullet != null)
        //{
        //    _bullet.transform.position = firePosTr.position;
        //    _bullet.transform.rotation = firePosTr.rotation;
        //    _bullet.SetActive(true);
        //    if (_bullet)
        //        muzzleFlash.Play();
        //}
        #endregion


    }

    private void UpdateBulletTxt()
    {
        magazineImage.fillAmount = curBullet * 0.1f;
        magazineTxt.text = $"<color=#64FFFF>{curBullet}</color>/{maxBullet}";
    }
}