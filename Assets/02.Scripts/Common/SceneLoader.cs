using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public CanvasGroup fadeCanvasGroup; //fade in 적용할 CanvasGroup
    [Range(0.5f, 2.0f)] public float fadeDuration = 1.0f; //fade in/out duration(적용시간)
    public Dictionary<string, LoadSceneMode> loadScenes = new Dictionary<string, LoadSceneMode>();

    void InitSceneInfo()
    {
        loadScenes.Add("Level_1", LoadSceneMode.Additive);            //Scene combine
        loadScenes.Add("BattleFieldScene", LoadSceneMode.Additive);   //Scene combine
    }

    IEnumerator Start()
    {
        //fadeCanvasGroup = GetComponent<CanvasGroup>();
        InitSceneInfo();
        fadeCanvasGroup.alpha = 1.0f; //canvasGroup alpha 100%

        foreach (var loadScene in loadScenes)
            yield return StartCoroutine(LoadScene(loadScene.Key, loadScene.Value));

        StartCoroutine(Fade(0.0f));
    }

    IEnumerator LoadScene(string sceneName, LoadSceneMode mode)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, mode);  //비동기방식으로 씬을 로드하고 로드가 완료될 때까지 대기
        Scene loadScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(loadScene);
    }

    IEnumerator Fade(float finalAlpha)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Level_1"));    //light map이 깨지는 것을 방지하기 위해 StageScene을 활성화
        fadeCanvasGroup.blocksRaycasts = true;

        float fadeSpeed = Math.Abs(fadeCanvasGroup.alpha - finalAlpha) / fadeDuration;  //절댓값 함수로 백분율 계산
    
        while (!Mathf.Approximately(fadeCanvasGroup.alpha, finalAlpha)) //fadeCanvasGroup.alpha가 finalAlpha와 일치하지 않을때
        {
            fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, finalAlpha, fadeSpeed * Time.deltaTime);
            yield return null;  //한 프레임 다음에
        }

        fadeCanvasGroup.blocksRaycasts = false; //fade in 완료시 raycast block off

        SceneManager.UnloadSceneAsync("SceneLoader");   //비동기적으로 Fade in이 완료된 후 SceneLoader 삭제
    }
}
