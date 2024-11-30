using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public GameObject[] startActivesObject;
    public GameObject[] startInactiveObjects;
    public bool StartFlash;
    public bool StartDrizzle;
    public bool StartRainstorm;
    public bool StartGameDisplayer;
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

    private void Start()
    {
        lastStep = -1;
        currentStep = -1;
    }

    // Start is called before the first frame update
    //void Awake()
    //{
    //    ResetScene();
    //}

    private void OnEnable()
    {
        ResetScene();
    }

    private void OnDisable()
    {
        MainStageManager.instance.AudioPlayer.Stop();
        MainStageManager.instance.AudioPlayer.clip = null;
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            GoToTheStep(-1);
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PreviousStep();
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextStep();
        }
    }

    public void ResetScene()
    {
        for (int i = 0; i < startActivesObject.Length; i++)
        {
            startActivesObject[i].SetActive(true);
        }
        for (int i = 0;i < startInactiveObjects.Length; i++)
        {
            startInactiveObjects[i].SetActive(false);
        }
        MainStageManager.instance.Flash.SetActive(StartFlash);
        MainStageManager.instance.Drizzle.SetActive(StartDrizzle);
        MainStageManager.instance.RainStorm.SetActive(StartRainstorm);
        MainStageManager.instance.GameStartDisplay.SetActive(StartGameDisplayer);
        currentStep = -1;
    }

    public void NextStep()
    {
        if (currentStep >= steps.Length - 1)
        {
            Debug.Log("最后一步,进入下一场景");
            MainStageManager.instance.GoToTheNextScene();
            lastStep = currentStep+1;
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
            MainStageManager.instance.GoToThePreviousScene();
            lastStep= currentStep-1;
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
            foreach (var item in startActivesObject) { item.SetActive(true); }
            foreach (var item in startInactiveObjects) { item.SetActive(false); }
            MainStageManager.instance.Flash.SetActive(StartFlash);
            MainStageManager.instance.Drizzle.SetActive(StartDrizzle);
            MainStageManager.instance.RainStorm.SetActive(StartRainstorm);
            MainStageManager.instance.GameStartDisplay.SetActive(StartGameDisplayer);
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

    
}
