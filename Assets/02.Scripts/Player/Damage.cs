using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Damage : MonoBehaviour
{
    //private readonly string E_bulletTag = "E_BULLET";
    private float HP = 0f;
    private readonly float MaxHP = 100;
    public GameObject bloodEffect;

    void Start()
    {
        HP = MaxHP;
        bloodEffect = Resources.Load<GameObject>("Effects/BulletImpactFleshBigEffect");
    }

    #region projectiling 방식
    /* void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(E_bulletTag))
        {
            col.gameObject.SetActive(false);

            Vector3 pos = col.contacts[0].point;
            Vector3 _normal = col.contacts[0].normal;
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);
            GameObject Blood = Instantiate(bloodEffect, pos, rot);
            Destroy(Blood, 1.0f);
        }
    } */
    #endregion

    void playerDamage(object[] obj)
    {
        HP -= (float)obj[1];
        Mathf.Clamp(HP, 0, MaxHP);

        if (HP <= 0f)
            EnemyAI_Ani.E_instance.state = EnemyAI_Ani.State.PLAYERDIE;


    }
}
