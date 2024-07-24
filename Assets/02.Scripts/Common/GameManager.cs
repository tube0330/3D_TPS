using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        OnCloseClick(false);
    }

    public bool isPause = false;

    public void OnPauseClick()
    {
        isPause = !isPause;

        Time.timeScale = (isPause) ? 0.0f : 1f;

        var playerObj = GameObject.FindGameObjectWithTag("Player");
        var scripts = playerObj.GetComponents<MonoBehaviour>(); //player에 있는 MonoBehaviour를 상속하는 스크립트들을 가져옴

        foreach (var script in scripts)
        {
            script.enabled = !isPause;
        }

        var canvasGroup = GameObject.Find("Panel_Weapon").GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = !isPause;
    }

    public void OnCloseClick(bool isOpen)
    {
        var canvasGroup = GameObject.Find("Inventory").GetComponent<CanvasGroup>();
        
        Time.timeScale = (isOpen) ? 0f : 1f;

        canvasGroup.alpha = (isOpen) ? 1f : 0f;
        canvasGroup.interactable = isOpen;
        canvasGroup.blocksRaycasts = isOpen;
    }
}
