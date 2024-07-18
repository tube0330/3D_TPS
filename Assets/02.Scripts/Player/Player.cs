using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAnimation
{
    public AnimationClip idle;
    public AnimationClip runForward;
    public AnimationClip runBackward;
    public AnimationClip runLeft;
    public AnimationClip runRight;
    public AnimationClip Sprint;
}
public class Player : MonoBehaviour
{
    public PlayerAnimation playerAnimation;

    [SerializeField] Transform tr;
    [SerializeField] Animation ani;
    [SerializeField] Rigidbody rb;
    [SerializeField] CapsuleCollider capCol;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip clip;

    [SerializeField] Transform FirePos;
    [SerializeField] GameObject Bullet;

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotSpeed = 90f;

    float h = 0f, v = 0f, r = 0f;

    void Start()
    {
        tr = transform;
        rb = GetComponent<Rigidbody>();
        ani = GetComponent<Animation>();
        capCol = GetComponent<CapsuleCollider>();

        ani.Play(playerAnimation.idle.name);
        /*ani.clip = playerAnimation.idle;
        ani.Play(playerAnimation.idle.name);*/

        FirePos = GameObject.Find("FirePos").GetComponent<Transform>();
    }

    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        r = Input.GetAxis("Mouse X");

        Vector3 moveDir = (h * Vector3.right) + (v * Vector3.forward);
        tr.Translate(moveDir.normalized * moveSpeed * Time.deltaTime, Space.Self);

        {
            MoveAni();
        }

        tr.Rotate(Vector3.up * r * Time.deltaTime * rotSpeed);

        Sprint();
    }

    private void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            moveSpeed = 10f;
            ani.CrossFade(playerAnimation.Sprint.name, 3.0f);
        }

        else if (Input.GetKeyUp(KeyCode.LeftShift))
            moveSpeed = 5f;
    }

    private void MoveAni()
    {
        if (h > 0.1f)
            ani.CrossFade(playerAnimation.runRight.name, 0.3f);
        else if (h < -0.1f)
            ani.CrossFade(playerAnimation.runLeft.name, 0.3f);

        else if (v > 0.1f)
            ani.CrossFade(playerAnimation.runForward.name, 0.3f);

        else if (v < -0.1f)
            ani.CrossFade(playerAnimation.runBackward.name, 0.3f);

        else
            ani.CrossFade(playerAnimation.idle.name, 0.3f);
    }
}
