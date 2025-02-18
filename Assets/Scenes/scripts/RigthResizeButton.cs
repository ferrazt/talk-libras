using UnityEngine;
using UnityEngine.UI;

public class RightResizeButton : MonoBehaviour
{
    public RectTransform rightPanel; // Referência ao painel direito que será expandido
    public RectTransform leftPanel;  // Referência ao painel esquerdo que será ocultado
    public Button button;

    private bool isExpanded = false;

    // Guardar os valores originais do RightPanel
    private Vector2 originalAnchorMin;
    private Vector2 originalAnchorMax;
    private Vector2 originalOffsetMin;
    private Vector2 originalOffsetMax;
    private Vector2 originalSize;
    private Vector2 originalPos;

    void Start()
    {
        button.onClick.AddListener(RightToggleSize);

        // Salvar as configurações originais do RightPanel
        originalAnchorMin = rightPanel.anchorMin;
        originalAnchorMax = rightPanel.anchorMax;
        originalOffsetMin = rightPanel.offsetMin;
        originalOffsetMax = rightPanel.offsetMax;
        originalSize = rightPanel.sizeDelta;
        originalPos = rightPanel.anchoredPosition;
    }

    void RightToggleSize()
    {
        if (!isExpanded)
        {
            // 1) Ocultar o painel esquerdo
            leftPanel.gameObject.SetActive(false);

            // 2) Expandir o painel direito para ocupar toda a tela
            rightPanel.anchorMin = new Vector2(0, 0);
            rightPanel.anchorMax = new Vector2(1, 1);
            rightPanel.offsetMin = Vector2.zero;
            rightPanel.offsetMax = Vector2.zero;
        }
        else
        {
            // 1) Reexibir o painel esquerdo
            leftPanel.gameObject.SetActive(true);

            // 2) Restaurar as configurações originais do painel direito
            rightPanel.anchorMin = originalAnchorMin;
            rightPanel.anchorMax = originalAnchorMax;
            rightPanel.offsetMin = originalOffsetMin;
            rightPanel.offsetMax = originalOffsetMax;
            rightPanel.sizeDelta = originalSize;
            rightPanel.anchoredPosition = originalPos;
        }

        isExpanded = !isExpanded;
    }
}
