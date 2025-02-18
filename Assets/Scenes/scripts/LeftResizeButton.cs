using UnityEngine;
using UnityEngine.UI;

public class LeftResizeButton : MonoBehaviour
{
    public RectTransform leftPanel;
    public RectTransform rightPanel;
    public Button button;

    private bool isExpanded = false;

    // Guardar valores originais
    private Vector2 originalAnchorMin;
    private Vector2 originalAnchorMax;
    private Vector2 originalOffsetMin;
    private Vector2 originalOffsetMax;
    private Vector2 originalSize;
    private Vector2 originalPos;

    void Start()
    {
        button.onClick.AddListener(LeftToggleSize);

        // Salvar as configurações originais do LeftPanel
        originalAnchorMin = leftPanel.anchorMin;
        originalAnchorMax = leftPanel.anchorMax;
        originalOffsetMin = leftPanel.offsetMin;
        originalOffsetMax = leftPanel.offsetMax;
        originalSize = leftPanel.sizeDelta;
        originalPos = leftPanel.anchoredPosition;
    }

    void LeftToggleSize()
    {
        if (!isExpanded)
        {
            // Esconder o RightPanel
            rightPanel.gameObject.SetActive(false);

            // Expandir o LeftPanel para ocupar toda a tela
            leftPanel.anchorMin = new Vector2(0, 0);
            leftPanel.anchorMax = new Vector2(1, 1);
            leftPanel.offsetMin = Vector2.zero;
            leftPanel.offsetMax = Vector2.zero;
        }
        else
        {
            // Mostrar o RightPanel
            rightPanel.gameObject.SetActive(true);

            // Restaura o LeftPanel para o tamanho/posição originais
            leftPanel.anchorMin = originalAnchorMin;
            leftPanel.anchorMax = originalAnchorMax;
            leftPanel.offsetMin = originalOffsetMin;
            leftPanel.offsetMax = originalOffsetMax;
            leftPanel.sizeDelta = originalSize;
            leftPanel.anchoredPosition = originalPos;
        }

        isExpanded = !isExpanded;
    }
}
