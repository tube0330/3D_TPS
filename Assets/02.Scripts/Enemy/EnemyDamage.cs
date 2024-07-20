using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private readonly string bulletTag = "BULLET";
    [SerializeField] private GameObject BloodEff;

    private float E_HP = 0;
    private float E_MaxHP = 100;
    
    void Start()
    {
        BloodEff = Resources.Load<GameObject>("Effects/BulletImpactFleshBigEffect");
        E_HP = E_MaxHP;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(bulletTag))
        {
            col.gameObject.SetActive(false);
            ShowBloodEffect(col);
            E_HP -= col.gameObject.GetComponent<BulletCtrl>().damage;
            E_HP = Mathf.Clamp(E_HP, 0, 100f);

            if (E_HP <= 0f)
                E_Die();
        }
    }
    void E_Die()
    {
        Debug.Log("사망");
        GetComponent<EnemyAI_Ani>().state = EnemyAI_Ani.State.DIE;
    }

    private void ShowBloodEffect(Collision col)
    {
        Vector3 pos = col.contacts[0].point;    //Collision 구조체 안에 있는 contacts라는 배열에 맞은 위치를 넘김
        Vector3 _normal = col.contacts[0].normal;   //                  "                         방향     "
        Quaternion rot = Quaternion.FromToRotation(Vector3.forward, _normal);
        GameObject blood = Instantiate(BloodEff, pos, rot);
        Destroy(blood, 1.0f);
    }
}
