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

    //private readonly string bulletTag = "BULLET";

    void Start()
    {
        C_swatAI = GetComponent<SwatAI>();
        ani = GetComponent<Animator>();
        BloodEff = Resources.Load<GameObject>("Effects/BulletImpactFleshBigEffect");
        HP = MaxHP;
    }

    void OnDamage(object[] obj)
    {
        ShowBloodEffect((Vector3)obj[0]);
        
        Debug.Log("Swat이 맞음");
        HP -= (float)obj[1];
        Mathf.Clamp(HP, 0, MaxHP);

        if (HP <= 0f)
            SwatDie();
    }

    void SwatDie()
    {
        C_swatAI.state = SwatAI.State.DIE;
    }

    void ShowBloodEffect(Vector3 col)
    {
        Vector3 pos = col;  //위치
        Vector3 nor = col.normalized;   //방향

        Quaternion rot = Quaternion.FromToRotation(Vector3.forward, nor);
        GameObject Blood = Instantiate(BloodEff, pos, rot);
        Destroy(Blood, 1.0f);
    }
}
