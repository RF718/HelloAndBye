using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonManager : MonoBehaviour
{
    [System.Serializable]
    public class ButtonData
    {
        public Button button;               // ��ť����
        public GameObject targetObject;     // ��ť�ƶ���Ŀ��λ��
        public string titlePinYin;          // Ҫ��ʾ ƴ��
        public string titleText;            // Ҫ��ʾ�ı�������
        public string descriptionText;      // Ҫ��ʾ�Ľ�������
        public bool beneficial;             //�Ƿ�Ϊ���
    }

    public ButtonData[] buttons;            // ��ť���飬���������ť������
    public GameObject description;          // ���ܵ�GameObject
    public TextMeshProUGUI titlePinYin;     // ����ƴ����TextMeshPro���
    public TextMeshProUGUI titleText;       // �����TextMeshPro���
    public TextMeshProUGUI descriptionText; // ���ܵ�TextMeshPro���
    public float moveDuration = 2.0f;       // ��ť�ƶ��ĳ���ʱ��
    public Vector3 targetScale = new Vector3(2.0f, 2.0f, 1.0f); // ��ť�ƶ�������Ҫ����Ŀ�����

    [SerializeField]
    private Sprite injuriousDescriptionSprite;
    [SerializeField]
    public Sprite beneficialDescriptionSprite;

    public string successSound;         // �ɹ���Ч
    public string beneficialInsectSound;  // �����Ч
    public string failureSound;          // ʧ����Ч

    private Vector3[] originalPositions;    // ��ť�ĳ�ʼλ��
    private Vector3[] originalScales;       // ��ť�ĳ�ʼ��С
    private ButtonData activeButton;        // ��ǰ�����ƶ��İ�ť����
    private bool isMoving = false;          // �ж��Ƿ����ƶ�
    private bool isReturning = false;       // �ж��Ƿ��ڷ���
    private bool hasMoved = false;          // �жϵ�ǰ�Ƿ��н��ܡ�
    private float elapsedTime;              // ��¼�ƶ�/���ŵ�ʱ��

    void Start()
    {
        // ��ʼ��ԭʼλ�úʹ�С
        originalPositions = new Vector3[buttons.Length];
        originalScales = new Vector3[buttons.Length];

        for (int i = 0; i < buttons.Length; i++)
        {
            RectTransform rect = buttons[i].button.GetComponent<RectTransform>();
            originalPositions[i] = rect.position;
            originalScales[i] = rect.localScale;

            // Ϊÿ����ť��ӵ���¼�
            int index = i; // ����հ�����
            buttons[i].button.onClick.AddListener(() => OnButtonClick(index));
        }

        // ����jieshao
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

        // �������������е�����λ��
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }
    }

    // ��ĳ����ť�����ʱ����
    void OnButtonClick(int index)
    {
        if (!hasMoved)
        {
            activeButton = buttons[index];
            elapsedTime = 0f; // ����ʱ��
            isMoving = true;

            // ����jieshao�еı���ͽ�������
            titlePinYin.text = activeButton.titlePinYin;
            titleText.text = activeButton.titleText;
            descriptionText.text = activeButton.descriptionText;

            // ��ʾ����
            description.GetComponent<Image>().sprite = activeButton.beneficial?beneficialDescriptionSprite:injuriousDescriptionSprite;
            description.SetActive(true);

            // ���ųɹ����������Ч
            MainStageManager.instance.audioManager.PlayAudioEffect(activeButton.beneficial ? successSound  : beneficialInsectSound);
            //audioSource.PlayOneShot(activeButton.beneficial ? successSound : beneficialInsectSound);
        }
    }

    // �������¼�
    void HandleClick()
    {
        // �������δ����
        if (!description.activeSelf)
        {
            bool clickedOnButton = false;

            // ����Ƿ����˰�ť
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
                // ����ʧ����Ч
                MainStageManager.instance.audioManager.PlayAudioEffect(failureSound);
                //audioSource.PlayOneShot(failureSound);
            }
        }
    }

    // �ƶ���ť��Ŀ�����
    void MoveButtonToTarget()
    {
        elapsedTime += Time.deltaTime;

        float t = elapsedTime / moveDuration;

        RectTransform buttonRect = activeButton.button.GetComponent<RectTransform>();

        // ��ֵ�ƶ���Ŀ��λ��
        buttonRect.position = Vector3.Lerp(originalPositions[System.Array.IndexOf(buttons, activeButton)],
                                           activeButton.targetObject.transform.position, t);

        // �������𽥱��
        buttonRect.localScale = Vector3.Lerp(originalScales[System.Array.IndexOf(buttons, activeButton)],
                                             targetScale, t);

        // �ж��Ƿ񵽴�Ŀ��λ��
        if (t >= 1.0f)
        {
            isMoving = false; // ֹͣ�ƶ�
        }
    }

    // ����ť���ص�ԭʼλ��
    public void ReturnButtonToOriginalPosition()
    {
        elapsedTime += Time.deltaTime;
        float t = elapsedTime / moveDuration;

        RectTransform buttonRect = activeButton.button.GetComponent<RectTransform>();

        int index = System.Array.IndexOf(buttons, activeButton);

        // ��ֵ�ƶ��س�ʼλ��
        buttonRect.position = Vector3.Lerp(activeButton.targetObject.transform.position,
                                           originalPositions[index], t);

        // ��ֵ���Ż�ԭʼ��С
        buttonRect.localScale = Vector3.Lerp(targetScale, originalScales[index], t);

        // �ж��Ƿ�ص�ԭʼλ��
        if (t >= 1.0f)
        {
            isReturning = false; // ֹͣ����
            description.SetActive(false); // ����jieshao
            activeButton = null; // �����ǰ���ť
        }
    }

    // ������ð�ťʱ���ô˷���
    public void OnResetButtonClick()
    {
        if (activeButton != null)
        {
            isReturning = true; // ��ʼ���ذ�ť
            activeButton.button.gameObject.SetActive(true); // ���ť
        }
    }
}
