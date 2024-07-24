using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager G_Instance;
    public bool isGameOver = false;

    void Awake()
    {
        if (G_Instance == null)
            G_Instance = this;

        else if (G_Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {

    }

    private bool isPause;
    public void OnPauseClick()
    {
        isPause = !isPause;
        Time.timeScale = (isPause) ? 0.0f : 1f;
        var playerObj = GameObject.FindGameObjectWithTag("Player");
    }
}
