using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    private MainStageManager mainStageManager;

    public GameObject[] startActivesObject;
    public GameObject[] startInactiveObjects;
    public bool StartFlash;
    public bool StartDrizzle;
    public bool StartRainstorm;
    public bool StartGameDisplayer;
    public bool endQuietly = true;
    public CountdownTimer CountDownTimer;
    public float countdownTime;
    public SceneStep[] steps;

    [fsIgnore]
    private int lastStep;

    [fsIgnore]
    private int currentStep;

    [fsIgnore]
    public int LastStep
    {
        get=>lastStep; set => lastStep = value;
    }

    private void Awake()
    {
        mainStageManager = MainStageManager.instance;
    }

    private void Start()
    {
        lastStep = -1;
        currentStep = -1;
    }

    private void OnEnable()
    {
        ResetScene();
    }

    private void OnDisable()
    {
        mainStageManager.audioManager.ResetChangeable();

        if (endQuietly)
        {
            mainStageManager.audioManager.StopBgmPlayer();
            mainStageManager.audioManager.StopAllAudioEffect();

            mainStageManager.FlashManager.SwitchFlashManager(false);
            mainStageManager.RainManager.SwitchDrizzle(false);
            mainStageManager.RainManager.SwitchRainStrom(false);
            
        }

        if(CountDownTimer!=null&&CountDownTimer.gameObject.activeSelf)
            CountDownTimer.gameObject.SetActive(false);
    }



    private void Update()
    {
        if (MainStageManager.instance.MenuManager.gameObject.activeSelf)
            return;
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GoToTheStep(-1);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PreviousStep();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextStep();
        }

        if (CountDownTimer != null)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                ShowCountDownTimer();
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                CountDownTimer.PauseOrResumeCountDown();
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                CountDownTimer.CountDownTimeDisable();
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                CountDownTimer.ResetCountDown();
            }
        }

    }

    public void ResetScene()
    {
        lastStep = currentStep;
        for (int i = 0; i < startActivesObject.Length; i++)
        {
            startActivesObject[i].SetActive(true);
        }
        for (int i = 0;i < startInactiveObjects.Length; i++)
        {
            startInactiveObjects[i].SetActive(false);
        }
        mainStageManager.FlashManager.SwitchFlashManager(StartFlash);
        mainStageManager.RainManager.SwitchDrizzle(StartDrizzle);
        mainStageManager.RainManager.SwitchRainStrom(StartRainstorm);
        mainStageManager.scenesManager.GameStartDisplay.SetActive(StartGameDisplayer);
        if (StartGameDisplayer)
            mainStageManager.audioManager.SetAndPlayBgmPlayer("gameStart");
        currentStep = -1;
    }

    public void NextStep()
    {
        if (currentStep >= steps.Length - 1)
        {
            Debug.Log("最后一步,进入下一场景");
            mainStageManager.scenesManager.GoToTheNextScene();
            lastStep = currentStep;
            return;
        }
        lastStep = currentStep++;
        Debug.Log("下一步： " + currentStep);
        GoToTheStep(currentStep);
    }

    public void PreviousStep()
    {
        if (currentStep == -1)
        {
            Debug.Log("初始状态");
            mainStageManager.scenesManager.GoToThePreviousScene();
            lastStep= currentStep;
            return;
        }
        lastStep = currentStep--;
        Debug.Log("返回： " + currentStep);
        GoToTheStep(currentStep);
    }
    
    public void GoToTheStep(int step,bool jump = false)
    {
        if (step == -1)
        {
            ResetScene();
        }
        if (steps.Length>0&&step >= 0 && step < steps.Length)
        {
            steps[step].Execute();
        }
        if (jump)
        {
            lastStep = currentStep;
            currentStep=step;
        }
        
    }

    public void ShowCountDownTimer()
    {
        CountDownTimer.ShowAndSetCountDown(countdownTime);
    }
}
