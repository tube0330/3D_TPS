using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private GameObject BloodEff;

    private float E_HP = 0;
    private float E_MaxHP = 100;

    void Start()
    {
        BloodEff = Resources.Load<GameObject>("Effects/BulletImpactFleshBigEffect");
        E_HP = E_MaxHP;
    }
    #region projectile 방식과 충돌 감지 isTrigger 체크된 경우 OnTriggerEnter
    /* void OnCollisionEnter(Collision col)
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
    } */
    #endregion

    void OnDamage(object[] _params)
    {
        ShowBloodEffect((Vector3)_params[0]);

        E_HP -= (float)_params[1];
        E_HP = Mathf.Clamp(E_HP, 0, 100f);

        if (E_HP <= 0f)
            E_Die();
    }

    void E_Die()
    {
        Debug.Log("사망");
        GetComponent<EnemyAI_Ani>().state = EnemyAI_Ani.State.DIE;
    }

    private void ShowBloodEffect(Vector3 col)
    {
        /* Vector3 pos = col.contacts[0].point;    //Collision 구조체 안에 있는 contacts라는 배열에 맞은 위치를 넘김
        Vector3 _normal = col.contacts[0].normal;   //                  "                         방향     "
        Quaternion rot = Quaternion.FromToRotation(Vector3.forward, _normal);
        GameObject blood = Instantiate(BloodEff, pos, rot);
        Destroy(blood, 1.0f); */

        Vector3 pos = col;  //위치
        Vector3 nor = col.normalized;   //방향
        Quaternion rot = Quaternion.FromToRotation(Vector3.forward, nor);
        GameObject blood = Instantiate(BloodEff, pos, rot);
        Destroy(blood, 1.0f);
    }
}
