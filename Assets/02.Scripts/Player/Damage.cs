using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    private readonly string E_bulletTag = "E_BULLET";
    public GameObject bloodEffect;

    void Start()
    {
        bloodEffect = Resources.Load<GameObject>("Effects/BulletImpactFleshBigEffect");
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag(E_bulletTag))
        {
            col.gameObject.SetActive(false);

            Vector3 pos = col.contacts[0].point;
            Vector3 _normal = col.contacts[0].normal;
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);
            GameObject Blood = Instantiate(bloodEffect, pos, rot);
            Destroy(Blood, 1.0f);
        }
    }
}
