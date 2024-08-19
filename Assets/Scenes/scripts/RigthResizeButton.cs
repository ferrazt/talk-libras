using UnityEngine;
using UnityEngine.UI;

public class RightResizeButton : MonoBehaviour
{
    public RectTransform leftPanel;  // Refer�ncia ao LeftPanel
    public RectTransform rightPanel; // Refer�ncia ao RightPanel
    public Button button;
    private Vector2 originalSize;    // Tamanho original do RightPanel
    private Vector2 originalPos;     // Posi��o original do RightPanel
    private Vector2 originalOffsetMin; // Original offsetMin
    private Vector2 originalOffsetMax; // Original offsetMax
    private bool isExpanded = false; // Estado do painel

    void Start()
    {
        // Salvar os valores originais
        originalSize = rightPanel.sizeDelta;
        originalPos = rightPanel.anchoredPosition;
        originalOffsetMin = rightPanel.offsetMin;
        originalOffsetMax = rightPanel.offsetMax;

        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(ToggleSize);
    }

    public void ToggleSize()
    {
        if (isExpanded)
        {
            // Voltar para o tamanho original
            rightPanel.offsetMin = originalOffsetMin;
            rightPanel.offsetMax = originalOffsetMax;
            rightPanel.sizeDelta = originalSize;
            rightPanel.anchoredPosition = originalPos;
            leftPanel.gameObject.SetActive(true); // Mostrar o LeftPanel novamente
        }
        else
        {
            // Esconder o LeftPanel e expandir o RightPanel
            leftPanel.gameObject.SetActive(false);

            // Definir offsetMin para expandir o RightPanel � esquerda
            rightPanel.offsetMin = new Vector2(-leftPanel.rect.width, rightPanel.offsetMin.y); // Ajusta o lado esquerdo
            rightPanel.offsetMax = new Vector2(0, rightPanel.offsetMax.y); // Mant�m o lado direito no lugar
        }

        isExpanded = !isExpanded;
    }
}
