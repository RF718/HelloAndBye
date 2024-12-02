using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public SettingManager settingManager;
    public GameObject mainMenu;
    // Start is called before the first frame update

    private void OnEnable()
    {
        mainMenu.SetActive(true);
        settingManager.gameObject.SetActive(false);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onContinueButtionClicked()
    {
        this.gameObject.SetActive(false);
    }

    public void onSettingButtionClicked()
    {
        mainMenu.SetActive(false);
        settingManager.gameObject.SetActive(true);
    }

    public void OnBackToStartButtonClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("StartStage");
    }

    public void OnEndButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
