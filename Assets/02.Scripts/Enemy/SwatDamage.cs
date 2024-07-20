using System;
using UnityEditorInternal;
using UnityEngine;

public class SwatDamage : MonoBehaviour
{
    [SerializeField] private SwatAI C_swatAI;
    [SerializeField] private Animator ani;
    [SerializeField] private GameObject BloodEff;

    [SerializeField] private float HP = 0f;
    [SerializeField] private float MaxHP = 100f;

    private readonly string bulletTag = "BULLET";

    void Start()
    {
        C_swatAI = GetComponent<SwatAI>();
        ani = GetComponent<Animator>();
        BloodEff = Resources.Load<GameObject>("Effects/BulletImpactFleshBigEffect");
        HP = MaxHP;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(bulletTag))
        {
            col.gameObject.SetActive(false);
            ShowBloodEffect(col);
            HP -= col.gameObject.GetComponent<BulletCtrl>().damage;
            HP = Math.Clamp(HP, 0, MaxHP);

            if (HP <= 0f)
                SwatDie();
        }
    }

    void SwatDie()
    {
        C_swatAI.state = SwatAI.State.DIE;
    }

    void ShowBloodEffect(Collision col)
    {
        Vector3 pos = col.contacts[0].point;
        Vector3 nor = col.contacts[0].normal;

        Quaternion rot = Quaternion.FromToRotation(Vector3.forward, nor);
        GameObject Blood = Instantiate(BloodEff, pos, rot);
        Destroy(Blood, 1.0f);
    }
}
