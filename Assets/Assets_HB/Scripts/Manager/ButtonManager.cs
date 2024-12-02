using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonManager : MonoBehaviour
{
    [System.Serializable]
    public class ButtonData
    {
        public Button button;               // 按钮对象
        public GameObject targetObject;     // 按钮移动的目标位置
        public string titlePinYin;          // 要显示 拼音
        public string titleText;            // 要显示的标题文字
        public string descriptionText;      // 要显示的介绍文字
        public bool beneficial;             //是否为益虫
    }

    public ButtonData[] buttons;            // 按钮数组，包含多个按钮的数据
    public GameObject description;          // 介绍的GameObject
    public TextMeshProUGUI titlePinYin;     // 标题拼音的TextMeshPro组件
    public TextMeshProUGUI titleText;       // 标题的TextMeshPro组件
    public TextMeshProUGUI descriptionText; // 介绍的TextMeshPro组件
    public float moveDuration = 2.0f;       // 按钮移动的持续时间
    public Vector3 targetScale = new Vector3(2.0f, 2.0f, 1.0f); // 按钮移动过程中要变大的目标比例

    [SerializeField]
    private Sprite injuriousDescriptionSprite;
    [SerializeField]
    public Sprite beneficialDescriptionSprite;

    public string successSound;         // 成功音效
    public string beneficialInsectSound;  // 益虫音效
    public string failureSound;          // 失败音效

    private Vector3[] originalPositions;    // 按钮的初始位置
    private Vector3[] originalScales;       // 按钮的初始大小
    private ButtonData activeButton;        // 当前正在移动的按钮数据
    private bool isMoving = false;          // 判断是否在移动
    private bool isReturning = false;       // 判断是否在返回
    private bool hasMoved = false;          // 判断当前是否有介绍。
    private float elapsedTime;              // 记录移动/缩放的时间

    void Start()
    {
        // 初始化原始位置和大小
        originalPositions = new Vector3[buttons.Length];
        originalScales = new Vector3[buttons.Length];

        for (int i = 0; i < buttons.Length; i++)
        {
            RectTransform rect = buttons[i].button.GetComponent<RectTransform>();
            originalPositions[i] = rect.position;
            originalScales[i] = rect.localScale;

            // 为每个按钮添加点击事件
            int index = i; // 避免闭包问题
            buttons[i].button.onClick.AddListener(() => OnButtonClick(index));
        }

        // 隐藏jieshao
        description.SetActive(false);
    }

    void Update()
    {   
        if (MainStageManager.instance.MenuManager.gameObject.activeSelf||MainStageManager.instance.scenesManager.IsGameStartActive())
            return;
        
        if (isMoving && activeButton != null)
        {
            MoveButtonToTarget();
        }

        if (isReturning && activeButton != null)
        {
            ReturnButtonToOriginalPosition();
        }

        // 检测鼠标点击场景中的任意位置
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }
    }

    // 当某个按钮被点击时触发
    void OnButtonClick(int index)
    {
        if (!hasMoved)
        {
            activeButton = buttons[index];
            elapsedTime = 0f; // 重置时间
            isMoving = true;

            // 设置jieshao中的标题和介绍文字
            titlePinYin.text = activeButton.titlePinYin;
            titleText.text = activeButton.titleText;
            descriptionText.text = activeButton.descriptionText;

            // 显示介绍
            description.GetComponent<Image>().sprite = activeButton.beneficial?beneficialDescriptionSprite:injuriousDescriptionSprite;
            description.SetActive(true);

            // 播放成功或者益虫音效
            MainStageManager.instance.audioManager.PlayAudioEffect(activeButton.beneficial ? successSound  : beneficialInsectSound);
            //audioSource.PlayOneShot(activeButton.beneficial ? successSound : beneficialInsectSound);
        }
    }

    // 处理点击事件
    void HandleClick()
    {
        // 如果介绍未出现
        if (!description.activeSelf)
        {
            bool clickedOnButton = false;

            // 检查是否点击了按钮
            foreach (var buttonData in buttons)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(
                    buttonData.button.GetComponent<RectTransform>(),
                    Input.mousePosition,
                    Camera.main))
                {
                    clickedOnButton = true;
                    break;
                }
            }

            if (!clickedOnButton)
            {
                // 播放失败音效
                MainStageManager.instance.audioManager.PlayAudioEffect(failureSound);
                //audioSource.PlayOneShot(failureSound);
            }
        }
    }

    // 移动按钮到目标对象
    void MoveButtonToTarget()
    {
        elapsedTime += Time.deltaTime;

        float t = elapsedTime / moveDuration;

        RectTransform buttonRect = activeButton.button.GetComponent<RectTransform>();

        // 插值移动到目标位置
        buttonRect.position = Vector3.Lerp(originalPositions[System.Array.IndexOf(buttons, activeButton)],
                                           activeButton.targetObject.transform.position, t);

        // 按比例逐渐变大
        buttonRect.localScale = Vector3.Lerp(originalScales[System.Array.IndexOf(buttons, activeButton)],
                                             targetScale, t);

        // 判断是否到达目标位置
        if (t >= 1.0f)
        {
            isMoving = false; // 停止移动
        }
    }

    // 将按钮返回到原始位置
    public void ReturnButtonToOriginalPosition()
    {
        elapsedTime += Time.deltaTime;
        float t = elapsedTime / moveDuration;

        RectTransform buttonRect = activeButton.button.GetComponent<RectTransform>();

        int index = System.Array.IndexOf(buttons, activeButton);

        // 插值移动回初始位置
        buttonRect.position = Vector3.Lerp(activeButton.targetObject.transform.position,
                                           originalPositions[index], t);

        // 插值缩放回原始大小
        buttonRect.localScale = Vector3.Lerp(targetScale, originalScales[index], t);

        // 判断是否回到原始位置
        if (t >= 1.0f)
        {
            isReturning = false; // 停止返回
            description.SetActive(false); // 隐藏jieshao
            activeButton = null; // 清除当前活动按钮
        }
    }

    // 点击重置按钮时调用此方法
    public void OnResetButtonClick()
    {
        if (activeButton != null)
        {
            isReturning = true; // 开始返回按钮
            activeButton.button.gameObject.SetActive(true); // 激活按钮
        }
    }
}
