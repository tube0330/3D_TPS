using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

/* [System.Serializable]
public class PlayerAnimation
{
    public AnimationClip idle;
    public AnimationClip runForward;
    public AnimationClip runBackward;
    public AnimationClip runLeft;
    public AnimationClip runRight;
    public AnimationClip Sprint;
} */
public class Player : MonoBehaviourPun
{
    //public PlayerAnimation playerAnimation;

    [SerializeField] Transform tr;
    [SerializeField] Animator ani;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip clip;
    [SerializeField] CapsuleCollider col;

    [SerializeField] Transform FirePos;
    [SerializeField] GameObject Bullet;

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotSpeed = 90f;

    //float h = 0f, v = 0f, r = 0f;

    [Header("PlayerInput")]
    public PlayerInput playerInput;
    public InputActionMap playerMap;
    public InputAction moveAction;
    public InputAction sprintAction;
    Vector3 moveDir = Vector3.zero;

    Vector3 curPos = Vector3.zero;
    Quaternion curRot = Quaternion.identity;

    void Awake()
    {
        photonView.Synchronization = ViewSynchronization.Unreliable;
        photonView.ObservedComponents[0] = this;
    }

    void OnEnable()
    {
        GameManager.OnItemChange += UpdateSetUp;
    }

    void UpdateSetUp()
    {
        moveSpeed = GameManager.G_Instance.gameData.speed;
    }

    void Start()
    {
        tr = transform;
        ani = GetComponent<Animator>();
        moveSpeed = GameManager.G_Instance.gameData.speed;
        col = GetComponent<CapsuleCollider>();

        /* ani.Play(playerAnimation.idle.name);
        ani.clip = playerAnimation.idle;
        ani.Play(playerAnimation.idle.name); */

        FirePos = GameObject.Find("FirePos").GetComponent<Transform>();

        playerInput = GetComponent<PlayerInput>();
        playerMap = playerInput.actions.FindActionMap("Player");
        moveAction = playerInput.actions["Move"];
        sprintAction = playerInput.actions["Sprint"];

        curPos = tr.position;
        curRot = tr.rotation;
    }

    void Update()
    {
        /* h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        r = Input.GetAxis("Mouse X");

        Vector3 moveDir = (h * Vector3.right) + (v * Vector3.forward);
        tr.Translate(moveDir.normalized * moveSpeed * Time.deltaTime, Space.Self);
        tr.Rotate(Vector3.up * r * Time.deltaTime * rotSpeed);

        MoveAni();
        Sprint(); */

        if (photonView.IsMine)
        {
            Vector2 dir = moveAction.ReadValue<Vector2>();
            moveDir = new Vector3(dir.x, 0, dir.y).normalized; // 방향 벡터 정규화

            if (moveDir != Vector3.zero)
            {
                ani.SetBool("move", true);
                tr.Translate(moveDir * moveSpeed * Time.deltaTime);
                tr.rotation = Quaternion.Slerp(tr.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime * rotSpeed);
            }

            else
                ani.SetBool("move", false);

            //MoveAni();
            Sprint();
        }

        else
        {
            tr.position = Vector3.Lerp(tr.position, curPos, Time.deltaTime * 10f);
            tr.rotation = Quaternion.Slerp(tr.rotation, curRot, Time.deltaTime * 10f);
        }
    }

    private void Sprint()
    {
        ani.SetBool("move", true);
        bool isSprinting = sprintAction.ReadValue<float>() > 0; // 스프린트 액션의 상태를 읽어옴

        if (isSprinting && moveDir != Vector3.zero)
        {
            moveSpeed = 10f;
            ani.SetFloat("moveSpeed", moveSpeed);
        }

        else
        {
            moveSpeed = 5f;
            ani.SetFloat("moveSpeed", moveSpeed);
        }
    }

    /* private void MoveAni()
    {
        if (moveDir.z > 0.1f)
            ani.CrossFade(playerAnimation.runForward.name, 0.3f);
        else if (moveDir.z < -0.1f)
            ani.CrossFade(playerAnimation.runBackward.name, 0.3f);

        if (moveDir.x > 0.1f)
            ani.CrossFade(playerAnimation.runRight.name, 0.3f);
        else if (moveDir.x < -0.1f)
            ani.CrossFade(playerAnimation.runLeft.name, 0.3f);

        if (moveDir == Vector3.zero)
            ani.CrossFade(playerAnimation.idle.name, 0.3f);
    } */

    public void OnPhotonSerializeView(PhotonStream stream, bool isLocal)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
        }

        else
        {
            curPos = (Vector3)stream.ReceiveNext();
            curRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
