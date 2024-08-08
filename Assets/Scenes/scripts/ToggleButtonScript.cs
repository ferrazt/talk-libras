using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class ToggleButtonScript : MonoBehaviour
{
    private Animator animator;
    public bool IsOn = false;
    public IconButtonScript iconButtonScript;
    public AudioClient audioClient;

    void Start()
    {
        animator = GetComponent<Animator>();
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(Toggle);
    }

    public async void Toggle()
    {
        IsOn = !IsOn;
        animator.SetBool("IsOn", IsOn);

        if (iconButtonScript != null)
        {
            iconButtonScript.SetToggle(IsOn);
        }

        if (audioClient != null)
        {
            if (IsOn)
            {
                await audioClient.StartTranscription();
            }
            else
            {
                await audioClient.StopTranscription();
            }
        }
    }

    public void SetToggle(bool state)
    {
        IsOn = state;
        animator.SetBool("IsOn", IsOn);
    }
}
