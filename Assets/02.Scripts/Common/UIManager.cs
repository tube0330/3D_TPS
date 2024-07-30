using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class UIManager : MonoBehaviour
{
    public void OnClickPlayBtn()
    {
        SceneManager.LoadScene("Level_1");
        SceneManager.LoadScene("BattleFieldScene", LoadSceneMode.Additive); //기존씬을 삭제하지 않고 추가해서 새로운 씬을 로드.
    }

    public void OnClickQuitBtn()
    {
       #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = true;
        #else
            Application.Quit();
        #endif
    }
}
