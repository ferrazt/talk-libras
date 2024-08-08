using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SendButtonScript : MonoBehaviour
{
    public TMP_InputField inputField;
    public Button sendButton;
    public LeftPanelTextManager leftPanelTextManager;
    public BinAnimationLoader binAnimationLoader;

    private AnimationRequestHandler animationRequestHandler;

    void Start()
    {
        if (inputField == null)
            Debug.LogError("TMP_InputField not found");
        else if (sendButton == null)
            Debug.LogError("Button not assigned.");
        else if (binAnimationLoader == null)
            Debug.LogError("BinAnimationLoader not found.");
        else
        {
            // Inicializa o AnimationRequestHandler com o componente MonoBehaviour correto
            animationRequestHandler = gameObject.AddComponent<AnimationRequestHandler>();
            animationRequestHandler.Initialize(leftPanelTextManager, binAnimationLoader);
            sendButton.onClick.AddListener(SendTextAsync);
            inputField.onSubmit.AddListener(delegate { SendTextAsync(); });
        }
    }

    void SendTextAsync()
    {
        if (inputField.text != "")
        {
            string text = inputField.text;
            inputField.text = "";

            if (leftPanelTextManager != null)
            {
                leftPanelTextManager.UpdateText(text);
            }

            animationRequestHandler.SendTextAndHandleAnimation(text);
        }
    }
}
