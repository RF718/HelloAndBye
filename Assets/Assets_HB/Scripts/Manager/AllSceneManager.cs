using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

public class AllSceneManager : MonoBehaviour
{
    public SceneManager[] scenes;
    public SceneManager startScene;
    public GameObject GameStartDisplay;
    public Image BlackFade; // 黑色遮罩，用于淡入淡出
    public float fadeDuration = 0.5f; // 淡入淡出的持续时间

    [fsIgnore]
    private SceneManager activeScene;
    [fsIgnore]
    private int lastSceneId;

    public void ResetAllCanvas()
    {
        for (int i = 0; i < scenes.Length; i++)
        {
            scenes[i].ResetScene();
        }
    }

    private void Revoke()
    {
        GoToTheScene(lastSceneId, true);
    }

    private void GoToTheScene(int sceneId, bool revoke = false)
    {
        if (scenes.Length > 0 && sceneId >= 0 && sceneId < scenes.Length)
        {
            StartCoroutine(FadeOutIn(() =>
            {
                activeScene.gameObject.SetActive(false);
                lastSceneId = Array.IndexOf(scenes, activeScene);
                scenes[sceneId].gameObject.SetActive(true);
                activeScene = scenes[sceneId];

                if (revoke)
                {
                    activeScene.GoToTheStep(activeScene.LastStep, true);
                }
            }));

        }
    }

    public void GoToTheNextScene()
    {
        var currentSceneId = Array.IndexOf(scenes, activeScene);
        if (currentSceneId >= scenes.Length - 2)
            return;
        GoToTheScene(++currentSceneId);
    }

    public void GoToThePreviousScene()
    {
        var currentSceneId = Array.IndexOf(scenes, activeScene);
        if (currentSceneId == 0)
            return;
        GoToTheScene(--currentSceneId, true);
    }

    void Start()
    {
        startScene.gameObject.SetActive(true);
        activeScene = startScene;
        //bool flag = true;
        //if (scenes.Length > 0)
        //{
        //    for (int i = 0; i < scenes.Length; i++)
        //    {
        //        if (flag && scenes[i] == startScene)
        //        {
        //            scenes[i].gameObject.SetActive(true);
        //            flag = false;
        //        }
        //        else
        //        {
        //            scenes[i].gameObject.SetActive(false);
        //        }
        //    }
        //    activeScene = scenes[0];
        //}
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1)) GoToTheScene(0);
        if (Input.GetKeyDown(KeyCode.F2)) GoToTheScene(1);
        if (Input.GetKeyDown(KeyCode.F3)) GoToTheScene(2);
        if (Input.GetKeyDown(KeyCode.F4)) GoToTheScene(3);
        if (Input.GetKeyDown(KeyCode.F5)) GoToTheScene(4);
        if (Input.GetKeyDown(KeyCode.F6)) GoToTheScene(5);
        if (Input.GetKeyDown(KeyCode.F7)) GoToTheScene(6);
        if (Input.GetKeyDown(KeyCode.F8)) GoToTheScene(7);
        if (Input.GetKeyDown(KeyCode.F9)) GoToTheScene(8);
        if (Input.GetKeyDown(KeyCode.F10)) GoToTheScene(9);
        if (Input.GetKeyDown(KeyCode.F11)) GoToTheScene(10);
        if (Input.GetKeyDown(KeyCode.F12)) GoToTheScene(11);
        if (Input.GetKeyDown(KeyCode.B)) GoToTheScene(12);
        if (Input.GetKeyDown(KeyCode.Backspace)) Revoke();
        if (Input.GetKeyDown(KeyCode.Escape)) UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("StartStage");
    }

    // 淡入淡出协程
    IEnumerator FadeOutIn(Action onFadeComplete)
    {
        // 淡出
        yield return StartCoroutine(Fade(1f));

        // 完成淡出，执行场景切换逻辑
        onFadeComplete?.Invoke();

        // 淡入
        yield return StartCoroutine(Fade(0f));
    }

    // 控制遮罩的透明度
    IEnumerator Fade(float targetAlpha)
    {
        BlackFade.gameObject.SetActive(true);
        float startAlpha = BlackFade.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            BlackFade.color = new Color(BlackFade.color.r, BlackFade.color.g, BlackFade.color.b, newAlpha);
            yield return null;
        }

        // 确保最终透明度完全到达目标值
        BlackFade.color = new Color(BlackFade.color.r, BlackFade.color.g, BlackFade.color.b, targetAlpha);
        BlackFade.gameObject.SetActive(false);
    }

    IEnumerator FadeIn()
    {


        yield return new WaitForSecondsRealtime(1f);

        yield return StartCoroutine(Fade(0f));
    }

    public void HouseFade()
    {
        BlackFade.gameObject.SetActive(true);
        BlackFade.color = new Color(BlackFade.color.r, BlackFade.color.g, BlackFade.color.b, 1);

        StartCoroutine(FadeIn());
    }
}
