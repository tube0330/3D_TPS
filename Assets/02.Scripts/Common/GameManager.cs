using System.Collections;
using System.Collections.Generic;
using DataInfo;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager G_Instance;
    public bool isGameOver = false;
    public Text killTxt;
    public int killCnt = 0;

    [Header("Datamanager")]
    [SerializeField] DataManager dataManager;
    [SerializeField] GameData gameData;

    void Awake()
    {
        if (G_Instance == null)
            G_Instance = this;

        else if (G_Instance != this)
            Destroy(gameObject);

        dataManager = GetComponent<DataManager>();
        dataManager.Initialize();

        killTxt = GameObject.Find("Canvas_UI").transform.GetChild(7).GetComponent<Text>();

        DontDestroyOnLoad(gameObject);
        OnCloseClick(false);
        LoadGameData();
    }

    void LoadGameData()
    {
        //killCnt = PlayerPrefs.GetInt("KILLCOUNT", 0);

        #region 하드디스크에 저장된 데이터 넘어오는중
        GameData data = dataManager.Load();
        gameData.HP = data.HP;
        gameData.damage = data.damage;
        gameData.killcnt = data.killcnt;
        gameData.equipItem = data.equipItem;
        gameData.speed = data.speed;
        #endregion

        //killTxt.text = $"<color=#ff0000>KILL</color> " + killCnt.ToString("0000");    //자릿수설정

        killTxt.text = $"<color=#ff0000>KILL</color> " + gameData.killcnt.ToString("0000");
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
        /* ++killCnt;
        killTxt.text = $"<color=#ff0000>KILL</color> " + killCnt.ToString("0000");

        PlayerPrefs.SetInt("KILLCOUNT", killCnt); */

        gameData.killcnt++;
        killTxt.text = $"<color=#ff0000>KILL</color> " + gameData.killcnt.ToString("0000");
        PlayerPrefs.SetInt("KILLCOUNT", killCnt);
    }

    void OnDisable()    //게임 종료하면 자동으로 호출
    {
        PlayerPrefs.DeleteKey("KILLCOUNT");
    }
}
