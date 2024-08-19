using UnityEngine;
using TMPro;

public class LeftPanelTextManager : MonoBehaviour
{
    public TextMeshProUGUI panelText;

    void Start()
    {
        if (panelText == null)
        {
            Debug.LogError("Panel Text is not assigned.");
        }
    }

    public void UpdateText(string newText)
    {
        if (panelText != null && panelText.text != "")
        {
            panelText.text += "\n" + newText;
        }
        else
        {
            panelText.text = newText;
        }
    }
}
