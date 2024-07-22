using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class Damage : MonoBehaviour
{
    //private readonly string E_bulletTag = "E_BULLET";
    public GameObject bloodEffect;
    public float HP = 0;
    public float MaxHP = 100;
    private string enemyTag = "ENEMY";
    private string swatTag = "SWAT";

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
        ShowBloodEffect((Vector3)obj[0]);

        Debug.Log("맞음");
        HP -= (float)obj[1];
        Mathf.Clamp(HP, 0, MaxHP);

        if (HP <= 0f)
            PlayerDie();
    }

    public void PlayerDie()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        GameObject[] swates = GameObject.FindGameObjectsWithTag(swatTag);

        for(int i = 0; i<enemies.Length; i++)
        enemies[i].gameObject.SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);

        for(int i = 0; i<swates.Length; i++)
        swates[i].gameObject.SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
    }

    private void ShowBloodEffect(Vector3 col)
    {
        Vector3 pos = col;  //위치
        Vector3 nor = col.normalized;   //방향
        Quaternion rot = Quaternion.FromToRotation(Vector3.forward, nor);
        GameObject blood = Instantiate(bloodEffect, pos, rot);
        Destroy(blood, 1.0f);
    }
}