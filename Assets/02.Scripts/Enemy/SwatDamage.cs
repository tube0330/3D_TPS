using System;
using Unity.Properties;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class SwatDamage : MonoBehaviour
{
    [SerializeField] private SwatAI C_swatAI;
    [SerializeField] private Animator ani;
    [SerializeField] private GameObject BloodEff;

    [SerializeField] public float HP = 0f;
    [SerializeField] private float MaxHP = 100f;
    [SerializeField] private Image HPBar;
    [SerializeField] private Text HPtxt;

    //private readonly string bulletTag = "BULLET";

    void Start()
    {
        C_swatAI = GetComponent<SwatAI>();
        ani = GetComponent<Animator>();
        BloodEff = Resources.Load<GameObject>("Effects/BulletImpactFleshBigEffect");
        HP = MaxHP;
        HPBar = transform.GetChild(3).GetChild(1).GetComponent<Image>();
        HPtxt = transform.GetChild(3).GetChild(2).GetComponent<Text>();
    }

    void OnDamage(object[] obj)
    {
        ShowBloodEffect((Vector3)obj[0]);

        HP -= (float)obj[1];
        HP = Mathf.Clamp(HP, 0, 100f);
        HPBar.fillAmount = HP / MaxHP;
        HPtxt.text = $"HP {HP}";

        if (HPBar.fillAmount <= 0.3f)
            HPBar.color = Color.red;
        else if (HPBar.fillAmount <= 0.5f)
            HPBar.color = Color.yellow;
        else if (HPBar.fillAmount <= 1f)
            HPBar.color = Color.green;

        //Mathf.Clamp(HP, 0, MaxHP);
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
