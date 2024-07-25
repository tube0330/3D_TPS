using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{

    [SerializeField] private Texture[] textures;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private GameObject Effect;
    [SerializeField] private int hitCount = 0;
    [SerializeField] private CamerShake camshake;
    [SerializeField] private MeshFilter meshFilter; //barrel 찌그러진거 표현하려고
    [SerializeField] private Mesh[] meshes;         //barrel 찌그러진거 표현하려고
    //[SerializeField] private Barrel bal;
    //private readonly string bulletTag = "BULLET";

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        textures = Resources.LoadAll<Texture>("BarrelTextures");
        meshRenderer.material.mainTexture = textures[Random.Range(0, textures.Length)];
        Effect = Resources.Load("Effects/BigExplosionEffect") as GameObject;
        camshake = GameObject.Find("Main Camera").GetComponent<CamerShake>();
        meshFilter = GetComponent<MeshFilter>();
        meshes = Resources.LoadAll<Mesh>("Meshes");
    }

    #region
    //프로젝타일 방식의 충돌감지 
    /* void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(bulletTag))
        {
            if (++hitCount == 5)
            {
                ExplosionBarrel();
            }
        }
    } */

    void OnDamage(object[] _params)
    {
        Vector3 firePos = (Vector3)_params[0];  //발사 위치 받아옴
        Vector3 hitPos = (Vector3)_params[1];   //맞은 위치 받아옴

        /* 맞은 좌표에서 발사위치 = 맞은위치 - 발사위치 => 거리 + 방향
        * Ray의 각도 구하기위해
        * 전문 용어로는 입사벡터
        */
        Vector3 incomeVector = hitPos - firePos;  //입사벡터를 정규화벡터로 변경

        /* Ray의 hit좌표에 입사벡터의 각도로 힘 생성
         * 어떤 지점에 힘을 모아 물리를 생성할 때 호출되는 메서드
         */
        GetComponent<Rigidbody>().AddForceAtPosition(incomeVector * 1500f, hitPos); //hitPos에서 incomeVector방향으로

        if (++hitCount == 5)
            ExplosionBarrel();
    }
    #endregion

    void ExplosionBarrel()
    {
        GameObject eff = Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(eff, 2.0f);

        Collider[] cols = Physics.OverlapSphere(transform.position, 20f, 1 << 7 | 1 << 8 | 1 << 10);
        foreach (Collider col in cols)
        {
            Rigidbody rigidbody = col.GetComponent<Rigidbody>();

            if (rigidbody != null)
            {
                rigidbody.mass = 1.0f;

                rigidbody.AddExplosionForce(1000, transform.position, 20f, 1200f);
                col.gameObject.SendMessage("E_Die", SendMessageOptions.DontRequireReceiver);
                col.gameObject.SendMessage("SwatDie", SendMessageOptions.DontRequireReceiver);
            }
            camshake.TurnOn();
            Invoke("BarrelMassChange", 3.0f);
        }

        SoundManager.S_instance.PlaySound(transform.position, SoundManager.S_instance.clip2);

        //찌그러진 매시 두 개 중 하나 골라와 적용
        int idx = Random.Range(0, meshes.Length);
        meshFilter.sharedMesh = meshes[idx];
        GetComponent<MeshCollider>().sharedMesh = meshes[idx];
    }

    void BarrelMassChange()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, 20f, 1 << 7);

        foreach (Collider col in cols)
        {
            Rigidbody rigidbody = col.GetComponent<Rigidbody>();

            if (rigidbody != null)
            {
                rigidbody.mass = 60.0f;
            }
        }
    }
}
