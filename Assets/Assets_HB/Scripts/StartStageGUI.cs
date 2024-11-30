using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartStageGUI : MonoBehaviour
{
    public Canvas manual;

    public void StartClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("MainStage");
    }

    public void ManualClicked()
    {
        if(!manual.gameObject.activeSelf)
            manual.gameObject.SetActive(true);
    }

    public void EndClicked()
    {
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void BackClicked()
    {
        if (manual.gameObject.activeSelf)
            manual.gameObject.SetActive(false);
    }
}
