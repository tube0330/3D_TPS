using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public struct PlayerSound
{
    public AudioClip[] fire;
    public AudioClip[] reload;
}
public class FireCtrl : MonoBehaviour
{
    public enum weaponType
    {
        RIFLE = 0, SHOTGUN = 1
    }
    public weaponType curWeaponType = weaponType.SHOTGUN;
    public PlayerSound playerSound;

    [SerializeField] float firetime;
    [SerializeField] private Transform firePos;
    //[SerializeField] AudioClip fireclip;
    //[SerializeField] AudioSource Source;
    [SerializeField] Player Player;
    [SerializeField] private ParticleSystem muzzleFlash;
    private readonly string enemyTag = "ENEMY";
    private readonly string swatTag = "SWAT";
    private readonly string WallTag = "WALL";
    private readonly string BarrelTag = "BARREL";
    private const float DIST = 20f;

    public Image magazineImage;
    public Text magazineTxt;
    public float reloadTime = 2.0f;
    public bool isReload = false;
    public int maxBullet = 10;
    public int curBullet = 10;

    public Sprite[] weaponIcon;
    public Image weaponImg;



    void Start()
    {
        firetime = Time.time;
        //fireclip = Resources.Load("Sounds/p_ak_1") as AudioClip;
        Player = GetComponent<Player>();
        muzzleFlash.Stop();
        magazineImage = GameObject.Find("Canvas_UI").transform.GetChild(1).GetChild(2).GetComponent<Image>();
        magazineTxt = GameObject.Find("Canvas_UI").transform.GetChild(1).GetChild(0).GetComponent<Text>();
        weaponIcon = Resources.LoadAll<Sprite>("WeaponIcons");
        weaponImg = GameObject.Find("Canvas_UI").transform.GetChild(3).GetChild(0).GetComponent<Image>();

    }
    void Update()
    {
        Debug.DrawRay(firePos.position, firePos.forward * 25f, Color.green);    //광선 그리기
        
        if (EventSystem.current.IsPointerOverGameObject()) return;   //UI에 특정 이벤트가 발생되면 빠져나감

        if (Input.GetMouseButtonDown(0) && !isReload)
        {
            if (!isReload)
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

    private void UpdateBulletTxt()
    {
        magazineImage.fillAmount = (float)curBullet / (float)maxBullet;
        magazineTxt.text = $"<color=#64FFFF>{curBullet}</color>/{maxBullet}";
    }

    public void OnChangeWeapon()
    {
        curWeaponType = (weaponType)((int)++curWeaponType % 2);
        weaponImg.sprite = weaponIcon[(int)curWeaponType];  //스프라이트 이미지
    }

    IEnumerator Reloading()
    {
        isReload = true;
        SoundManager.S_instance.PlaySound(transform.position, playerSound.reload[(int)curWeaponType]);
        yield return new WaitForSeconds(playerSound.reload[(int)curWeaponType].length + 0.0f);

        curBullet = maxBullet;
        magazineImage.fillAmount = 1.0f;
        UpdateBulletTxt();
        isReload = false;
    }


    private void Fire()
    {

        RaycastHit hit;//광선이 오브젝트에 맞으면 충돌지점이나 거리들을 알려주는 광선 구조체
                       //광선을 쏘았을 때 맞았는지 여부를 측정
        if (Physics.Raycast(firePos.position, firePos.forward, out hit, DIST))
        {
            if (hit.collider.CompareTag(enemyTag))
            {
                object[] _params = new object[2];
                _params[0] = hit.point;//첫번째 배열에는 맞은 위치를 전달
                _params[1] = 25f;// 데미지 값을 전달
                hit.collider.SendMessage("OnDamage", _params, SendMessageOptions.DontRequireReceiver);
                //광선에 맞은 오브젝트의 함수를 호출하면서 매개변수 값을 전달
            }
            if (hit.collider.CompareTag(swatTag))
            {
                object[] _params = new object[2];
                _params[0] = hit.point;//첫번째 배열에는 맞은 위치를 전달
                _params[1] = 25f;// 데미지 값을 전달
                hit.collider.SendMessage("OnDamage", _params, SendMessageOptions.DontRequireReceiver);
                //광선에 맞은 오브젝트의 함수를 호출하면서 매개변수 값을 전달
            }
            if (hit.collider.CompareTag(WallTag))
            {
                object[] _params = new object[2];
                _params[0] = hit.point;//첫번째 배열에는 맞은 위치를 전달

                hit.collider.SendMessage("OnDamage", _params, SendMessageOptions.DontRequireReceiver);
                //광선에 맞은 오브젝트의 함수를 호출하면서 매개변수 값을 전달
            }
            if (hit.collider.CompareTag(BarrelTag))
            {

                object[] _params = new object[2];
                _params[0] = firePos.position;//발사위치
                _params[1] = hit.point;//맞은위치
                hit.collider.SendMessage("OnDamage", _params, SendMessageOptions.DontRequireReceiver);
                //광선에 맞은 오브젝트의 함수를 호출하면서 매개변수 값을 전달
            }
        }
        //Source.PlayOneShot(fireclip, 1.0f);
        SoundManager.S_instance.PlaySound(transform.position, playerSound.fire[(int)curWeaponType]);

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
}