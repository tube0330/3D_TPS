using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviourPunCallbacks
{
    public static ObjectPoolingManager poolingManager;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject E_bulletPrefab;

    private int maxPool = 50;
    public List<GameObject> bulletPoolList;
    public List<GameObject> E_bulletPoolList;

    [Header("EnemyObjectPool")]
    public GameObject EnemyPrefab;
    public GameObject SwatPrefab;
    public List<GameObject> EnemyPoolList;
    public List<GameObject> SwatPoolList;
    public List<Transform> SpawnPointList;

    void Awake()
    {
        if (poolingManager == null)
            poolingManager = this;

        else if (poolingManager != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        bulletPrefab = Resources.Load("Bullet") as GameObject;
        E_bulletPrefab = Resources.Load("E_Bullet") as GameObject;
        EnemyPrefab = Resources.Load<GameObject>("Enemy");
        SwatPrefab = Resources.Load<GameObject>("Swat");

        StartCoroutine(CreateBulletPool());
        StartCoroutine(E_CreateBulletPool());

        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(CreateEnemyPool());
            StartCoroutine(CreateSwatPool());
        }
    }

    private void Start()
    {
        var SpawnPoint = GameObject.Find("SpawnPoints");

        if (SpawnPoint != null)
            SpawnPoint.GetComponentsInChildren<Transform>(SpawnPointList);

        SpawnPointList.RemoveAt(0);

        /* if (SpawnPointList.Count > 0 && PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(CreateEnemy());
            StartCoroutine(CreateSwat());
        } */
    }

    IEnumerator CreateEnemyPool()
    {
        yield return new WaitForSeconds(2.0f);
        GameObject EnemyGroup = new GameObject("EnemyGroup");

        for (int i = 0; i < 10; i++)
        {
            /* var enemyObj = Instantiate(EnemyPrefab, EnemyGroup.transform);
            enemyObj.name = $"{(i + 1).ToString()}명";
            enemyObj.SetActive(false);
            EnemyPoolList.Add(enemyObj); */

            if (PhotonNetwork.IsMasterClient)
            {
                var enemyObj = PhotonNetwork.InstantiateRoomObject(EnemyPrefab.name, Vector3.zero, Quaternion.identity);

                enemyObj.name = $"{(i + 1).ToString()}명";
                enemyObj.SetActive(false);
                EnemyPoolList.Add(enemyObj);
                enemyObj.transform.parent = EnemyGroup.transform;   // 적을 EnemyGroup의 자식으로 설정.
            }
        }
    }

    IEnumerator CreateSwatPool()
    {
        yield return new WaitForSeconds(2.0f);
        GameObject SwatGroup = new GameObject("SwatGroup");

        for (int i = 0; i < 10; i++)
        {
            /* var swatObj = Instantiate(SwatPrefab, SwatGroup.transform);
            swatObj.name = $"{(i + 1).ToString()}명";
            swatObj.SetActive(false);
            SwatPoolList.Add(swatObj); */

            if (PhotonNetwork.IsMasterClient)
            {
                var swatObj = PhotonNetwork.Instantiate(SwatPrefab.name, Vector3.zero, Quaternion.identity);

                swatObj.name = $"{(i + 1).ToString()}명";
                swatObj.SetActive(false);
                SwatPoolList.Add(swatObj);
                swatObj.transform.parent = SwatGroup.transform;   // 적을 SwatGroup 자식으로 설정.
            }
        }
    }

    IEnumerator CreateBulletPool()
    {
        yield return new WaitForSeconds(2.0f);
        GameObject PlayerBulletGroup = new GameObject("PlayerBulletGroup");

        for (int i = 0; i < 50; i++)
        {
            var _bullet = Instantiate(bulletPrefab, PlayerBulletGroup.transform);

            _bullet.name = $"{(i + 1).ToString()}발";
            _bullet.SetActive(false);

            bulletPoolList.Add(_bullet);
        }
    }

    IEnumerator E_CreateBulletPool()
    {
        yield return new WaitForSeconds(2.0f);
        GameObject EnemyBulletGroup = new GameObject("EnemyBulletGroup");

        for (int i = 0; i < maxPool; i++)
        {
            var E_bullet = Instantiate(E_bulletPrefab, EnemyBulletGroup.transform);

            E_bullet.name = $"{(i + 1).ToString()}발";
            E_bullet.SetActive(false);

            E_bulletPoolList.Add(E_bullet);
        }
    }

    public GameObject GetBulletPool()
    {
        for (int i = 0; i < bulletPoolList.Count; i++)
        {
            if (bulletPoolList[i].activeSelf == false)
                return bulletPoolList[i];
        }

        return null;
    }

    public GameObject E_GetBulletPool()
    {
        for (int i = 0; i < E_bulletPoolList.Count; i++)
        {
            if (E_bulletPoolList[i].activeSelf == false)
                return E_bulletPoolList[i];
        }

        return null;
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom called. Player count: " + PhotonNetwork.CurrentRoom.PlayerCount);
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            StartCoroutine(CreateSwat());
            StartCoroutine(CreateEnemy());
        }
    }

    IEnumerator CreateEnemy()
    {
        while (!GameManager.G_Instance.isGameOver)
        {
            yield return new WaitForSeconds(3f);    //3초 간격으로 CreateEnemy 호출

            if (GameManager.G_Instance.isGameOver) yield break; //게임이 종료되면 코루틴을 종료해서 다음 루틴 진행하지 않음

            foreach (GameObject _enemy in EnemyPoolList)
            {
                if (_enemy.activeSelf == false)
                {
                    int idx = Random.Range(0, SpawnPointList.Count);
                    _enemy.transform.position = SpawnPointList[idx].position;
                    _enemy.transform.rotation = SpawnPointList[idx].rotation;
                    Debug.Log(_enemy.transform.position = SpawnPointList[idx].position);
                    _enemy.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }

    IEnumerator CreateSwat()
    {
        while (!GameManager.G_Instance.isGameOver)
        {
            yield return new WaitForSeconds(6f);

            if (GameManager.G_Instance.isGameOver) yield break;

            foreach (GameObject swat in SwatPoolList)
            {
                if (swat.activeSelf == false)
                {
                    int idx = Random.Range(0, SpawnPointList.Count);
                    swat.transform.position = SpawnPointList[idx].position;
                    swat.transform.rotation = SpawnPointList[idx].rotation;
                    swat.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }
}
