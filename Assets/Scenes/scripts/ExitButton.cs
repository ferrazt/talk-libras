using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    public Button exitButton;

    void Start()
    {
        // Conecta o m�todo ExitApp ao clique do bot�o
        exitButton.onClick.AddListener(ExitApp);
    }

    void ExitApp()
    {
#if UNITY_EDITOR
        // Para o modo Play dentro do Editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Fecha o aplicativo em um build normal
        Application.Quit();
#endif
    }
}
