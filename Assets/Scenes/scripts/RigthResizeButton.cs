using UnityEngine;
using UnityEngine.UI;

public class RigthResizeButton : MonoBehaviour
{
    public RectTransform leftPanel;  // Referência ao LeftPanel
    public RectTransform rightPanel; // Referência ao RightPanel
    public Canvas canvas;
    public Button button;
    private Vector2 originalSize;    // Tamanho original do RightPanel
    private Vector2 originalPos;     // Posição original do RightPanel
    private bool isExpanded = false; // Estado do painel

    void Start()
    {
        originalSize = rightPanel.sizeDelta;
        originalPos = rightPanel.anchoredPosition;

        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(ToggleSize);
    }

    public void ToggleSize()
    {
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        RectTransform btnSpeed = button.GetComponent<RectTransform>();

        if (isExpanded)
        {
            // Voltar para o tamanho original
            rightPanel.sizeDelta = originalSize;
            rightPanel.anchoredPosition = originalPos;
            leftPanel.gameObject.SetActive(true); // Mostrar o LeftPanel novamente
        }
        else
        {
            // Expandir para ocupar todo o Canvas

            rightPanel.sizeDelta = new Vector2(canvasRect.rect.x, canvasRect.rect.y);
            btnSpeed.position = new Vector3(-1320, 497, 0);
            rightPanel.anchoredPosition = new Vector2(0, rightPanel.anchoredPosition.y);
            leftPanel.gameObject.SetActive(false); // Esconder o LeftPanel
        }

        isExpanded = !isExpanded;
    }
}
