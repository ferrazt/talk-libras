using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IconSpeedButtonScript : MonoBehaviour
{
    private TMP_Text speedText;
    BinAnimationLoader binAnimationLoader;

    void Start()
    {
        Button btn = GetComponent<Button>();
        speedText = GetComponentInChildren<TMP_Text>();
        btn.onClick.AddListener(UpdateSpeed);
        speedText.text = "1.5x";
        binAnimationLoader = GetComponent<BinAnimationLoader>();
        if (binAnimationLoader == null)
        {
            // Tentar encontrar em toda a cena
            binAnimationLoader = FindObjectOfType<BinAnimationLoader>();

            if (binAnimationLoader == null)
            {
                Debug.LogError("AnimationRequestHandler não encontrado!");
                return;
            }
        }

        binAnimationLoader.SpeedAvatar(1.5f);
    }

    void UpdateSpeed()
    {
        switch (speedText.text)
        {
            case "1.0x":
                speedText.text = "1.5x";
                binAnimationLoader.SpeedAvatar(1.5f);
                break;
            case "1.5x":
                speedText.text = "2.0x";
                binAnimationLoader.SpeedAvatar(2.0f);
                break;
            case "2.0x":
                speedText.text = "2.5x";
                binAnimationLoader.SpeedAvatar(2.5f);
                break;
            case "2.5x":
                speedText.text = "3.0x";
                binAnimationLoader.SpeedAvatar(3.0f);
                break;
            case "3.0x":
                speedText.text = "1.0x";
                binAnimationLoader.SpeedAvatar(1.0f);
                break;
            default:
                speedText.text = "1.0x";
                binAnimationLoader.SpeedAvatar(1.0f);
                break;
        }
    }
}
