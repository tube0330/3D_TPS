using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager G_Instance;
    public bool isGameOver = false;
    public Text killTxt;
    public int killCnt = 0;

    void Awake()
    {
        if (G_Instance == null)
            G_Instance = this;

        else if (G_Instance != this)
            Destroy(gameObject);

        killTxt = GameObject.Find("Canvas_UI").transform.GetChild(7).GetComponent<Text>();

        DontDestroyOnLoad(gameObject);
        OnCloseClick(false);
        LoadGameData();
    }

    void LoadGameData()
    {
        killCnt = PlayerPrefs.GetInt("KILLCOUNT", 0);
        killTxt.text = $"<color=#ff0000>KILL</color> " + killCnt.ToString("0000");    //자릿수설정
       
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

    public void KillScore()
    {
        ++killCnt;
        killTxt.text = $"<color=#ff0000>KILL</color> " + killCnt.ToString("0000");

        PlayerPrefs.SetInt("KILLCOUNT", killCnt);
    }

    void OnDisable()    //게임 종료하면 자동으로 호출
    {
        PlayerPrefs.DeleteKey("KILLCOUNT");
    }
}
