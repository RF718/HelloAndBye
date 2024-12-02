using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using Unity.VisualScripting.FullSerializer;
using static UnityEngine.Rendering.DebugUI;

public class MainStageManager : MonoBehaviour
{
    private static MainStageManager _instance;
    public static MainStageManager instance // sometimes instantiated by BountyManagerLoader.ResourceLoadCallback
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<MainStageManager>();
                if (_instance != null)
                {
                    _instance.SetInstance();
                }
            }
            return _instance;
        }
    }

    public void SetInstance()
    {
        _instance = this;
    }



    public AllSceneManager scenesManager;
    public AudioManager audioManager;
    public FlashManager FlashManager;
    public RainManager RainManager;
    public MenuManager MenuManager;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
            MenuManager.gameObject.SetActive(!MenuManager.gameObject.activeSelf);
    }

}
