using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{

    [SerializeField] private Texture[] textures;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject Effect;
    [SerializeField] private int hitCount = 0;
    //[SerializeField] private CamerShake camshake;
    //[SerializeField] private Barrel bal;

    private readonly string bulletTag = "BULLET";

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        textures = Resources.LoadAll<Texture>("BarrelTextures");
        meshRenderer.material.mainTexture = textures[Random.Range(0, textures.Length)];
        Effect = Resources.Load("BigExplosionEffect") as GameObject;
        //camshake = GetComponent<CamerShake>();
        //bal = GetComponent<Barrel>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag(bulletTag))
        {
            if (++hitCount == 5)
            {
                ExplosionBarrel();
            }

        }
    }

    void ExplosionBarrel()
    {
        GameObject eff = Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(eff, 2.0f);

        Collider[] cols = Physics.OverlapSphere(transform.position, 20f, 1 << 7);
        foreach (Collider col in cols)
        {
            Rigidbody rigidbody = col.GetComponent<Rigidbody>();

            if (rigidbody != null)
            {
                rigidbody.mass = 1.0f;

                rigidbody.AddExplosionForce(1000, transform.position, 20f, 1200f);
                SoundManager.S_instance.PlaySound(transform.position, SoundManager.S_instance.clip2);
            }
            //camshake.TurnOn();
            Invoke("BarrelMassChange", 3.0f);
        }
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
