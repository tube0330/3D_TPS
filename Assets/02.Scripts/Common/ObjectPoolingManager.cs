using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    public static ObjectPoolingManager poolingManager;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject E_bulletPrefab;

    public int maxPool = 10;
    public List<GameObject> bulletPoolList;
    public List<GameObject> E_bulletPoolList;

    void Awake()
    {
        if (poolingManager == null)
            poolingManager = GetComponent<ObjectPoolingManager>();

        else if (poolingManager != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        bulletPrefab = Resources.Load("Bullet") as GameObject;
        E_bulletPrefab = Resources.Load("E_Bullet") as GameObject;

        CreateBulletPool();
        E_CreateBulletPool();
    }

    void CreateBulletPool()
    {
        GameObject PlayerBulletGroup = new GameObject("PlayerBulletGroup");

        for (int i = 0; i < maxPool; i++)
        {
            var _bullet = Instantiate(bulletPrefab, PlayerBulletGroup.transform);

            _bullet.name = $"{(i + 1).ToString()}발";
            _bullet.SetActive(false);

            bulletPoolList.Add(_bullet);
        }
    }

    void E_CreateBulletPool()
    {
        GameObject EnemyBulletGroup = new GameObject("EnemyBulletGroup");

        for (int i = 0; i < 20; i++)
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
}
