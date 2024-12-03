using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyBindingController:MonoBehaviour
{
    public string actionName;
    public string description;
    public KeyCode keyCode;
    public Button button;
    public TextMeshProUGUI buttonText;
    public TextMeshProUGUI descriptionText;
    public ButtonSettingManager manager;

    private void Awake()
    {
        descriptionText.text = description;
    }
    public void UpdateUI()
    {
        if (keyCode != KeyCode.None)
            buttonText.text = keyCode.ToString();
        else
            buttonText.text = "N/A";
    }

    public void onKeyBindingClicked()
    {
        StartCoroutine(manager.WaitForKeyPress(this));
    }
}
