using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Damage : MonoBehaviour
{
    //private readonly string E_bulletTag = "E_BULLET";
    public GameObject bloodEffect;
    public float HP = 0;
    public float MaxHP = 100;
    private string enemyTag = "ENEMY";
    private string swatTag = "SWAT";
    [SerializeField] private Image BloodScreen;
    [SerializeField] private Image Img_HPBar;

    void Start()
    {
        HP = MaxHP;
        bloodEffect = Resources.Load<GameObject>("Effects/BulletImpactFleshBigEffect");
        BloodScreen = GameObject.Find("Canvas_UI").transform.GetChild(0).GetComponent<Image>();
        Img_HPBar = GameObject.Find("Canvas_UI").transform.GetChild(2).GetChild(2).GetComponent<Image>();
        Img_HPBar.color = Color.green;
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
        HP -= (float)obj[1];
        Mathf.Clamp(HP, 0, MaxHP);

        Img_HPBar.fillAmount = (HP/MaxHP);

        if (HP <= 0f && Img_HPBar.fillAmount==0)
            PlayerDie();

        StartCoroutine(ShowBloodScreen());
    }

    IEnumerator ShowBloodScreen()
    {
        BloodScreen.color = new Color(1, 0, 0, Random.Range(0.25f, 0.35f));
        yield return new WaitForSeconds(0.1f);
        BloodScreen.color  =Color.clear;    //텍스처의 색깔을 전부 0
    }

    public void PlayerDie()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        GameObject[] swates = GameObject.FindGameObjectsWithTag(swatTag);

        for (int i = 0; i < enemies.Length; i++)
            enemies[i].gameObject.SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);

        for (int i = 0; i < swates.Length; i++)
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